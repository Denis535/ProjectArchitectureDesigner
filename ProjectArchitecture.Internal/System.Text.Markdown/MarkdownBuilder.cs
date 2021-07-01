// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace System.Text.Markdown {
    using System;
    using System.Collections.Generic;

    // https://guides.github.com/features/mastering-markdown/
    public class MarkdownBuilder {

        private StringBuilder Builder { get; } = new StringBuilder();


        // Append/Block
        public MarkdownBuilder AppendTableOfContents(IEnumerable<(string Text, int Level)> headers) {
            var prevs = new List<string>();
            Builder.AppendLine( "Table of Contents".Header1() );
            foreach (var (header, level) in headers) {
                Builder.AppendLine( header.Link( header.GetUri( prevs ) ).Item( level ) );
            }
            return this;
        }
        public MarkdownBuilder AppendHeader(string text, int level) {
            Builder.AppendLine( MarkdownSyntaxFactory.Header( text, level ) );
            return this;
        }
        public MarkdownBuilder AppendItem(string text, int level) {
            Builder.AppendLine( MarkdownSyntaxFactory.Item( text, level ) );
            return this;
        }

        // Append/Span
        public MarkdownBuilder Append(string text) {
            Builder.Append( text );
            return this;
        }
        public MarkdownBuilder AppendItalic(string text) {
            Builder.Append( text.Italic() );
            return this;
        }
        public MarkdownBuilder AppendBold(string text) {
            Builder.Append( text.Bold() );
            return this;
        }
        public MarkdownBuilder AppendCode(string text) {
            Builder.Append( text.Code() );
            return this;
        }
        public MarkdownBuilder AppendReference(string text, int id) {
            Builder.Append( text.Reference( id ) );
            return this;
        }
        public MarkdownBuilder AppendLink(string text, string url) {
            Builder.Append( text.Link( url ) );
            return this;
        }
        public MarkdownBuilder AppendLink(int id, string url) {
            Builder.Append( id.Link( url ) );
            return this;
        }

        // Append/Line
        public MarkdownBuilder AppendLine() {
            Builder.AppendLine();
            return this;
        }
        public MarkdownBuilder AppendLine(string text) {
            Builder.AppendLine( text );
            return this;
        }


        // Utils
        public override string ToString() {
            return Builder.ToString();
        }


    }
}
