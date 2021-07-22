// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.Markdown;
    using ProjectArchitecture.Model;

    public static class ProjectMarkdownRenderer {


        // Render
        public static string RenderToMarkdown(this ProjectArchNode project, Func<TypeArchNode, bool> predicate) {
            var builder = new MarkdownBuilder();
            builder.AppendTableOfContents( project, predicate );
            builder.AppendLine();
            builder.AppendBody( project, predicate );
            return builder.ToString();
        }
        private static void AppendTableOfContents(this MarkdownBuilder builder, ProjectArchNode project, Func<TypeArchNode, bool> predicate) {
            var prevs = new List<string>();
            builder.AppendHeader( "Table of Contents", 1 );
            // Project
            builder.AppendProject( project, prevs );
            foreach (var module in project.GetModules( predicate )) {
                // Module
                builder.AppendModule( module, prevs );
                foreach (var @namespace in module.GetNamespaces( predicate )) {
                    // Namespace
                    builder.AppendNamespace( @namespace, prevs );
                }
            }
        }
        private static void AppendBody(this MarkdownBuilder builder, ProjectArchNode project, Func<TypeArchNode, bool> predicate) {
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
        }


        // AppendObject
        private static void AppendProject(this MarkdownBuilder builder, ProjectArchNode project, IList<string> prevs) {
            builder.AppendItemLink( project.GetString(), 1, prevs );
        }
        private static void AppendModule(this MarkdownBuilder builder, ModuleArchNode module, IList<string> prevs) {
            builder.AppendItemLink( module.GetString(), 2, prevs );
        }
        private static void AppendNamespace(this MarkdownBuilder builder, NamespaceArchNode @namespace, IList<string> prevs) {
            builder.AppendItemLink( @namespace.GetString(), 3, prevs );
        }
        // AppendObject
        private static void AppendProject(this MarkdownBuilder builder, ProjectArchNode project) {
            builder.AppendHeader( project.GetString(), 1 );
        }
        private static void AppendModule(this MarkdownBuilder builder, ModuleArchNode module) {
            builder.AppendHeader( module.GetString(), 2 );
        }
        private static void AppendNamespace(this MarkdownBuilder builder, NamespaceArchNode @namespace) {
            builder.AppendHeader( @namespace.GetString(), 3 );
        }
        private static void AppendGroup(this MarkdownBuilder builder, GroupArchNode group) {
            if (group.IsDefault) return;
            builder.AppendItem( group.GetString(), 1 );
        }
        private static void AppendType(this MarkdownBuilder builder, TypeArchNode type) {
            builder.AppendItem( type.GetString(), 1 );
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
                => node.Name.Italic(),
                TypeArchNode
                => node.Name.Bold(),
                { } => throw new ArgumentException( "Node is invalid: " + node ),
                null => throw new ArgumentNullException( nameof( node ), "Node is null" ),
            };
        }


    }
}
