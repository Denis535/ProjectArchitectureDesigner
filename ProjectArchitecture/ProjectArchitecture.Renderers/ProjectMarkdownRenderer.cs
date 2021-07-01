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
            foreach (var node in project.DescendantNodesAndSelf.Where( IsNotDefaultGroup )) {
                if (node.IsHeader()) {
                    builder.AppendHeader( node.GetDisplayString(), node.GetLevel() );
                } else {
                    builder.AppendItem( node.GetDisplayString(), 1 );
                }
            }
        }


        // Helpers/ArchNode
        private static bool IsHeader(this ArchNode node) {
            return node is ProjectArchNode or ModuleArchNode or NamespaceArchNode;
        }
        private static (string Text, int Level) GetHeader(this ArchNode node) {
            return (node.GetDisplayString(), node.GetLevel());
        }
        private static bool IsNotDefaultGroup(this ArchNode node) {
            return !(node is GroupArchNode group && group.IsDefault);
        }
        private static string GetDisplayString(this ArchNode node) {
            return node switch {
                ProjectArchNode => "Project: {0}".Format( node.Name ),
                ModuleArchNode => "Module: {0}".Format( node.Name ),
                NamespaceArchNode => "Namespace: {0}".Format( node.Name ),
                GroupArchNode => "Group: {0}".Format( node.Name ),
                TypeArchNode => "{0}".Format( node.Name ),
                { } => throw new ArgumentException( "ArchNode is invalid: " + node ),
                null => throw new ArgumentNullException( nameof( node ), "ArchNode is null" ),
            };
        }
        private static int GetLevel(this ArchNode node) {
            return node switch {
                ProjectArchNode => 1,
                ModuleArchNode => 2,
                NamespaceArchNode => 3,
                GroupArchNode => 4,
                TypeArchNode => 5,
                { } => throw new ArgumentException( "ArchNode is invalid: " + node ),
                null => throw new ArgumentNullException( nameof( node ), "ArchNode is null" ),
            };
        }


    }
}
