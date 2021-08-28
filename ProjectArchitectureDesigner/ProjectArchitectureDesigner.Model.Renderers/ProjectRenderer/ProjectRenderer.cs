// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ProjectArchitectureDesigner.Model;

    public abstract class ProjectRenderer {

        protected StringBuilder Builder { get; } = new StringBuilder();
        private NodeRenderer Renderer { get; }


        public ProjectRenderer(NodeRenderer renderer) {
            Renderer = renderer;
        }


        // Render
        public string Render(ProjectArchNode project) {
            RenderInternal( project, true );
            return Builder.ToString();
        }
        public string Render(ProjectArchNode project, bool withTableOfContents) {
            RenderInternal( project, withTableOfContents );
            return Builder.ToString();
        }
        protected virtual void RenderInternal(ProjectArchNode project, bool withTableOfContents) {
            if (withTableOfContents) AppendTableOfContents( project );
            AppendContent( project );
        }


        // Append/TableOfContents
        protected virtual void AppendTableOfContents(ProjectArchNode project) {
            AppendTableOfContentsRow( project );
            foreach (var module in project.Modules) AppendTableOfContents( module );
        }
        private void AppendTableOfContents(ModuleArchNode module) {
            AppendTableOfContentsRow( module );
            foreach (var @namespace in module.Namespaces) AppendTableOfContents( @namespace );
        }
        private void AppendTableOfContents(NamespaceArchNode @namespace) {
            AppendTableOfContentsRow( @namespace );
        }
        // Append/TableOfContents/Row
        protected virtual void AppendTableOfContentsRow(ArchNode node) {
            Builder.AppendLine( GetStringWithHighlight( node ) );
        }


        // Append/Content
        protected virtual void AppendContent(ProjectArchNode project) {
            AppendContentRow( project );
            foreach (var module in project.Modules) AppendContent( module );
        }
        private void AppendContent(ModuleArchNode module) {
            AppendContentRow( module );
            foreach (var @namespace in module.Namespaces) AppendContent( @namespace );
        }
        private void AppendContent(NamespaceArchNode @namespace) {
            AppendContentRow( @namespace );
            foreach (var group in @namespace.Groups) AppendContent( group );
        }
        private void AppendContent(GroupArchNode group) {
            AppendContentRow( group );
            foreach (var type in group.Types) AppendContent( type );
        }
        private void AppendContent(TypeArchNode type) {
            AppendContentRow( type );
        }
        // Append/Content/Row
        protected virtual void AppendContentRow(ArchNode node) {
            Builder.AppendLine( GetStringWithHighlight( node ) );
        }


        // GetString
        protected string GetString(ArchNode node) {
            return Render( Renderer, node, node.GetName() );
        }
        protected string GetStringWithHighlight(ArchNode node) {
            return RenderAndHighlight( Renderer, node, node.GetName() );
        }


        // Utils
        public override string ToString() {
            return Builder.ToString();
        }


        // Helpers/Render
        private static string Render(NodeRenderer renderer, ArchNode node, string text) {
            foreach (var renderer_ in Flatten( renderer ).Reverse()) {
                text = renderer_.Render( node, text );
            }
            return text;
        }
        private static string RenderAndHighlight(NodeRenderer renderer, ArchNode node, string text) {
            foreach (var renderer_ in Flatten( renderer ).Reverse()) {
                text = renderer_.Render( node, text );
                text = renderer_.Highlight( node, text );
            }
            return text;
        }
        private static IEnumerable<NodeRenderer> Flatten(NodeRenderer? renderer) {
            while (renderer != null) {
                yield return renderer;
                renderer = renderer.Source;
            }
        }


    }
}
