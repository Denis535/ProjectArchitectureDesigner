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
        protected override void RenderInternal(ProjectArchNode project, bool withTableOfContents) {
            Builder.AppendLine( "```" );
            if (withTableOfContents) AppendTableOfContents( project );
            AppendContent( project );
            Builder.AppendLine( "```" );
        }
        // Append/TableOfContents
        protected override void AppendTableOfContents(ProjectArchNode project) {
            Builder.AppendLine( "Table of Contents" );
            base.AppendTableOfContents( project );
            Builder.AppendLine();
        }
        protected override void AppendTableOfContentsRow(ArchNode node, string text) {
            Builder.AppendLine( text );
        }
        // Append/Content
        protected override void AppendContent(ProjectArchNode project) {
            base.AppendContent( project );
        }
        protected override void AppendContentRow(ArchNode node, string text) {
            Builder.AppendLine( text );
        }
    }
    public class ColorTextProjectRenderer : ProjectRenderer {
        public ColorTextProjectRenderer() : base( new TextRenderer() ) {
        }
        public ColorTextProjectRenderer(NodeRenderer renderer) : base( renderer ) {
        }
        // Render
        protected override void RenderInternal(ProjectArchNode project, bool withTableOfContents) {
            Builder.AppendLine( "```diff" );
            if (withTableOfContents) AppendTableOfContents( project );
            AppendContent( project );
            Builder.AppendLine( "```" );
        }
        // Append/TableOfContents
        protected override void AppendTableOfContents(ProjectArchNode project) {
            Builder.AppendLine( "Table of Contents" );
            base.AppendTableOfContents( project );
            Builder.AppendLine();
        }
        protected override void AppendTableOfContentsRow(ArchNode node, string text) {
            if (node is ProjectArchNode) {
                Builder.Append( "- " ).AppendLine( text );
            }
            if (node is ModuleArchNode) {
                Builder.Append( "- " ).AppendLine( text );
            }
            if (node is NamespaceArchNode) {
                Builder.Append( "! " ).AppendLine( text );
            }
        }
        // Append/Content
        protected override void AppendContent(ProjectArchNode project) {
            base.AppendContent( project );
        }
        protected override void AppendContentRow(ArchNode node, string text) {
            if (node is ProjectArchNode) {
                Builder.Append( "- " ).AppendLine( text );
            }
            if (node is ModuleArchNode) {
                Builder.Append( "- " ).AppendLine( text );
            }
            if (node is NamespaceArchNode) {
                Builder.Append( "! " ).AppendLine( text );
            }
            if (node is GroupArchNode) {
                Builder.Append( "+ " ).AppendLine( text );
            }
            if (node is TypeArchNode) {
                Builder.Append( "# " ).AppendLine( text );
            }
        }
    }
}
