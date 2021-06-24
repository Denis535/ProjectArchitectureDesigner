// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ProjectArchitecture.Model;

    public static class ProjectMarkdownRenderer {


        // Render/Markdown
        public static string RenderToMarkdown(this ProjectArchNode project) {
            var builder = new StringBuilder();
            builder.AppendTableOfContents( project );
            builder.AppendLine();
            builder.AppendBody( project );
            return builder.ToString();
        }
        private static void AppendTableOfContents(this StringBuilder builder, ProjectArchNode project) {
            builder.AppendLine( "Table of Contents".Header1() );
            foreach (var (node, link, uri) in project.DescendantNodesAndSelf.Where( i => i is ProjectArchNode or ModuleArchNode or NamespaceArchNode ).GetLinks()) {
                builder.AppendLine( node.GetDisplayString( link, uri ) );
            }
        }
        private static void AppendBody(this StringBuilder builder, ProjectArchNode project) {
            foreach (var node in project.DescendantNodesAndSelf) {
                if (!node.IsDefaultGroup()) {
                    builder.AppendLine( node.GetDisplayString() );
                }
            }
        }


        // Helpers
        private static bool IsDefaultGroup(this ArchNode node) {
            return node is GroupArchNode group && group.IsDefault;
        }
        // Helpers/GetLinks
        private static IEnumerable<(ArchNode Node, string Link, string Uri)> GetLinks(this IEnumerable<ArchNode> nodes) {
            var prevs = new List<string>();
            return nodes.Select( i => i.GetLink( prevs ) );
        }
        private static (ArchNode Node, string Link, string Uri) GetLink(this ArchNode node, List<string> prevs) {
            var link = node.ToString();
            var uri = node.GetUri( prevs );
            return (node, link, uri);
        }
        private static string GetUri(this ArchNode node, List<string> prevs) {
            var uri = node.ToString().ToLowerInvariant().Trim().Replace( "  ", " " ).Replace( " ", "-" ).Where( IsValid ).Map( string.Concat );
            prevs.Add( uri );
            var id = prevs.Count( i => i == uri ) - 1;
            return id == 0 ? uri : $"{uri}-{id}";
        }
        private static bool IsValid(char @char) {
            return char.IsLetterOrDigit( @char ) || @char is '-';
        }
        // Helpers/GetDisplayString
        private static string GetDisplayString(this ArchNode node, string link, string uri) {
            return node switch {
                ProjectArchNode
                => link.Link( uri ).Item1(),
                ModuleArchNode
                => link.Link( uri ).Item2(),
                NamespaceArchNode
                => link.Link( uri ).Item3(),
                GroupArchNode
                => link.Link( uri ).Item4(),
                { } => throw new ArgumentException( "ArchNode is invalid: " + node ),
                null => throw new ArgumentNullException( nameof( node ), "ArchNode is null" ),
            };
        }
        private static string GetDisplayString(this ArchNode node) {
            return node switch {
                ProjectArchNode
                => "Project: {0}".Format( node.Name ).Header1(),
                ModuleArchNode
                => "Module: {0}".Format( node.Name ).Header2(),
                NamespaceArchNode
                => "Namespace: {0}".Format( node.Name ).Header3(),
                GroupArchNode
                => "Group: {0}".Format( node.Name ).Item1(),
                TypeArchNode
                => "{0}".Format( node.Name ).Item1(),
                { } => throw new ArgumentException( "ArchNode is invalid: " + node ),
                null => throw new ArgumentNullException( nameof( node ), "ArchNode is null" ),
            };
        }


    }
}
