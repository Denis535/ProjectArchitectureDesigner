// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ProjectArchitecture.Model;

    public static class ProjectAlignedTextRenderer {


        // Render
        public static string RenderToAlignedText(this ProjectArchNode project, Func<TypeArchNode, bool> predicate) {
            var builder = new StringBuilder();
            // Project
            builder.AppendProject( project );
            foreach (var module in project.GetModules( predicate )) {
                // Module
                builder.AppendModule( module );
                foreach (var @namespace in module.GetNamespaces( predicate )) {
                    // Namespace
                    builder.AppendNamespace( @namespace );
                    foreach (var group in @namespace.GetGroup( predicate )) {
                        // Group
                        builder.AppendGroup( group );
                        foreach (var type in group.GetTypes( predicate )) {
                            // Type
                            builder.AppendType( type );
                        }
                    }
                }
            }
            return builder.ToString();
        }


        // AppendObject
        private static void AppendProject(this StringBuilder builder, ProjectArchNode project) {
            builder.AppendLine( project.GetString() );
        }
        private static void AppendModule(this StringBuilder builder, ModuleArchNode module) {
            builder.AppendLine( module.GetString() );
        }
        private static void AppendNamespace(this StringBuilder builder, NamespaceArchNode @namespace) {
            builder.AppendLine( @namespace.GetString() );
        }
        private static void AppendGroup(this StringBuilder builder, GroupArchNode group) {
            if (group.IsDefault) return;
            builder.AppendLine( group.GetString() );
        }
        private static void AppendType(this StringBuilder builder, TypeArchNode type) {
            builder.AppendLine( type.GetString() );
        }


        // Helpers/GetChildren
        internal static IEnumerable<ModuleArchNode> GetModules(this ProjectArchNode project, Func<TypeArchNode, bool> predicate) {
            return project.Modules.Where( i => i.Types.Any( predicate ) );
        }
        internal static IEnumerable<NamespaceArchNode> GetNamespaces(this ModuleArchNode module, Func<TypeArchNode, bool> predicate) {
            return module.Namespaces.Where( i => i.Types.Any( predicate ) );
        }
        internal static IEnumerable<GroupArchNode> GetGroup(this NamespaceArchNode @namespace, Func<TypeArchNode, bool> predicate) {
            return @namespace.Groups.Where( i => i.Types.Any( predicate ) );
        }
        internal static IEnumerable<TypeArchNode> GetTypes(this GroupArchNode group, Func<TypeArchNode, bool> predicate) {
            return group.Types.Where( predicate );
        }
        // Helpers/GetString
        private static string GetString(this ArchNode node) {
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
