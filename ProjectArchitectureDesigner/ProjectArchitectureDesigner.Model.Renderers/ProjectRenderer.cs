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
        public abstract string Render(ProjectArchNode project);


        // AppendHierarchy
        protected virtual void AppendHierarchy(ProjectArchNode project) {
            AppendLine( project, GetString( project ) );
            foreach (var module in project.Modules) {
                AppendHierarchy( module );
            }
        }
        protected virtual void AppendHierarchy(ModuleArchNode module) {
            AppendLine( module, GetString( module ) );
            foreach (var @namespace in module.Namespaces) {
                AppendHierarchy( @namespace );
            }
        }
        protected virtual void AppendHierarchy(NamespaceArchNode @namespace) {
            AppendLine( @namespace, GetString( @namespace ) );
            foreach (var group in @namespace.Groups) {
                AppendHierarchy( group );
            }
        }
        protected virtual void AppendHierarchy(GroupArchNode group) {
            AppendLine( group, GetString( group ) );
            foreach (var type in group.Types) {
                AppendLine( type, GetString( type ) );
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
            return GetString( Renderer, node, node.GetName() );
        }


        // Helpers/GetString
        private static string GetString(NodeRenderer renderer, ArchNode node, string text) {
            if (renderer.Source != null) text = GetString( renderer.Source, node, text );
            text = renderer.Render( node, text );
            text = renderer.Highlight( node, text );
            return text;
        }


    }
}
