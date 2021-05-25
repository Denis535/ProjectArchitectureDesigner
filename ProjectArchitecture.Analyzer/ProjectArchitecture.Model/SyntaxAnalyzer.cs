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
        // Node
        public abstract record Node(string Value) {
            public override string ToString() => string.Format( "{0}: {1}", GetType().Name, Value );
        }
        // Node/Project
        public record ProjectData(string Value, Module[] Modules) : Node( Value );
        public record Module(string Value) : Node( Value );
        // Node/Module
        public record ModuleData(string Value, Namespace[] Namespaces) : Node( Value );
        public record Namespace(string Value, Group[] Groups) : Node( Value );
        public record Group(string Value, Type[] Types) : Node( Value );
        public record Type(string Value) : Node( Value );


        // Project
        public static ProjectData GetProjectData(ClassDeclarationSyntax @class) {
            var method = @class.Members.OfType<MethodDeclarationSyntax>().FirstOrDefault( i => i.Identifier.ValueText == "DefineChildren" );
            var body = (SyntaxNode?) method?.Body ?? method?.ExpressionBody;
            var modules = body?.GetProjectData().ToArray() ?? Array.Empty<Module>();
            return new ProjectData( @class.Identifier.ValueText, modules );
        }
        private static IEnumerable<Module> GetProjectData(this SyntaxNode syntax) {
            foreach (var node in syntax.DescendantNodes()) {
                if (IsModule( node )) {
                    yield return GetModule( node );
                }
            }
        }

        // Module
        public static ModuleData GetModuleData(ClassDeclarationSyntax @class) {
            var method = @class.Members.OfType<MethodDeclarationSyntax>().FirstOrDefault( i => i.Identifier.ValueText == "DefineChildren" );
            var body = (SyntaxNode?) method?.Body ?? method?.ExpressionBody;
            var @namespaces = body?.GetModuleData().GetNamespaceHierarchy().ToArray() ?? Array.Empty<Namespace>();
            return new ModuleData( @class.Identifier.ValueText, namespaces );
        }
        private static IEnumerable<Node> GetModuleData(this SyntaxNode syntax) {
            foreach (var node in syntax.DescendantNodes()) {
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
            return new Namespace( namespace_.Value, groups_ );
        }
        private static IEnumerable<Group> GetGroupHierarchy(Node[] groups) {
            return groups.Unflatten( i => i is Group ).Select( i => GetGroup( (Group?) i.Key, i.Children ) );
        }
        private static Group GetGroup(Group? group, Node[] types) {
            var group_ = group ?? new Group( "Default", null! );
            var types_ = types.Cast<Type>().ToArray();
            return new Group( group_.Value, types_ );
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
            var group = GetCommentContent( comment.ToString() );
            return new Group( group, null! );
        }
        private static Type GetType(SyntaxNode syntax) {
            var type = ((TypeOfExpressionSyntax) syntax).Type.ToString();
            return new Type( type );
        }

        // Helpers/Syntax
        private static string GetCommentContent(string comment) {
            var content = comment.SkipWhile( i => i == '/' );
            return string.Concat( content ).Trim();
        }


    }
}
