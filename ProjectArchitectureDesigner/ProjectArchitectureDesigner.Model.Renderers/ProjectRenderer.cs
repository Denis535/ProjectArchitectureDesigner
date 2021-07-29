// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ProjectArchitectureDesigner.Model;

    public abstract class ProjectRenderer {

        private INodeRenderer Renderer { get; }

        public ProjectRenderer(INodeRenderer renderer) {
            Renderer = renderer;
        }


        // Render
        public abstract string Render(ProjectArchNode project, Func<TypeArchNode, bool> predicate);
        protected void RenderProject(ProjectArchNode project, Func<TypeArchNode, bool> predicate) {
            RenderProject( project, Renderer.Render( project ), predicate );
        }
        // Render/Node
        protected virtual void RenderProject(ProjectArchNode project, string text, Func<TypeArchNode, bool> predicate) {
            foreach (var module in GetModules( project, predicate )) {
                RenderModule( module, Renderer.Render( module ), predicate );
            }
        }
        protected virtual void RenderModule(ModuleArchNode module, string text, Func<TypeArchNode, bool> predicate) {
            foreach (var @namespace in GetNamespaces( module, predicate )) {
                RenderNamespace( @namespace, Renderer.Render( @namespace ), predicate );
            }
        }
        protected virtual void RenderNamespace(NamespaceArchNode @namespace, string text, Func<TypeArchNode, bool> predicate) {
            foreach (var group in GetGroup( @namespace, predicate )) {
                RenderGroup( group, Renderer.Render( group ), predicate );
            }
        }
        protected virtual void RenderGroup(GroupArchNode group, string text, Func<TypeArchNode, bool> predicate) {
            foreach (var type in GetTypes( group, predicate )) {
                RenderType( type, Renderer.Render( type ) );
            }
        }
        protected virtual void RenderType(TypeArchNode type, string text) {
        }


        // Helpers/GetChildren
        private static IEnumerable<ModuleArchNode> GetModules(ProjectArchNode project, Func<TypeArchNode, bool> predicate) {
            return project.Modules.Where( i => i.Types.Any( predicate ) );
        }
        private static IEnumerable<NamespaceArchNode> GetNamespaces(ModuleArchNode module, Func<TypeArchNode, bool> predicate) {
            return module.Namespaces.Where( i => i.Types.Any( predicate ) );
        }
        private static IEnumerable<GroupArchNode> GetGroup(NamespaceArchNode @namespace, Func<TypeArchNode, bool> predicate) {
            return @namespace.Groups.Where( i => i.Types.Any( predicate ) );
        }
        private static IEnumerable<TypeArchNode> GetTypes(GroupArchNode group, Func<TypeArchNode, bool> predicate) {
            return group.Types.Where( predicate );
        }


    }
}
