// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Markdown;
    using ProjectArchitecture.Model;

    public static class ProjectMarkdownRenderer {


        // Render/Markdown
        public static string RenderToMarkdown(this ProjectArchNode project) {
            var builder = new MarkdownBuilder();
            builder.AppendTableOfContents( project );
            builder.AppendLine();
            builder.AppendBody( project );
            return builder.ToString();
        }
        private static void AppendTableOfContents(this MarkdownBuilder builder, ProjectArchNode project) {
            var prevs = new List<string>();
            builder.AppendHeader( "Table of Contents", 1 );
            foreach (var header in project.DescendantNodesAndSelf.Where( IsHeader )) {
                builder.AppendItemLink( header.GetString(), header.GetLevel(), prevs );
            }
        }
        private static void AppendBody(this MarkdownBuilder builder, ProjectArchNode project) {
            foreach (var node in project.DescendantNodesAndSelf.WhereNot( IsDefaultGroup )) {
                if (node.IsHeader()) {
                    builder.AppendHeader( node.GetString(), node.GetLevel() );
                } else {
                    builder.AppendItem( node.GetString(), 1 );
                }
            }
        }


        // Helpers
        private static bool IsDefaultGroup(this ArchNode node) {
            return node is GroupArchNode group && group.IsDefault;
        }
        private static bool IsHeader(this ArchNode node) {
            return node is ProjectArchNode or ModuleArchNode or NamespaceArchNode;
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
        // Helpers/GetLevel
        private static int GetLevel(this ArchNode node) {
            return node switch {
                ProjectArchNode
                => 1,
                ModuleArchNode
                => 2,
                NamespaceArchNode
                => 3,
                GroupArchNode
                => 4,
                TypeArchNode
                => 5,
                { } => throw new ArgumentException( "Node is invalid: " + node ),
                null => throw new ArgumentNullException( nameof( node ), "Node is null" ),
            };
        }


    }
}
