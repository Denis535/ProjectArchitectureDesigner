// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ProjectArchitectureDesigner.Model;

    public class TextProjectRenderer : ProjectRenderer {
        public TextProjectRenderer() : base( new TextRenderer() ) {
        }
        public TextProjectRenderer(NodeRenderer renderer) : base( renderer ) {
        }
        // Render
        public override TextProjectRenderer Render(ProjectArchNode project) {
            Builder.AppendLine( "```" );
            AppendHierarchy( project );
            Builder.AppendLine( "```" );
            return this;
        }
    }
    public class ColorTextProjectRenderer : ProjectRenderer {
        public ColorTextProjectRenderer() : base( new ColorHighlighter( new TextRenderer() ) ) {
        }
        public ColorTextProjectRenderer(NodeRenderer renderer) : base( renderer ) {
        }
        // Render
        public override ColorTextProjectRenderer Render(ProjectArchNode project) {
            Builder.AppendLine( "```diff" );
            AppendHierarchy( project );
            Builder.AppendLine( "```" );
            return this;
        }
    }
}
