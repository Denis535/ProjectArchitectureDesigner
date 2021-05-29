// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal record ProjectInfo(string Name, ModuleNode[] Modules);
    internal record ModuleInfo(string Name, NamespaceNode[] Namespaces);
    internal abstract record Node();
    internal record ModuleNode(string Name) : Node();
    internal record NamespaceNode(string Name, GroupNode[] Groups) : Node();
    internal record GroupNode(string Name, TypeNode[] Types) : Node();
    internal record TypeNode(string Name) : Node();

    internal static class SyntaxAnalyzer {


        // Project
        public static bool IsProject(ClassDeclarationSyntax @class) {
            return @class.IsPartial() && @class.IsChildOf( "ProjectArchNode" );
        }
        public static ProjectInfo GetProjectInfo(ClassDeclarationSyntax @class) {
            var name = @class.Identifier.ValueText.WithoutPrefix( "Project_" ).Replace( '_', '.' );
            var modules = @class?.GetModules().ToArray() ?? Array.Empty<ModuleNode>();
            return new ProjectInfo( name, modules );
        }
        // Module
        public static bool IsModule(ClassDeclarationSyntax @class) {
            return @class.IsPartial() && @class.IsChildOf( "ModuleArchNode" );
        }
        public static ModuleInfo GetModuleInfo(ClassDeclarationSyntax @class) {
            var name = @class.Identifier.ValueText.WithoutPrefix( "Module_" ).Replace( '_', '.' );
            var @namespaces = @class?.GetNamespacesGroupsTypes().GetNamespaceHierarchy().ToArray() ?? Array.Empty<NamespaceNode>();
            return new ModuleInfo( name, namespaces );
        }


        // Helpers/GetModules
        private static IEnumerable<ModuleNode> GetModules(this ClassDeclarationSyntax @class) {
            var method = @class.Members.OfType<MethodDeclarationSyntax>().FirstOrDefault( i => i.Identifier.ValueText == "DefineChildren" );
            var body = (SyntaxNode?) method?.Body ?? method?.ExpressionBody;
            if (body == null) yield break;
            foreach (var node in body.DescendantNodes()) {
                if (IsModule( node )) {
                    yield return GetModule( node );
                }
            }
        }
        // Helpers/GetNamespacesGroupsTypes
        private static IEnumerable<Node> GetNamespacesGroupsTypes(this ClassDeclarationSyntax @class) {
            var method = @class.Members.OfType<MethodDeclarationSyntax>().FirstOrDefault( i => i.Identifier.ValueText == "DefineChildren" );
            var body = (SyntaxNode?) method?.Body ?? method?.ExpressionBody;
            if (body == null) yield break;
            foreach (var node in body.DescendantNodes()) {
                if (IsNamespace( node )) {
                    yield return GetNamespace( node );
                } else
                if (IsType( node )) {
                    if (HasGroup( node )) {
                        yield return GetGroup( node );
                    }
                    yield return GetType( node );
                }
            }
        }
        private static IEnumerable<NamespaceNode> GetNamespaceHierarchy(this IEnumerable<Node> namespaces) {
            return namespaces.Unflatten( i => i is NamespaceNode ).Select( i => GetNamespace( (NamespaceNode?) i.Key, i.Children ) );
        }
        private static NamespaceNode GetNamespace(NamespaceNode? @namespace, Node[] groups) {
            var namespace_ = @namespace ?? new NamespaceNode( "Global", null! );
            var groups_ = groups.GetGroupHierarchy().ToArray();
            return new NamespaceNode( namespace_.Name, groups_ );
        }
        private static IEnumerable<GroupNode> GetGroupHierarchy(this Node[] groups) {
            return groups.Unflatten( i => i is GroupNode ).Select( i => GetGroup( (GroupNode?) i.Key, i.Children ) );
        }
        private static GroupNode GetGroup(GroupNode? group, Node[] types) {
            var group_ = group ?? new GroupNode( "Default", null! );
            var types_ = types.Cast<TypeNode>().ToArray();
            return new GroupNode( group_.Name, types_ );
        }
        // Helpers/IsNode
        private static bool IsModule(SyntaxNode syntax) {
            return syntax is TypeOfExpressionSyntax;
        }
        private static bool IsNamespace(SyntaxNode syntax) {
            return syntax is LiteralExpressionSyntax literal && literal.Kind() == SyntaxKind.StringLiteralExpression;
        }
        private static bool HasGroup(SyntaxNode syntax) {
            // Note: SyntaxTrivia.ToString() doesn't return documentation comment!
            // Note: So, one should use SyntaxTrivia.ToFullString()!
            var comment = syntax.GetLeadingTrivia().Where( i => i.Kind() is SyntaxKind.SingleLineCommentTrivia or SyntaxKind.SingleLineDocumentationCommentTrivia ).LastOrDefault().ToFullString();
            return comment.StartsWith( "// " ) || comment.StartsWith( "/// " );
        }
        private static bool IsType(SyntaxNode syntax) {
            return syntax is TypeOfExpressionSyntax;
        }
        // Helpers/GetNode
        private static ModuleNode GetModule(SyntaxNode syntax) {
            var module = ((TypeOfExpressionSyntax) syntax).Type.ToString();
            return new ModuleNode( module );
        }
        private static NamespaceNode GetNamespace(SyntaxNode syntax) {
            var @namespace = ((LiteralExpressionSyntax) syntax).Token.ValueText;
            return new NamespaceNode( @namespace, null! );
        }
        private static GroupNode GetGroup(SyntaxNode syntax) {
            var comment = syntax.GetLeadingTrivia().Where( i => i.Kind() is SyntaxKind.SingleLineCommentTrivia or SyntaxKind.SingleLineDocumentationCommentTrivia ).LastOrDefault();
            var group = comment.ToString().GetCommentContent();
            return new GroupNode( group, null! );
        }
        private static TypeNode GetType(SyntaxNode syntax) {
            var type = ((TypeOfExpressionSyntax) syntax).Type.ToString();
            return new TypeNode( type );
        }
        // Helpers/String
        private static string GetCommentContent(this string comment) {
            var content = comment.SkipWhile( i => i == '/' );
            return string.Concat( content ).Trim();
        }
        private static string WithoutPrefix(this string value, string prefix) {
            var i = value.IndexOf( prefix );
            if (i != -1) value = value.Substring( i + prefix.Length );
            return value;
        }


    }
}
