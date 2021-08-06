// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ProjectArchitectureDesigner.Model;

    public class HierarchicalTextProjectRenderer : ProjectRenderer {

        private HierarchicalStringBuilder Builder { get; } = new HierarchicalStringBuilder();

        public HierarchicalTextProjectRenderer(INodeRenderer renderer) : base( renderer ) {
        }


        // Render
        public override string Render(ProjectArchNode project, Func<TypeArchNode, bool> predicate) {
            RenderProject( project, predicate );
            return Builder.ToString();
        }
        // Render/Node
        protected override void RenderProject(ProjectArchNode project, Func<TypeArchNode, bool> predicate) {
            using var scope = Builder.AppendTitle( Render( project ) );
            base.RenderProject( project, predicate );
        }
        protected override void RenderModule(ModuleArchNode module, Func<TypeArchNode, bool> predicate) {
            using var scope = Builder.AppendSection( Render( module ) );
            base.RenderModule( module, predicate );
        }
        protected override void RenderNamespace(NamespaceArchNode @namespace, Func<TypeArchNode, bool> predicate) {
            using var scope = Builder.AppendSection( Render( @namespace ) );
            base.RenderNamespace( @namespace, predicate );
        }
        protected override void RenderGroup(GroupArchNode group, Func<TypeArchNode, bool> predicate) {
            if (!group.IsDefault) Builder.AppendLineWithPrefix( "| - ", Render( group ) );
            base.RenderGroup( group, predicate );
        }
        protected override void RenderType(TypeArchNode type) {
            Builder.AppendLineWithPrefix( "|   ", Render( type ) );
        }


    }
}
