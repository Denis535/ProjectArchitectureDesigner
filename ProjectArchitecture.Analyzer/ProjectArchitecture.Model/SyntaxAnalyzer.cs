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
        public record ProjectData(string Name, Module[] Modules) {
            public override string ToString() => string.Format( "{0}: {1}", GetType().Name, Name );
        }
        public record ModuleData(string Name, Namespace[] Namespaces) {
            public override string ToString() => string.Format( "{0}: {1}", GetType().Name, Name );
        }
        public abstract record Node(string Name) {
            public override string ToString() => string.Format( "{0}: {1}", GetType().Name, Name );
        }
        public record Module(string Name) : Node( Name );
        public record Namespace(string Name, Group[] Groups) : Node( Name );
        public record Group(string Name, Type[] Types) : Node( Name );
        public record Type(string Name) : Node( Name );


        // Project
        public static ProjectData GetProjectData(ClassDeclarationSyntax @class) {
            var name = @class.Identifier.ValueText.WithoutPrefix( "Project_" ).Replace( '_', '.' );
            var modules = @class?.GetModules().ToArray() ?? Array.Empty<Module>();
            return new ProjectData( name, modules );
        }
        // Module
        public static ModuleData GetModuleData(ClassDeclarationSyntax @class) {
            var name = @class.Identifier.ValueText.WithoutPrefix( "Module_" ).Replace( '_', '.' );
            var @namespaces = @class?.GetNamespacesGroupsTypes().GetNamespaceHierarchy().ToArray() ?? Array.Empty<Namespace>();
            return new ModuleData( name, namespaces );
        }


        // Helpers/GetModules
        private static IEnumerable<Module> GetModules(this ClassDeclarationSyntax @class) {
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
        private static IEnumerable<Namespace> GetNamespaceHierarchy(this IEnumerable<Node> namespaces) {
            return namespaces.Unflatten( i => i is Namespace ).Select( i => GetNamespace( (Namespace?) i.Key, i.Children ) );
        }
        private static Namespace GetNamespace(Namespace? @namespace, Node[] groups) {
            var namespace_ = @namespace ?? new Namespace( "Global", null! );
            var groups_ = groups.Map( GetGroupHierarchy ).ToArray();
            return new Namespace( namespace_.Name, groups_ );
        }
        private static IEnumerable<Group> GetGroupHierarchy(Node[] groups) {
            return groups.Unflatten( i => i is Group ).Select( i => GetGroup( (Group?) i.Key, i.Children ) );
        }
        private static Group GetGroup(Group? group, Node[] types) {
            var group_ = group ?? new Group( "Default", null! );
            var types_ = types.Cast<Type>().ToArray();
            return new Group( group_.Name, types_ );
        }

        // Helpers/Syntax
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
        // Helpers/Syntax
        private static Module GetModule(SyntaxNode syntax) {
            var module = ((TypeOfExpressionSyntax) syntax).Type.ToString();
            return new Module( module );
        }
        private static Namespace GetNamespace(SyntaxNode syntax) {
            var @namespace = ((LiteralExpressionSyntax) syntax).Token.ValueText;
            return new Namespace( @namespace, null! );
        }
        private static Group GetGroup(SyntaxNode syntax) {
            var comment = syntax.GetLeadingTrivia().Where( i => i.Kind() is SyntaxKind.SingleLineCommentTrivia or SyntaxKind.SingleLineDocumentationCommentTrivia ).LastOrDefault();
            var group = comment.ToString().GetCommentContent();
            return new Group( group, null! );
        }
        private static Type GetType(SyntaxNode syntax) {
            var type = ((TypeOfExpressionSyntax) syntax).Type.ToString();
            return new Type( type );
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
