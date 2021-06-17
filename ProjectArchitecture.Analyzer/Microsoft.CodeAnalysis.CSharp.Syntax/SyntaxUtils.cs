// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace Microsoft.CodeAnalysis.CSharp.Syntax {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal static class SyntaxUtils {


        // IsEntry
        public static bool IsModuleEntry(this SyntaxNode syntax) {
            return syntax is TypeOfExpressionSyntax;
        }
        public static bool IsNamespaceEntry(this SyntaxNode syntax) {
            return syntax is LiteralExpressionSyntax literal && literal.Kind() == SyntaxKind.StringLiteralExpression;
        }
        public static bool HasGroupEntry(this SyntaxNode syntax) {
            // Note: SyntaxTrivia.ToString() doesn't return documentation comment.
            // Note: So, you should use SyntaxTrivia.ToFullString()!
            var comment = syntax.GetLeadingTrivia().Where( i => i.Kind() is SyntaxKind.SingleLineCommentTrivia or SyntaxKind.SingleLineDocumentationCommentTrivia ).LastOrDefault();
            return comment.ToFullString().StartsWith( "// " ) || comment.ToFullString().StartsWith( "/// " );
        }
        public static bool IsTypeEntry(this SyntaxNode syntax) {
            return syntax is TypeOfExpressionSyntax;
        }
        // GetEntry
        public static string GetModuleEntry(this SyntaxNode syntax) {
            return ((TypeOfExpressionSyntax) syntax).Type.ToString();
        }
        public static string GetNamespaceEntry(this SyntaxNode syntax) {
            return ((LiteralExpressionSyntax) syntax).Token.ValueText;
        }
        public static string GetGroupEntry(this SyntaxNode syntax) {
            var comment = syntax.GetLeadingTrivia().Where( i => i.Kind() is SyntaxKind.SingleLineCommentTrivia or SyntaxKind.SingleLineDocumentationCommentTrivia ).LastOrDefault();
            return comment.GetCommentContent();
        }
        public static string GetTypeEntry(this SyntaxNode syntax) {
            return ((TypeOfExpressionSyntax) syntax).Type.ToString();
        }


        // GetName
        public static string GetName_Project(this string type) {
            return type.WithoutPrefix( "Project_" ).Replace( '_', '.' );
        }
        public static string GetName_Module(this string type) {
            return type.WithoutPrefix( "Module_" ).Replace( '_', '.' );
        }
        // GetTypeName
        public static string GetTypeName_Namespace(this string name) {
            return "Namespace_" + name.EscapeTypeName();
        }
        public static string GetTypeName_Group(this string name) {
            return "Group_" + name.EscapeTypeName();
        }
        // GetIdentifier
        public static string GetIdentifier(this string name) {
            return name.EscapeIdentifier();
        }
        public static string GetIdentifier_Module(this string name) {
            return name.WithoutPrefix( "Module_" ).EscapeIdentifier();
        }


        // EnsureIdentifierIsValid
        public static string EnsureIdentifierIsValid(string identifier) {
            if (string.IsNullOrEmpty( identifier )) {
                throw new Exception( "Identifier must not be empty" );
            }
            if (!identifier.First().Map( IsFirstCharValid )) {
                throw new Exception( "Identifier must start only with: letter or underscore: " + identifier );
            }
            if (!identifier.All( IsCharValid )) {
                throw new Exception( "Identifier must contain only: letter, digit or underscore: " + identifier );
            }
            if (!SyntaxFacts.IsValidIdentifier( identifier )) {
                throw new Exception( "Identifier is invalid: " + identifier );
            }
            return identifier;
        }
        private static bool IsFirstCharValid(char @char) {
            return char.IsLetter( @char ) || @char == '_';
        }
        private static bool IsCharValid(char @char) {
            return char.IsLetterOrDigit( @char ) || @char == '_';
        }


        // Syntax
        public static bool IsPartial(this TypeDeclarationSyntax type) {
            return type.Modifiers.Select( i => i.Kind() ).Contains( SyntaxKind.PartialKeyword );
        }
        public static bool IsChildOf(this TypeDeclarationSyntax type, string name) {
            var @base = type.BaseList?.Types.FirstOrDefault();
            return @base?.ToString() == name;
        }
        public static IEnumerable<MethodDeclarationSyntax> GetMethods(this TypeDeclarationSyntax type, string name) {
            return type.Members.OfType<MethodDeclarationSyntax>().Where( i => i.Identifier.ValueText == name );
        }
        public static SyntaxNode? GetBody(this MethodDeclarationSyntax method) {
            return (SyntaxNode?) method?.Body ?? method?.ExpressionBody;
        }


        // Helpers/Trivia
        private static string GetCommentContent(this SyntaxTrivia comment) {
            // Note: SyntaxTrivia.ToString() doesn't return documentation comment.
            // Note: So, you should use SyntaxTrivia.ToFullString()!
            var content = comment.ToFullString().SkipWhile( i => i == '/' );
            return string.Concat( content ).Trim();
        }
        // Helpers/String
        private static string EscapeTypeName(this string value) {
            value = string.Concat( value.Select( Escape ) );
            return value;
        }
        private static string EscapeIdentifier(this string value) {
            value = string.Concat( value.Select( Escape ) );
            if (SyntaxFacts.GetKeywordKind( value ) != SyntaxKind.None) value = "@" + value;
            return value;
        }
        private static char Escape(char @char) {
            return char.IsLetterOrDigit( @char ) ? @char : '_';
        }


    }
}
