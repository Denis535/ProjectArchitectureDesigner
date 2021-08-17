// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ProjectArchitectureDesigner.Model;

    public class TextProjectRenderer : ProjectRenderer {

        private StringBuilder Builder { get; } = new StringBuilder();


        public TextProjectRenderer() : base( new TextNodeRenderer() ) {
        }
        public TextProjectRenderer(NodeRenderer renderer) : base( renderer ) {
        }


        // Render
        public override TextProjectRenderer Render(ProjectArchNode project) {
            Builder.Clear();
            AppendHierarchy( project );
            return this;
        }
        public TextProjectRenderer WithNonBreakingSpace() {
            Builder.Replace( " ", "&nbsp" );
            return this;
        }
        public TextProjectRenderer WithCodeBlock() {
            Builder.Insert( 0, "```" + Environment.NewLine );
            Builder.AppendLine( "```" );
            return this;
        }
        public TextProjectRenderer WithDiffCodeBlock() {
            Builder.Insert( 0, "```diff" + Environment.NewLine );
            Builder.AppendLine( "```" );
            return this;
        }


        // AppendLine
        protected override void AppendLine(ProjectArchNode project, string text) {
            Builder.AppendLine( text );
        }
        protected override void AppendLine(ModuleArchNode module, string text) {
            Builder.AppendLine( text );
        }
        protected override void AppendLine(NamespaceArchNode @namespace, string text) {
            Builder.AppendLine( text );
        }
        protected override void AppendLine(GroupArchNode group, string text) {
            if (group.IsDefault()) return;
            Builder.AppendLine( text );
        }
        protected override void AppendLine(TypeArchNode type, string text) {
            Builder.AppendLine( text );
        }


        // Utils
        public override string ToString() {
            return Builder.ToString();
        }


    }
}
