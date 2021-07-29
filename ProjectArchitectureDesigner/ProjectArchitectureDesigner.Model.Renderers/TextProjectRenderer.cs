// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ProjectArchitectureDesigner.Model;

    public class TextProjectRenderer : ProjectRenderer {

        private StringBuilder Builder { get; } = new StringBuilder();

        public TextProjectRenderer(INodeRenderer renderer) : base( renderer ) {
        }


        // Render
        public override string Render(ProjectArchNode project, Func<TypeArchNode, bool> predicate) {
            RenderProject( project, predicate );
            return Builder.ToString();
        }
        // Render/Node
        protected override void RenderProject(ProjectArchNode project, string text, Func<TypeArchNode, bool> predicate) {
            Builder.AppendLine( text );
            base.RenderProject( project, text, predicate );
        }
        protected override void RenderModule(ModuleArchNode module, string text, Func<TypeArchNode, bool> predicate) {
            Builder.AppendLine( text );
            base.RenderModule( module, text, predicate );
        }
        protected override void RenderNamespace(NamespaceArchNode @namespace, string text, Func<TypeArchNode, bool> predicate) {
            Builder.AppendLine( text );
            base.RenderNamespace( @namespace, text, predicate );
        }
        protected override void RenderGroup(GroupArchNode group, string text, Func<TypeArchNode, bool> predicate) {
            if (!group.IsDefault) Builder.AppendLine( text );
            base.RenderGroup( group, text, predicate );
        }
        protected override void RenderType(TypeArchNode type, string text) {
            Builder.AppendLine( text );
            base.RenderType( type, text );
        }


    }
}
