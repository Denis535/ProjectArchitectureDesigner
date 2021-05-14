// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace Microsoft.CodeAnalysis.CSharp.Syntax {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal static class SyntaxUtils {


        // Syntax
        public static bool IsPartial(this TypeDeclarationSyntax @class) {
            return @class.Modifiers.Select( i => i.Kind() ).Contains( SyntaxKind.PartialKeyword );
        }
        public static bool IsChildOf(this TypeDeclarationSyntax @class, string name) {
            var @base = @class.BaseList?.Types.FirstOrDefault();
            return @base?.ToString() == name;
        }
        public static IEnumerable<AttributeSyntax> GetAttributes(this TypeDeclarationSyntax @class) {
            return @class.AttributeLists.SelectMany( i => i.Attributes );
        }


        // Syntax/Checks
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


    }
}
