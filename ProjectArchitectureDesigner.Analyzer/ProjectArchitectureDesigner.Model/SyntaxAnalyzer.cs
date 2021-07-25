// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal static class SyntaxAnalyzer {


        // Project
        public static bool IsProject(this ClassDeclarationSyntax @class) {
            return @class.IsChildOf( "ProjectArchNode" );
        }
        public static ProjectInfo GetProjectInfo(this ClassDeclarationSyntax @class) {
            var type = @class.Identifier.ValueText;
            var modules = @class
                .GetMethods( "Initialize" )
                .SingleOrDefault()?
                .GetBody()?
                .DescendantNodes()
                .GetModuleEntries()
                .ToArray();
            return new ProjectInfo( type, modules ?? Array.Empty<ModuleEntry>() );
        }


        // Module
        public static bool IsModule(this ClassDeclarationSyntax @class) {
            return @class.IsChildOf( "ModuleArchNode" );
        }
        public static ModuleInfo GetModuleInfo(this ClassDeclarationSyntax @class) {
            var type = @class.Identifier.ValueText;
            var @namespaces = @class
                .GetMethods( "Initialize" )
                .SingleOrDefault()?
                .GetBody()?
                .DescendantNodes()
                .GetNamespaceEntries()
                .ToArray();
            return new ModuleInfo( type, namespaces ?? Array.Empty<NamespaceEntry>() );
        }


        // Helpers/GetEntries
        private static IEnumerable<ModuleEntry> GetModuleEntries(this IEnumerable<SyntaxNode> nodes) {
            foreach (var module in nodes.Where( IsModuleEntry )) {
                yield return new ModuleEntry( module.GetModuleEntry() );
            }
        }
        private static IEnumerable<NamespaceEntry> GetNamespaceEntries(this IEnumerable<SyntaxNode> nodes) {
            foreach (var (@namespace, types) in nodes.Where( IsNamespaceOrTypeEntry ).Unflatten( IsNamespaceEntry )) {
                var namespace_ = @namespace?.GetNamespaceEntry() ?? "Global";
                var groups = types.GetGroupEntries().ToArray();
                yield return new NamespaceEntry( namespace_, groups );
            }
        }
        private static IEnumerable<GroupEntry> GetGroupEntries(this IEnumerable<SyntaxNode> nodes) {
            foreach (var group in nodes.SplitByFirst( HasGroupEntry )) {
                var group_ = group.First().GetGroupEntry() ?? "Default";
                var types = group.GetTypeEntries().ToArray();
                yield return new GroupEntry( group_, types );
            }
        }
        private static IEnumerable<TypeEntry> GetTypeEntries(this IEnumerable<SyntaxNode> nodes) {
            foreach (var type in nodes) {
                yield return new TypeEntry( type.GetTypeEntry() );
            }
        }
        // Helpers/IsEntry
        private static bool IsModuleEntry(this SyntaxNode syntax) {
            return syntax is TypeOfExpressionSyntax;
        }
        private static bool IsNamespaceOrTypeEntry(this SyntaxNode syntax) {
            return syntax.IsNamespaceEntry() || syntax.IsTypeEntry();
        }
        private static bool IsNamespaceEntry(this SyntaxNode syntax) {
            return syntax is LiteralExpressionSyntax literal && literal.Kind() == SyntaxKind.StringLiteralExpression;
        }
        private static bool HasGroupEntry(this SyntaxNode syntax) {
            var comment = syntax.GetLeadingTrivia().LastOrDefault( IsGroupEntry );
            return comment != default;
        }
        private static bool IsGroupEntry(this SyntaxTrivia trivia) {
            // Note: You should use SyntaxTrivia.ToFullString() to support SingleLineDocumentationCommentTrivia.
            if (trivia.Kind() is SyntaxKind.SingleLineCommentTrivia or SyntaxKind.SingleLineDocumentationCommentTrivia) {
                var @string = trivia.ToFullString();
                return @string.StartsWith( "// Group:" ) || @string.StartsWith( "/// Group:" );
            }
            return false;
        }
        private static bool IsTypeEntry(this SyntaxNode syntax) {
            return syntax is TypeOfExpressionSyntax;
        }
        // Helpers/GetEntry
        private static string GetModuleEntry(this SyntaxNode syntax) {
            return ((TypeOfExpressionSyntax) syntax).Type.ToString();
        }
        private static string? GetNamespaceEntry(this SyntaxNode syntax) {
            var @namespace = ((LiteralExpressionSyntax) syntax).Token.ValueText.Trim();
            if (@namespace.IsNotEmpty()) return @namespace;
            return null;
        }
        private static string? GetGroupEntry(this SyntaxNode syntax) {
            var comment = syntax.GetLeadingTrivia().LastOrDefault( IsGroupEntry );
            if (comment != default) {
                var group = comment.ToFullString().GetStringAfter( "Group:" )?.Trim();
                if (group.IsNotEmpty()) return group;
            }
            return null;
        }
        private static string GetTypeEntry(this SyntaxNode syntax) {
            return ((TypeOfExpressionSyntax) syntax).Type.ToString();
        }


    }
}
