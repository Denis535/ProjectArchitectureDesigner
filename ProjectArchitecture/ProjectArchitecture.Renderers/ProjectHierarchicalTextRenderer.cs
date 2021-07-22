// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ProjectArchitecture.Model;

    public static class ProjectHierarchicalTextRenderer {


        // Render
        public static string RenderToHierarchicalText(this ProjectArchNode project, Func<TypeArchNode, bool> predicate) {
            var builder = new HierarchicalStringBuilder();
            // Project
            using (builder.AppendProject( project )) {
                foreach (var module in project.GetModules( predicate )) {
                    // Module
                    using (builder.AppendModule( module )) {
                        foreach (var @namespace in module.GetNamespaces( predicate )) {
                            // Namespace
                            using (builder.AppendNamespace( @namespace )) {
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
                    }
                }
            }
            return builder.ToString();
        }


        // AppendObject
        private static IDisposable AppendProject(this HierarchicalStringBuilder builder, ProjectArchNode project) {
            return builder.AppendTitle( project.GetString() );
        }
        private static IDisposable AppendModule(this HierarchicalStringBuilder builder, ModuleArchNode module) {
            return builder.AppendSection( module.GetString() );
        }
        private static IDisposable AppendNamespace(this HierarchicalStringBuilder builder, NamespaceArchNode @namespace) {
            return builder.AppendSection( @namespace.GetString() );
        }
        private static void AppendGroup(this HierarchicalStringBuilder builder, GroupArchNode group) {
            if (group.IsDefault) return;
            builder.AppendLineWithPrefix( "| - ", group.GetString() );
        }
        private static void AppendType(this HierarchicalStringBuilder builder, TypeArchNode type) {
            builder.AppendLineWithPrefix( "|   ", type.GetString() );
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


    }
}
