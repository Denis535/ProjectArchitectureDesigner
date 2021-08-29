// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.Markdown;
    using ProjectArchitectureDesigner.Model;

    public class MarkdownProjectRenderer : ProjectRenderer {


        public MarkdownProjectRenderer() : base( new MarkdownHighlighter( new TextRenderer() ) ) {
        }
        public MarkdownProjectRenderer(NodeRenderer renderer) : base( renderer ) {
        }


        // Append/TableOfContents
        protected override void AppendTableOfContents(ProjectArchNode project) {
            Builder.AppendLine( "Table of Contents".Bold() );
            base.AppendTableOfContents( project );
            Builder.AppendLine();
        }
        protected override void AppendTableOfContentsRow(ArchNode node, string text) {
            text = text.Replace( " ", "&nbsp" );
            if (node is ProjectArchNode) {
                Builder.AppendLine( text.Item1() );
            }
            if (node is ModuleArchNode) {
                Builder.AppendLine( text.Item2() );
            }
            if (node is NamespaceArchNode) {
                Builder.AppendLine( text.Item3() );
            }
        }
        // Append/Content
        protected override void AppendContent(ProjectArchNode project) {
            base.AppendContent( project );
        }
        protected override void AppendContentRow(ArchNode node, string text) {
            text = text.Replace( " ", "&nbsp" );
            if (node is ProjectArchNode) {
                Builder.AppendLine( text.Item1() );
            }
            if (node is ModuleArchNode) {
                Builder.AppendLine( text.Item2() );
            }
            if (node is NamespaceArchNode) {
                Builder.AppendLine( text.Item3() );
            }
            if (node is GroupArchNode) {
                Builder.AppendLine( text.Item4() );
            }
            if (node is TypeArchNode) {
                Builder.AppendLine( text.Item4() );
            }
        }


    }
}
