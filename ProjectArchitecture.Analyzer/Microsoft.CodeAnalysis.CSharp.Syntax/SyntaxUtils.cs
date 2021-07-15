// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace Microsoft.CodeAnalysis.CSharp.Syntax {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal static class SyntaxUtils {


        // ToName
        public static string ToName(this string type, string? prefix) {
            return type.WithoutPrefix( prefix ).Replace( '_', '.' );
        }
        // ToType
        public static string ToType(this string name, string prefix) {
            return prefix + name.EscapeTypeName();
        }
        // ToIdentifier
        public static string ToIdentifier(this string name) {
            return name.EscapeIdentifier();
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
        public static string GetCommentContent(this SyntaxTrivia comment) {
            // Note: SyntaxTrivia.ToString() doesn't return documentation comment.
            // Note: So, you should use SyntaxTrivia.ToFullString().
            var content = comment.ToFullString().SkipWhile( i => i == '/' );
            return string.Concat( content ).Trim();
        }


        // Helpers/String
        private static string WithoutPrefix(this string value, string? prefix) {
            if (prefix != null && value.StartsWith( prefix )) return value.Substring( prefix.Length );
            return value;
        }
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
