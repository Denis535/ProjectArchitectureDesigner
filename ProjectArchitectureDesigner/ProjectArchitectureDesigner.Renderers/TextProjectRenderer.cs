// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ProjectArchitectureDesigner.Model;

    public static class TextProjectRenderer {


        // Render
        public static string RenderToText(this ProjectArchNode project, INodeRenderer renderer, Func<TypeArchNode, bool> predicate) {
            var builder = new StringBuilder();
            // Project
            builder.AppendProject( project, renderer );
            foreach (var module in project.GetModules( predicate )) {
                // Module
                builder.AppendModule( module, renderer );
                foreach (var @namespace in module.GetNamespaces( predicate )) {
                    // Namespace
                    builder.AppendNamespace( @namespace, renderer );
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
            return builder.ToString();
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
        // Helpers/AppendObject
        private static void AppendProject(this StringBuilder builder, ProjectArchNode project, INodeRenderer renderer) {
            builder.AppendLine( renderer.Render( project ) );
        }
        private static void AppendModule(this StringBuilder builder, ModuleArchNode module, INodeRenderer renderer) {
            builder.AppendLine( renderer.Render( module ) );
        }
        private static void AppendNamespace(this StringBuilder builder, NamespaceArchNode @namespace, INodeRenderer renderer) {
            builder.AppendLine( renderer.Render( @namespace ) );
        }
        private static void AppendGroup(this StringBuilder builder, GroupArchNode group, INodeRenderer renderer) {
            if (group.IsDefault) return;
            builder.AppendLine( renderer.Render( group ) );
        }
        private static void AppendType(this StringBuilder builder, TypeArchNode type, INodeRenderer renderer) {
            builder.AppendLine( renderer.Render( type ) );
        }


    }
}
