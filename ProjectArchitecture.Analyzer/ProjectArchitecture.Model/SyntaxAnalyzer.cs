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

    internal static class SyntaxAnalyzer {


        // Project
        public static bool IsProject(ClassDeclarationSyntax @class) {
            return @class.IsPartial() && @class.IsChildOf( "ProjectArchNode" );
        }
        public static ProjectInfo GetProjectInfo(ClassDeclarationSyntax @class) {
            var type = @class.Identifier.ValueText;
            var modules = @class.GetModules().ToArray();
            return new ProjectInfo( type, modules );
        }
        // Module
        public static bool IsModule(ClassDeclarationSyntax @class) {
            return @class.IsPartial() && @class.IsChildOf( "ModuleArchNode" );
        }
        public static ModuleInfo GetModuleInfo(ClassDeclarationSyntax @class) {
            var type = @class.Identifier.ValueText;
            var @namespaces = @class.GetNamespacesGroupsTypes().GetNamespaceHierarchy().ToArray();
            return new ModuleInfo( type, namespaces );
        }


        // Helpers/GetModules
        private static IEnumerable<ModuleEntry> GetModules(this ClassDeclarationSyntax @class) {
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
        private static IEnumerable<object> GetNamespacesGroupsTypes(this ClassDeclarationSyntax @class) {
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
        private static IEnumerable<NamespaceEntry> GetNamespaceHierarchy(this IEnumerable<object> namespaces) {
            return namespaces.Unflatten( i => i is NamespaceEntry ).Select( i => GetNamespace( (NamespaceEntry?) i.Key, i.Children.ToArray() ) );
        }
        private static NamespaceEntry GetNamespace(NamespaceEntry? @namespace, object[] groups) {
            var namespace_ = @namespace ?? new NamespaceEntry( "Global", null! );
            var groups_ = groups.GetGroupHierarchy().ToArray();
            return new NamespaceEntry( namespace_.Name, groups_ );
        }
        private static IEnumerable<GroupEntry> GetGroupHierarchy(this object[] groups) {
            return groups.Unflatten( i => i is GroupEntry ).Select( i => GetGroup( (GroupEntry?) i.Key, i.Children.ToArray() ) );
        }
        private static GroupEntry GetGroup(GroupEntry? group, object[] types) {
            var group_ = group ?? new GroupEntry( "Default", null! );
            var types_ = types.Cast<TypeEntry>().ToArray();
            return new GroupEntry( group_.Name, types_ );
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
        private static ModuleEntry GetModule(SyntaxNode syntax) {
            var module = ((TypeOfExpressionSyntax) syntax).Type.ToString();
            return new ModuleEntry( module );
        }
        private static NamespaceEntry GetNamespace(SyntaxNode syntax) {
            var @namespace = ((LiteralExpressionSyntax) syntax).Token.ValueText;
            return new NamespaceEntry( @namespace, null! );
        }
        private static GroupEntry GetGroup(SyntaxNode syntax) {
            var comment = syntax.GetLeadingTrivia().Where( i => i.Kind() is SyntaxKind.SingleLineCommentTrivia or SyntaxKind.SingleLineDocumentationCommentTrivia ).LastOrDefault();
            var group = comment.ToString().GetCommentContent();
            return new GroupEntry( group, null! );
        }
        private static TypeEntry GetType(SyntaxNode syntax) {
            var type = ((TypeOfExpressionSyntax) syntax).Type.ToString();
            return new TypeEntry( type );
        }
        // Helpers/String
        private static string GetCommentContent(this string comment) {
            var content = comment.SkipWhile( i => i == '/' );
            return string.Concat( content ).Trim();
        }


    }
}
