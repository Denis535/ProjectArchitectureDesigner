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
            foreach (var node in project.DescendantNodesAndSelf.WhereNot( IsDefaultGroup )) {
                builder.AppendLine( node.GetDisplayString_Item_Aligned() );
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
            using var scope = builder.AppendTitle( project.GetDisplayString_Header() );
            foreach (var module in project.Modules) builder.AppendModule( module );
        }
        private static void AppendModule(this HierarchicalStringBuilder builder, ModuleArchNode module) {
            using var scope = builder.AppendSection( module.GetDisplayString_Header() );
            foreach (var @namespace in module.Namespaces) builder.AppendNamespace( @namespace );
        }
        private static void AppendNamespace(this HierarchicalStringBuilder builder, NamespaceArchNode @namespace) {
            using var scope = builder.AppendSection( @namespace.GetDisplayString_Header() );
            foreach (var group in @namespace.Groups) builder.AppendGroup( group );
        }
        private static void AppendGroup(this HierarchicalStringBuilder builder, GroupArchNode group) {
            if (!group.IsDefault) builder.AppendLine( group.GetDisplayString_Item() );
            foreach (var type in group.Types) builder.AppendLineWithPrefix( "|   ", type.GetDisplayString_Item() );
        }


        // Helpers
        private static bool IsDefaultGroup(this ArchNode node) {
            return node is GroupArchNode group && group.IsDefault;
        }
        // Helpers/GetDisplayString
        private static string GetDisplayString_Header(this ArchNode node) {
            return node switch {
                ProjectArchNode
                => "Project: {0}".Format( node.Name ),
                ModuleArchNode
                => "Module: {0}".Format( node.Name ),
                NamespaceArchNode
                => "Namespace: {0}".Format( node.Name ),
                { } => throw new ArgumentException( "Node is invalid: " + node ),
                null => throw new ArgumentNullException( nameof( node ), "Node is null" ),
            };
        }
        private static string GetDisplayString_Item(this ArchNode node) {
            return node switch {
                GroupArchNode
                => node.Name,
                TypeArchNode
                => node.Name,
                { } => throw new ArgumentException( "Node is invalid: " + node.GetType() ),
                null => throw new ArgumentNullException( nameof( node ), "Node is null" ),
            };
        }
        private static string GetDisplayString_Item_Aligned(this ArchNode node) {
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
