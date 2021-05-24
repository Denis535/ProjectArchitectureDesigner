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
        public record Module(string Value) {
            public override string ToString() => string.Format( "Module: {0}", Value );
            public static implicit operator string(Module @object) => @object.Value;
        }
        public record Namespace(string Value, Group[] Groups) {
            public override string ToString() => string.Format( "Namespace: {0}", Value );
            public static implicit operator string(Namespace @object) => @object.Value;
        }
        public record Group(string Value, Type[] Types) {
            public override string ToString() => string.Format( "Group: {0}", Value );
            public static implicit operator string(Group @object) => @object.Value;
        }
        public record Type(string Value) {
            public override string ToString() => string.Format( "Type: {0}", Value );
            public static implicit operator string(Type @object) => @object.Value;
        }


        // Project
        public static Module[] GetProjectData(ClassDeclarationSyntax @class) {
            var constructor = @class.Members.OfType<ConstructorDeclarationSyntax>().FirstOrDefault();
            var body = (SyntaxNode?) constructor?.Body ?? constructor?.ExpressionBody;
            if (body != null) {
                return body.DescendantNodes().Where( IsModule ).Select( GetModule ).ToArray();
            }
            return Array.Empty<Module>();
        }

        // Module
        public static Namespace[] GetModuleData(ClassDeclarationSyntax @class) {
            var constructor = @class.Members.OfType<ConstructorDeclarationSyntax>().FirstOrDefault();
            var body = (SyntaxNode?) constructor?.Body ?? constructor?.ExpressionBody;
            if (body != null) {
                return GetNamespacesAndGroupsAndTypes( body ).Map( GetNamespaces ).ToArray();
            }
            return Array.Empty<Namespace>();
        }


        // Helpers/Module
        private static IEnumerable<object> GetNamespacesAndGroupsAndTypes(SyntaxNode syntax) {
            var nodes = syntax.DescendantNodes().Where( i => IsNamespace( i ) || IsType( i ) );
            foreach (var node in nodes) {
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
        private static IEnumerable<Namespace> GetNamespaces(IEnumerable<object> namespaces) {
            return namespaces.Unflatten( i => i is Namespace ).Select( i => GetNamespace( i.Key, i.Children ) );
        }
        private static Namespace GetNamespace(object? @namespace, object[] groups) {
            var namespace_ = (Namespace?) @namespace ?? new Namespace( "Global", null! );
            var groups_ = groups.Map( GetGroups ).ToArray();
            return new Namespace( namespace_, groups_ );
        }
        private static IEnumerable<Group> GetGroups(object[] groups) {
            return groups.Unflatten( i => i is Group ).Select( i => GetGroup( i.Key, i.Children ) );
        }
        private static Group GetGroup(object? group, object[] types) {
            var group_ = (Group?) group ?? new Group( "Default", null! );
            var types_ = types.Cast<Type>().ToArray();
            return new Group( group_, types_ );
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


        // Helpers/Syntax/Misc
        private static string GetCommentContent(string comment) {
            var content = comment.SkipWhile( i => i == '/' );
            return string.Concat( content ).Trim();
        }


    }
}
