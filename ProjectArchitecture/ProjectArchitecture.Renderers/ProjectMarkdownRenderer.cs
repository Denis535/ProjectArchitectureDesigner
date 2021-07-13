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
            builder.AppendTableOfContents( project.DescendantNodesAndSelf.Where( IsHeader ).Select( GetHeader ) );
            builder.AppendLine();
            builder.AppendBody( project );
            return builder.ToString();
        }
        private static void AppendBody(this MarkdownBuilder builder, ProjectArchNode project) {
            foreach (var node in project.DescendantNodesAndSelf.WhereNot( IsDefaultGroup )) {
                if (node.IsHeader()) {
                    builder.AppendHeader( node.GetDisplayString_Header(), node.GetHeaderLevel() );
                } else {
                    builder.AppendItem( node.GetDisplayString_Item(), 1 );
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
        private static (string Text, int Level) GetHeader(this ArchNode node) {
            return (node.GetDisplayString_Header(), node.GetHeaderLevel());
        }
        private static int GetHeaderLevel(this ArchNode node) {
            return node switch {
                ProjectArchNode
                => 1,
                ModuleArchNode
                => 2,
                NamespaceArchNode
                => 3,
                { } => throw new ArgumentException( "Node is invalid: " + node ),
                null => throw new ArgumentNullException( nameof( node ), "Node is null" ),
            };
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
                => node.Name.Italic(),
                TypeArchNode
                => node.Name.Bold(),
                { } => throw new ArgumentException( "Node is invalid: " + node ),
                null => throw new ArgumentNullException( nameof( node ), "Node is null" ),
            };
        }


    }
}
