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
        private static IEnumerable<ProjectInfo.Module_> GetModules(this ClassDeclarationSyntax @class) {
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
        private static IEnumerable<ModuleInfo.Namespace_> GetNamespaceHierarchy(this IEnumerable<object> namespaces) {
            return namespaces.Unflatten( i => i is ModuleInfo.Namespace_ ).Select( i => GetNamespace( (ModuleInfo.Namespace_?) i.Key, i.Children ) );
        }
        private static ModuleInfo.Namespace_ GetNamespace(ModuleInfo.Namespace_? @namespace, object[] groups) {
            var namespace_ = @namespace ?? new ModuleInfo.Namespace_( "Global", null! );
            var groups_ = groups.GetGroupHierarchy().ToArray();
            return new ModuleInfo.Namespace_( namespace_.Name, groups_ );
        }
        private static IEnumerable<ModuleInfo.Group_> GetGroupHierarchy(this object[] groups) {
            return groups.Unflatten( i => i is ModuleInfo.Group_ ).Select( i => GetGroup( (ModuleInfo.Group_?) i.Key, i.Children ) );
        }
        private static ModuleInfo.Group_ GetGroup(ModuleInfo.Group_? group, object[] types) {
            var group_ = group ?? new ModuleInfo.Group_( "Default", null! );
            var types_ = types.Cast<ModuleInfo.Type_>().ToArray();
            return new ModuleInfo.Group_( group_.Name, types_ );
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
        private static ProjectInfo.Module_ GetModule(SyntaxNode syntax) {
            var module = ((TypeOfExpressionSyntax) syntax).Type.ToString();
            return new ProjectInfo.Module_( module );
        }
        private static ModuleInfo.Namespace_ GetNamespace(SyntaxNode syntax) {
            var @namespace = ((LiteralExpressionSyntax) syntax).Token.ValueText;
            return new ModuleInfo.Namespace_( @namespace, null! );
        }
        private static ModuleInfo.Group_ GetGroup(SyntaxNode syntax) {
            var comment = syntax.GetLeadingTrivia().Where( i => i.Kind() is SyntaxKind.SingleLineCommentTrivia or SyntaxKind.SingleLineDocumentationCommentTrivia ).LastOrDefault();
            var group = comment.ToString().GetCommentContent();
            return new ModuleInfo.Group_( group, null! );
        }
        private static ModuleInfo.Type_ GetType(SyntaxNode syntax) {
            var type = ((TypeOfExpressionSyntax) syntax).Type.ToString();
            return new ModuleInfo.Type_( type );
        }
        // Helpers/String
        private static string GetCommentContent(this string comment) {
            var content = comment.SkipWhile( i => i == '/' );
            return string.Concat( content ).Trim();
        }


    }
}
