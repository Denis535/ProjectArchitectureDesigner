// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ProjectArchitectureDesigner.Model;

    public class TextProjectRenderer : ProjectRenderer {

        protected StringBuilder Builder { get; } = new StringBuilder();


        public TextProjectRenderer() : base( new TextRenderer() ) {
        }
        public TextProjectRenderer(NodeRenderer renderer) : base( renderer ) {
        }


        // Render
        public override TextProjectRenderer Render(ProjectArchNode project) {
            AppendHierarchy( project );
            return this;
        }
        public TextProjectRenderer WithNonBreakingSpace() {
            Builder.Replace( " ", "&nbsp" );
            return this;
        }


        // AppendNode
        protected override void AppendNode(ArchNode node, string text) {
            if (node is GroupArchNode group && group.IsDefault()) return;
            Builder.AppendLine( text );
        }


        // Utils
        public override string ToString() {
            return Builder.ToString();
        }


    }
}
