// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ColorBlockProjectRenderer : TextProjectRenderer {


        public ColorBlockProjectRenderer() : base( new ColorHighlighter( new TextRenderer() ) ) {
        }
        public ColorBlockProjectRenderer(NodeRenderer renderer) : base( renderer ) {
        }


        // Render
        public override ColorBlockProjectRenderer Render(ProjectArchNode project) {
            Builder.AppendLine( "```diff" );
            AppendHierarchy( project );
            Builder.AppendLine( "```" );
            return this;
        }


    }
}
