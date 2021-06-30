// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// https://guides.github.com/features/mastering-markdown/
// https://bitbucket.org/tutorials/markdowndemo/src/master/
// https://confluence.atlassian.com/bitbucket/readme-content-221449772.html

namespace ProjectArchitecture.Renderers {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    internal static class MarkdownSyntaxFactory {


        // Blocks/Header
        public static string Header1(this string value) => Header( value, 1 );
        public static string Header2(this string value) => Header( value, 2 );
        public static string Header3(this string value) => Header( value, 3 );
        public static string Header4(this string value) => Header( value, 4 );
        public static string Header(this string value, int level) {
            return Repeat( '#', level ) + " " + value;
        }

        // Blocks/Item
        public static string Item1(this string value) => Item( value, 1 );
        public static string Item2(this string value) => Item( value, 2 );
        public static string Item3(this string value) => Item( value, 3 );
        public static string Item4(this string value) => Item( value, 4 );
        public static string Item(this string value, int level) {
            return Indent( level ) + "- " + value;
        }

        // Spans
        public static string Italic(this string value) => "*{0}*".Format( value );
        public static string Bold(this string value) => "**{0}**".Format( value );
        public static string Code(this string value) => "`{0}`".Format( value );
        public static string Reference(this string link, int id) => "[{0}] [{1}]".Format( link, id );
        public static string Link(this string link, string url) => "[{0}] ({1})".Format( link, url );
        public static string Link(this int id, string url) => "[{0}]: {1}".Format( id, url );


        // Helpers
        private static string Indent(int level) {
            return Repeat( "    ", level - 1 );
        }
        private static string Repeat(string value, int count) {
            return new StringBuilder( value.Length * count ).Insert( 0, value, count ).ToString();
        }
        private static string Repeat(char value, int count) {
            return new string( value, count );
        }


    }
}