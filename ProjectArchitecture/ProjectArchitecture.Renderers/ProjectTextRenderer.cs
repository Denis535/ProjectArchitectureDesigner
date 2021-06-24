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
            foreach (var node in project.DescendantNodesAndSelf) {
                if (!node.IsDefaultGroup()) {
                    builder.AppendLine( node.GetDisplayString() );
                }
            }
            return builder.ToString();
        }


        // Render/Text/Hierarchical
        public static string RenderToHierarchicalText(this ProjectArchNode project) {
            var builder = new HierarchicalStringBuilder();
            builder.AppendObject( project );
            return builder.ToString();
        }
        private static void AppendObject(this HierarchicalStringBuilder builder, ProjectArchNode project) {
            using (builder.AppendTitle( "Project: {0}", project.Name )) {
                foreach (var module in project.Modules) builder.AppendObject( module );
            }
        }
        private static void AppendObject(this HierarchicalStringBuilder builder, ModuleArchNode module) {
            using var scope = builder.AppendSection( "Module: {0}", module.Name );
            foreach (var @namespace in module.Namespaces) builder.AppendObject( @namespace );
        }
        private static void AppendObject(this HierarchicalStringBuilder builder, NamespaceArchNode @namespace) {
            using var scope = builder.AppendSection( "Namespace: {0}", @namespace.Name );
            foreach (var group in @namespace.Groups) builder.AppendObject( group );
        }
        private static void AppendObject(this HierarchicalStringBuilder builder, GroupArchNode group) {
            if (!group.IsDefault) builder.AppendLineWithPrefix( "| - ", group.Name );
            foreach (var type in group.Types) builder.AppendLineWithPrefix( "|   ", type.Name );
        }


        // Helpers
        private static bool IsDefaultGroup(this ArchNode node) {
            return node is GroupArchNode group && group.IsDefault;
        }
        // Helpers/GetDisplayString
        private static string GetDisplayString(this ArchNode node) {
            return node switch {
                ProjectArchNode
                => "Project:      {0}".Format( node.Name ),
                ModuleArchNode
                => "- Module:     {0}".Format( node.Name ),
                NamespaceArchNode
                => "-- Namespace: {0}".Format( node.Name ),
                GroupArchNode
                => "--- Group:    {0}".Format( node.Name ),
                TypeArchNode
                => "--- Type:     {0}".Format( node.Name ),
                { } => throw new ArgumentException( "ArchNode is invalid: " + node.GetType() ),
                null => throw new ArgumentNullException( nameof( node ), "ArchNode is null" ),
            };
        }


    }
}
