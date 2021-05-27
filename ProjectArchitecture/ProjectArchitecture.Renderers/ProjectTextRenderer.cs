// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ProjectArchitecture.Model;

    public static class ProjectTextRenderer {


        // Render/Text
        public static string RenderToText(this ProjectArchNode project) {
            var builder = new StringBuilder();
            foreach (var node in project.Flatten()) {
                builder.AppendLine( (node.GetTitle() + ": ").PadRight( 11 ) + node.Name );
            }
            return builder.ToString();
        }
        private static string GetTitle(this ArchNode node) {
            return node switch {
                ProjectArchNode => "Project",
                ModuleArchNode => "Module",
                NamespaceArchNode => "Namespace",
                GroupArchNode => "Group",
                TypeArchNode => "Type",
                { } => throw new ArgumentException( "Node is invalid: " + node.GetType() ),
                null => throw new ArgumentNullException( nameof( node ), "Node is null" ),
            };
        }


        // Render/HierarchicalText
        public static string RenderToHierarchicalText(this ProjectArchNode project) {
            var builder = new HierarchicalStringBuilder();
            using (builder.AppendTitle( "Project: {0}", project.Name )) {
                foreach (var module in project.Modules) builder.Render( module );
            }
            return builder.ToString();
        }
        private static void Render(this HierarchicalStringBuilder builder, ModuleArchNode module) {
            using var scope = builder.AppendSection( "Module: {0}", module.Name );
            foreach (var @namespace in module.Namespaces) builder.Render( @namespace );
        }
        private static void Render(this HierarchicalStringBuilder builder, NamespaceArchNode @namespace) {
            using var scope = builder.AppendSection( "Namespace: {0}", @namespace.Name );
            foreach (var group in @namespace.Groups) builder.Render( group );
        }
        private static void Render(this HierarchicalStringBuilder builder, GroupArchNode group) {
            using var scope = builder.AppendSection( group.Name );
            foreach (var type in group.Types) builder.AppendLineWithPrefix( "| * ", type.Name );
        }


    }
}
