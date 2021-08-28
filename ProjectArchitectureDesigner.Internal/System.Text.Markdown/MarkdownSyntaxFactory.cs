// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// https://guides.github.com/features/mastering-markdown/
namespace System.Text.Markdown {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public static class MarkdownSyntaxFactory {


        // Syntax
        public static string Header1(this string text) => Header( text, 1 );
        public static string Header2(this string text) => Header( text, 2 );
        public static string Header3(this string text) => Header( text, 3 );
        public static string Header4(this string text) => Header( text, 4 );
        public static string Header(this string text, int level) {
            return Repeat( '#', level ) + " " + text;
        }


        public static string Item1(this string text) => Item( text, 1 );
        public static string Item2(this string text) => Item( text, 2 );
        public static string Item3(this string text) => Item( text, 3 );
        public static string Item4(this string text) => Item( text, 4 );
        public static string Item(this string text, int level) {
            return Repeat( "    ", level - 1 ) + "- " + text;
        }


        public static string CodeBlock() => "```";
        public static string CodeBlockCSharp() => "```csharp";


        public static string Italic(this string text) => "*{0}*".Format( text );
        public static string Bold(this string text) => "**{0}**".Format( text );
        public static string Code(this string text) => "`{0}`".Format( text );
        public static string Reference(this string text, int id) => "[{0}][{1}]".Format( text, id );
        public static string Link(this string text, string uri, IList<string> uris) => "[{0}]({1})".Format( text, uri.ToUri().WithId( uris ) );
        public static string Link(this int id, string uri, IList<string> uris) => "[{0}]:{1}".Format( id, uri.ToUri().WithId( uris ) );


        // Helpers/Uri
        private static string ToUri(this string text) {
            var chars = text.ToLowerInvariant().Trim().Replace( "  ", " " ).Replace( " ", "-" ).Where( IsValid );
            return string.Concat( chars );
        }
        private static string WithId(this string uri, IList<string> uris) {
            var id = uris.Count( i => i == uri );
            uris.Add( uri );
            return (id != 0) ? (uri + "-" + id) : uri;
        }
        private static bool IsValid(char @char) {
            return char.IsLetterOrDigit( @char ) || @char is '-';
        }
        // Helpers/String
        private static string Repeat(char value, int count) {
            return new string( value, count );
        }
        private static string Repeat(string value, int count) {
            return new StringBuilder( value.Length * count ).Insert( 0, value, count ).ToString();
        }


    }
}