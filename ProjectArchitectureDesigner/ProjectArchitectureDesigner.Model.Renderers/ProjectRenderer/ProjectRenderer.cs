// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ProjectArchitectureDesigner.Model;

    public abstract class ProjectRenderer {

        protected StringBuilder Builder { get; } = new StringBuilder();
        protected NodeRenderer Renderer { get; }


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
            var text = Renderer.Render( project );
            AppendTableOfContentsRow( project, text );
            foreach (var module in project.Modules) AppendTableOfContents( module );
        }
        private void AppendTableOfContents(ModuleArchNode module) {
            var text = Renderer.Render( module );
            AppendTableOfContentsRow( module, text );
            foreach (var @namespace in module.Namespaces) AppendTableOfContents( @namespace );
        }
        private void AppendTableOfContents(NamespaceArchNode @namespace) {
            var text = Renderer.Render( @namespace );
            AppendTableOfContentsRow( @namespace, text );
        }
        // Append/TableOfContents/Row
        protected abstract void AppendTableOfContentsRow(ArchNode node, string text);


        // Append/Content
        protected virtual void AppendContent(ProjectArchNode project) {
            var text = Renderer.Render( project );
            AppendContentRow( project, text );
            foreach (var module in project.Modules) AppendContent( module );
        }
        private void AppendContent(ModuleArchNode module) {
            var text = Renderer.Render( module );
            AppendContentRow( module, text );
            foreach (var @namespace in module.Namespaces) AppendContent( @namespace );
        }
        private void AppendContent(NamespaceArchNode @namespace) {
            var text = Renderer.Render( @namespace );
            AppendContentRow( @namespace, text );
            foreach (var group in @namespace.Groups) AppendContent( group );
        }
        private void AppendContent(GroupArchNode group) {
            var text = Renderer.Render( group );
            AppendContentRow( group, text );
            foreach (var type in group.Types) AppendContent( type );
        }
        private void AppendContent(TypeArchNode type) {
            var text = Renderer.Render( type );
            AppendContentRow( type, text );
        }
        // Append/Content/Row
        protected abstract void AppendContentRow(ArchNode node, string text);


        // Utils
        public override string ToString() {
            return Builder.ToString();
        }


    }
}
