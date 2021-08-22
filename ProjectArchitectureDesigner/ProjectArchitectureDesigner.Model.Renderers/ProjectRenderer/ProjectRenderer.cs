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
        public abstract ProjectRenderer Render(ProjectArchNode project);


        // AppendHierarchy
        protected virtual void AppendHierarchy(ProjectArchNode project) {
            AppendText( project );
            foreach (var module in project.Modules) {
                AppendHierarchy( module );
            }
        }
        protected virtual void AppendHierarchy(ModuleArchNode module) {
            AppendText( module );
            foreach (var @namespace in module.Namespaces) {
                AppendHierarchy( @namespace );
            }
        }
        protected virtual void AppendHierarchy(NamespaceArchNode @namespace) {
            AppendText( @namespace );
            foreach (var group in @namespace.Groups) {
                AppendHierarchy( group );
            }
        }
        protected virtual void AppendHierarchy(GroupArchNode group) {
            AppendText( group );
            foreach (var type in group.Types) {
                AppendText( type );
            }
        }


        // AppendText
        protected virtual void AppendText(ArchNode node) {
            if (node is GroupArchNode group && group.IsDefault()) return;
            AppendText( node, GetStringWithHighlight( node ) );
        }
        protected virtual void AppendText(ArchNode node, string text) {
            if (node is ProjectArchNode project) AppendText( project, text );
            if (node is ModuleArchNode module) AppendText( module, text );
            if (node is NamespaceArchNode @namespace) AppendText( @namespace, text );
            if (node is GroupArchNode group) AppendText( group, text );
            if (node is TypeArchNode type) AppendText( type, text );
        }
        protected virtual void AppendText(ProjectArchNode project, string text) {
            Builder.AppendLine( text );
        }
        protected virtual void AppendText(ModuleArchNode module, string text) {
            Builder.AppendLine( text );
        }
        protected virtual void AppendText(NamespaceArchNode @namespace, string text) {
            Builder.AppendLine( text );
        }
        protected virtual void AppendText(GroupArchNode group, string text) {
            Builder.AppendLine( text );
        }
        protected virtual void AppendText(TypeArchNode type, string text) {
            Builder.AppendLine( text );
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
