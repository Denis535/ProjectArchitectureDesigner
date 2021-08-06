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
        // Render/Node
        protected virtual void RenderProject(ProjectArchNode project, Func<TypeArchNode, bool> predicate) {
            foreach (var module in GetModules( project, predicate )) {
                RenderModule( module, predicate );
            }
        }
        protected virtual void RenderModule(ModuleArchNode module, Func<TypeArchNode, bool> predicate) {
            foreach (var @namespace in GetNamespaces( module, predicate )) {
                RenderNamespace( @namespace, predicate );
            }
        }
        protected virtual void RenderNamespace(NamespaceArchNode @namespace, Func<TypeArchNode, bool> predicate) {
            foreach (var group in GetGroup( @namespace, predicate )) {
                RenderGroup( group, predicate );
            }
        }
        protected virtual void RenderGroup(GroupArchNode group, Func<TypeArchNode, bool> predicate) {
            foreach (var type in GetTypes( group, predicate )) {
                RenderType( type );
            }
        }
        protected virtual void RenderType(TypeArchNode type) {
        }
        // Render/Node
        protected virtual string Render(ArchNode node) {
            return Renderer.Highlight( node, Renderer.Render( node ) );
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
