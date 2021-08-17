// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ProjectArchitectureDesigner.Model;

    public abstract class ProjectRenderer {

        private NodeRenderer Renderer { get; }


        public ProjectRenderer(NodeRenderer renderer) {
            Renderer = renderer;
        }


        // Render
        public abstract ProjectRenderer Render(ProjectArchNode project);


        // AppendHierarchy
        protected virtual void AppendHierarchy(ProjectArchNode project) {
            AppendLine( project, GetStringWithHighlight( project ) );
            foreach (var module in project.Modules) {
                AppendHierarchy( module );
            }
        }
        protected virtual void AppendHierarchy(ModuleArchNode module) {
            AppendLine( module, GetStringWithHighlight( module ) );
            foreach (var @namespace in module.Namespaces) {
                AppendHierarchy( @namespace );
            }
        }
        protected virtual void AppendHierarchy(NamespaceArchNode @namespace) {
            AppendLine( @namespace, GetStringWithHighlight( @namespace ) );
            foreach (var group in @namespace.Groups) {
                AppendHierarchy( group );
            }
        }
        protected virtual void AppendHierarchy(GroupArchNode group) {
            AppendLine( group, GetStringWithHighlight( group ) );
            foreach (var type in group.Types) {
                AppendLine( type, GetStringWithHighlight( type ) );
            }
        }


        // AppendLine
        protected virtual void AppendLine(ProjectArchNode project, string text) {
        }
        protected virtual void AppendLine(ModuleArchNode module, string text) {
        }
        protected virtual void AppendLine(NamespaceArchNode @namespace, string text) {
        }
        protected virtual void AppendLine(GroupArchNode group, string text) {
        }
        protected virtual void AppendLine(TypeArchNode type, string text) {
        }


        // GetString
        protected virtual string GetString(ArchNode node) {
            return Render( Renderer, node, node.GetName() );
        }
        protected virtual string GetStringWithHighlight(ArchNode node) {
            return RenderAndHighlight( Renderer, node, node.GetName() );
        }


        // Helpers/Render
        private static string Render(NodeRenderer renderer, ArchNode node, string text) {
            if (renderer.Source != null) text = Render( renderer.Source, node, text );
            text = renderer.Render( node, text );
            return text;
        }
        private static string RenderAndHighlight(NodeRenderer renderer, ArchNode node, string text) {
            if (renderer.Source != null) text = RenderAndHighlight( renderer.Source, node, text );
            text = renderer.Render( node, text );
            text = renderer.Highlight( node, text );
            return text;
        }


    }
}
