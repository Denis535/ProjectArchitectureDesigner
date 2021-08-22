// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ProjectArchitectureDesigner.Model;

    public class MarkdownProjectRenderer : ProjectRenderer {
        public MarkdownProjectRenderer() : base( new MarkdownHighlighter( new TextRenderer() ) ) {
        }
        public MarkdownProjectRenderer(NodeRenderer renderer) : base( renderer ) {
        }
        // Render
        public override MarkdownProjectRenderer Render(ProjectArchNode project) {
            AppendHierarchy( project );
            return this;
        }
        // AppendText
        protected override void AppendText(ArchNode node, string text) {
            text = "- " + text.Replace( " ", "&nbsp" );
            base.AppendText( node, text );
        }
    }
}
