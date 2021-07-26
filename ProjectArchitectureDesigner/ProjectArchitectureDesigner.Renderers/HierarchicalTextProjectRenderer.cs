// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ProjectArchitectureDesigner.Model;

    public static class HierarchicalTextProjectRenderer {


        // Render
        public static string RenderToHierarchicalText(this ProjectArchNode project, INodeRenderer renderer, Func<TypeArchNode, bool> predicate) {
            var builder = new HierarchicalStringBuilder();
            // Project
            using (builder.AppendProject( project, renderer )) {
                foreach (var module in project.GetModules( predicate )) {
                    // Module
                    using (builder.AppendModule( module, renderer )) {
                        foreach (var @namespace in module.GetNamespaces( predicate )) {
                            // Namespace
                            using (builder.AppendNamespace( @namespace, renderer )) {
                                foreach (var group in @namespace.GetGroup( predicate )) {
                                    // Group
                                    builder.AppendGroup( group, renderer );
                                    foreach (var type in group.GetTypes( predicate )) {
                                        // Type
                                        builder.AppendType( type, renderer );
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return builder.ToString();
        }


        // Helpers/AppendObject
        private static IDisposable AppendProject(this HierarchicalStringBuilder builder, ProjectArchNode project, INodeRenderer renderer) {
            return builder.AppendTitle( renderer.Render( project ) );
        }
        private static IDisposable AppendModule(this HierarchicalStringBuilder builder, ModuleArchNode module, INodeRenderer renderer) {
            return builder.AppendSection( renderer.Render( module ) );
        }
        private static IDisposable AppendNamespace(this HierarchicalStringBuilder builder, NamespaceArchNode @namespace, INodeRenderer renderer) {
            return builder.AppendSection( renderer.Render( @namespace ) );
        }
        private static void AppendGroup(this HierarchicalStringBuilder builder, GroupArchNode group, INodeRenderer renderer) {
            if (group.IsDefault) return;
            builder.AppendLineWithPrefix( "| - ", renderer.Render( group ) );
        }
        private static void AppendType(this HierarchicalStringBuilder builder, TypeArchNode type, INodeRenderer renderer) {
            builder.AppendLineWithPrefix( "|   ", renderer.Render( type ) );
        }


    }
}
