// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace Microsoft.CodeAnalysis.CSharp.Syntax {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal static class SyntaxUtils {


        // ToBeautifulName
        public static string ToBeautifulName(this string type) {
            return type.Replace( '_', '.' );
        }
        // EscapeType
        public static string EscapeType(this string value) {
            value = string.Concat( value.Select( Escape ) );
            return value;
        }
        // EscapeIdentifier
        public static string EscapeIdentifier(this string value) {
            value = string.Concat( value.Select( Escape ) );
            if (SyntaxFacts.GetKeywordKind( value ) != SyntaxKind.None) value = "@" + value;
            return value;
        }
        private static char Escape(char @char) {
            return char.IsLetterOrDigit( @char ) ? @char : '_';
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


        // String/Format2
        public static string Format2(this string value, params object?[] args) {
            var builder = new StringBuilder( value.Length * 2 );
            for (int i = 0, j = 0; i < value.Length;) {
                if (value.IsPlaceholder( ref i )) {
                    builder.AppendArgument( args, ref j );
                    continue;
                }
                builder.Append( value[ i ] );
                i++;
            }
            return builder.ToString();
        }
        private static bool IsPlaceholder(this string value, ref int i) {
            if (value[ i ] == '$') {
                i++;
                while (i < value.Length && char.IsLetterOrDigit( value[ i ] )) i++;
                return true;
            }
            return false;
        }
        private static void AppendArgument(this StringBuilder builder, object?[] args, ref int i) {
            var arg = args[ i++ ];
            if (arg is string @string) {
                builder.Append( @string );
                return;
            }
            if (arg is IEnumerable enumerable) {
                foreach (var item in enumerable) {
                    builder.Append( item );
                    builder.Append( ", " );
                }
                builder.Length -= 2;
                return;
            }
            builder.Append( arg );
        }
        // String/WithoutPrefix
        public static string WithoutPrefix(this string value, string prefix) {
            if (value.StartsWith( prefix )) return value.Substring( prefix.Length );
            return value;
        }
        // String/TakeAfter
        public static string? TakeAfter(this string value, string prefix) {
            var i = value.IndexOf( prefix );
            if (i != -1) return value.Substring( i + prefix.Length );
            return null;
        }


    }
}
