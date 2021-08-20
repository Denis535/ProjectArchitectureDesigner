// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CodeBlockProjectRenderer : TextProjectRenderer {


        public CodeBlockProjectRenderer() : base( new TextRenderer() ) {
        }
        public CodeBlockProjectRenderer(NodeRenderer renderer) : base( renderer ) {
        }


        // Render
        public override CodeBlockProjectRenderer Render(ProjectArchNode project) {
            Builder.AppendLine( "```" );
            AppendHierarchy( project );
            Builder.AppendLine( "```" );
            return this;
        }


    }
}
