// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ProjectArchitecture.Model;

    public static class ProjectTextRenderer {


        // Render/Text/Table
        public static string RenderToAlignedText(this ProjectArchNode project) {
            var builder = new StringBuilder();
            foreach (var node in project.DescendantNodesAndSelf.WhereNot( IsDefaultGroup )) {
                builder.AppendLine( node.GetAlignedString() );
            }
            return builder.ToString();
        }


        // Render/Text/Hierarchical
        public static string RenderToHierarchicalText(this ProjectArchNode project) {
            var builder = new HierarchicalStringBuilder();
            builder.AppendProject( project );
            return builder.ToString();
        }
        private static void AppendProject(this HierarchicalStringBuilder builder, ProjectArchNode project) {
            using var scope = builder.AppendTitle( project.GetString() );
            foreach (var module in project.Modules) builder.AppendModule( module );
        }
        private static void AppendModule(this HierarchicalStringBuilder builder, ModuleArchNode module) {
            using var scope = builder.AppendSection( module.GetString() );
            foreach (var @namespace in module.Namespaces) builder.AppendNamespace( @namespace );
        }
        private static void AppendNamespace(this HierarchicalStringBuilder builder, NamespaceArchNode @namespace) {
            using var scope = builder.AppendSection( @namespace.GetString() );
            foreach (var group in @namespace.Groups) builder.AppendGroup( group );
        }
        private static void AppendGroup(this HierarchicalStringBuilder builder, GroupArchNode group) {
            if (!group.IsDefault) builder.AppendLine( group.GetString() );
            foreach (var type in group.Types) builder.AppendLineWithPrefix( "|   ", type.GetString() );
        }


        // Helpers
        private static bool IsDefaultGroup(this ArchNode node) {
            return node is GroupArchNode group && group.IsDefault;
        }
        // Helpers/GetString
        private static string GetString(this ArchNode node) {
            return node switch {
                ProjectArchNode
                => "Project: {0}".Format( node.Name ),
                ModuleArchNode
                => "Module: {0}".Format( node.Name ),
                NamespaceArchNode
                => "Namespace: {0}".Format( node.Name ),
                GroupArchNode
                => node.Name,
                TypeArchNode
                => node.Name,
                { } => throw new ArgumentException( "Node is invalid: " + node ),
                null => throw new ArgumentNullException( nameof( node ), "Node is null" ),
            };
        }
        private static string GetAlignedString(this ArchNode node) {
            return node switch {
                ProjectArchNode
                => "Project: ——— {0}".Format( node.Name ),
                ModuleArchNode
                => "Module: ———— {0}".Format( node.Name ),
                NamespaceArchNode
                => "Namespace: — {0}".Format( node.Name ),
                GroupArchNode
                => "Group: ————— {0}".Format( node.Name ),
                TypeArchNode
                => "Type: —————— {0}".Format( node.Name ),
                { } => throw new ArgumentException( "Node is invalid: " + node.GetType() ),
                null => throw new ArgumentNullException( nameof( node ), "Node is null" ),
            };
        }


    }
}
