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

    //internal struct Column {
    //    public readonly TextAlignment Alignment;
    //    public readonly int Length;
    //    public Column(TextAlignment alignment, int length) {
    //        Alignment = alignment;
    //        Length = length;
    //    }
    //}

    internal static class MarkdownSyntaxFactory {


        // Blocks/Header
        public static string Header1(string value) => Header( 1, value );
        public static string Header2(string value) => Header( 2, value );
        public static string Header3(string value) => Header( 3, value );
        public static string Header(int level, string value) {
            return Repeat( '#', level ) + " " + value;
        }

        // Blocks/Item
        public static string Item1(string value) => Item( 1, value );
        public static string Item2(string value) => Item( 2, value );
        public static string Item3(string value) => Item( 3, value );
        public static string Item(int level, string value) {
            return Indent( level ) + "- " + value;
        }

        // Spans
        public static string Italic(string value) => "*{0}*".Format( value );
        public static string Bold(string value) => "**{0}**".Format( value );
        public static string Code(string value) => "`{0}`".Format( value );
        public static string Reference(string link, int id) => "[{0}] [{1}]".Format( link, id );
        public static string Link(string link, string url) => "[{0}] ({1})".Format( link, url );
        public static string Link(int id, string url) => "[{0}]: {1}".Format( id, url );


        //public static string TableAlignment(params Column[] values) => TableAlignment( values.AsEnumerable() );
        //public static string TableRow(params string[] values) => TableRow( values.AsEnumerable() );

        //public static string TableAlignment(IEnumerable<Column> values) => "|" + string.Join( "|", values.Select( TableAlignment ) ) + "|";
        //public static string TableRow(IEnumerable<string> values) => "| " + string.Join( " | ", values ) + " |";

        //private static string TableAlignment(Column value) => TableAlignment( value.Alignment, value.Length );
        //private static string TableAlignment(TextAlignment alignment, int length) {
        //    // |:- |:- |
        //    var line = new string( '-', length );
        //    if (alignment == TextAlignment.Left) return $":{line} ";
        //    if (alignment == TextAlignment.Right) return $" {line}:";
        //    if (alignment == TextAlignment.Center) return $":{line}:";
        //    return $" {line} ";
        //}


        // Helpers
        private static string Indent(int level) {
            return Repeat( "  ", level - 1 );
        }
        private static string Repeat(string value, int count) {
            return new StringBuilder( value.Length * count ).Insert( 0, value, count ).ToString();
        }
        private static string Repeat(char value, int count) {
            return new string( value, count );
        }


    }
}