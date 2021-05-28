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


        // Helpers/StringBuilder
        private static void AppendTableOfContents(this StringBuilder builder, ProjectArchNode project) {
            builder.AppendLine( "# Table of Contents" );
            foreach (var (node, link, uri) in project.DescendantNodesAndSelf.Where( i => i is ProjectArchNode or ModuleArchNode or NamespaceArchNode ).GetLinks()) {
                builder.AppendLine( node.GetDisplayString( link, uri ) );
            }
        }
        private static void AppendBody(this StringBuilder builder, ProjectArchNode project) {
            foreach (var node in project.DescendantNodesAndSelf) {
                builder.AppendLine( node.GetDisplayString() );
            }
        }
        // Helpers/GetLinks
        private static IEnumerable<(ArchNode, string Link, string Uri)> GetLinks(this IEnumerable<ArchNode> nodes) {
            var prevs = new List<string>();
            return nodes.Select( i => i.GetLink( prevs ) );
        }
        private static (ArchNode Item, string Link, string Uri) GetLink(this ArchNode node, List<string> prevs) {
            var link = node.ToString();
            var uri = node.ToString().GetUri( prevs );
            return (node, link, uri);
        }
        private static string GetUri(this string value, List<string> prevs) {
            var uri = string.Concat( value.Trim().ToLowerInvariant().Select( Escape ) );
            prevs.Add( uri );
            var id = prevs.Count( i => i == uri ) - 1;
            return id == 0 ? uri : $"{uri}-{id}";
        }
        private static char Escape(char @char) {
            return char.IsLetterOrDigit( @char ) ? @char : '-';
        }
        // Helpers/GetDisplayString
        private static string GetDisplayString(this ArchNode node, string link, string uri) {
            return node switch {
                ProjectArchNode
                => string.Format( "  - [{0}](#{1})", link, uri ),
                ModuleArchNode
                => string.Format( "    - [{0}](#{1})", link, uri ),
                NamespaceArchNode
                => string.Format( "      - [{0}](#{1})", link, uri ),
                GroupArchNode
                => string.Format( "        - [{0}](#{1})", link, uri ),
                { } => throw new ArgumentException( "ArchNode is invalid: " + node.GetType() ),
                null => throw new ArgumentNullException( nameof( node ), "ArchNode is null" ),
            };
        }
        private static string GetDisplayString(this ArchNode node) {
            return node switch {
                ProjectArchNode proj
                => "# " + proj,
                ModuleArchNode module
                => "## " + module,
                NamespaceArchNode @namespace
                => "### " + @namespace,
                GroupArchNode group
                => "#### " + group,
                TypeArchNode type
                => "* " + type.Name,
                { } => throw new ArgumentException( "ArchNode is invalid: " + node.GetType() ),
                null => throw new ArgumentNullException( nameof( node ), "ArchNode is null" ),
            };
        }


    }
}
