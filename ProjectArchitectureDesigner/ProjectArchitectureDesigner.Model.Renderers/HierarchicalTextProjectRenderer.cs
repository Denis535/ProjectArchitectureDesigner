// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ProjectArchitectureDesigner.Model;

    public class HierarchicalTextProjectRenderer : ProjectRenderer {

        private string? indent_module, indent_namespace;
        private StringBuilder Builder { get; } = new StringBuilder();


        public HierarchicalTextProjectRenderer() : base( new TextNodeRenderer( null ) ) {
        }
        public HierarchicalTextProjectRenderer(NodeRenderer renderer) : base( renderer ) {
        }


        // Render
        public override string Render(ProjectArchNode project) {
            Builder.Clear();
            AppendHierarchy( project );
            return Builder.ToString();
        }


        // AppendLine
        protected override void AppendLine(ProjectArchNode project, string text) {
            Builder.AppendLine( text );
        }
        protected override void AppendLine(ModuleArchNode module, string text) {
            Builder.Append( "| - " );
            Builder.AppendLine( text );
            indent_module = !IsLast( module ) ? "|   " : "    ";
        }
        protected override void AppendLine(NamespaceArchNode @namespace, string text) {
            Builder.Append( indent_module );
            Builder.Append( "| - " );
            Builder.AppendLine( text );
            indent_namespace = !IsLast( @namespace ) ? "|   " : "    ";
        }
        protected override void AppendLine(GroupArchNode group, string text) {
            if (group.IsDefault()) return;
            Builder.Append( indent_module );
            Builder.Append( indent_namespace );
            Builder.Append( "| - " );
            Builder.AppendLine( text );
        }
        protected override void AppendLine(TypeArchNode type, string text) {
            Builder.Append( indent_module );
            Builder.Append( indent_namespace );
            Builder.Append( "|   " );
            Builder.AppendLine( text );
        }


        // Helpers/IsLast
        private static bool IsLast(ModuleArchNode module) {
            return module == module.Project.Modules.Last();
        }
        private static bool IsLast(NamespaceArchNode @namespace) {
            return @namespace == @namespace.Module.Namespaces.Last();
        }


    }
}
