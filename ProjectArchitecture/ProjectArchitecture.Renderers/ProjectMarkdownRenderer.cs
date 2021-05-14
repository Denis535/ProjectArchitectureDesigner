// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ProjectArchitecture.Model;

    public static class ProjectMarkdownRenderer {


        public static string RenderMarkdown(this ProjectNode project) {
            var builder = new StringBuilder();
            builder.AppendTableOfContents( project );
            builder.AppendLine();
            builder.AppendBody( project );
            return builder.ToString();
        }


        // Helpers/Project
        private static void AppendTableOfContents(this StringBuilder builder, ProjectNode project) {
            builder.AppendLine( "# Table of Contents" );
            foreach (var (node, link, uri) in project.Flatten().Where( i => i is ProjectNode or ModuleNode or NamespaceNode ).GetLinks()) {
                builder.AppendLine( node.GetLinkString( link, uri ) );
            }
        }
        private static void AppendBody(this StringBuilder builder, ProjectNode project) {
            foreach (var node in project.Flatten()) {
                builder.AppendLine( node.GetItemString() );
            }
        }
        // Helpers/Node
        private static string GetLinkString(this Node node, string link, string uri) {
            return node switch {
                ProjectNode
                => string.Format( "  - [{0}](#{1})", link, uri ),
                ModuleNode
                => string.Format( "    - [{0}](#{1})", link, uri ),
                NamespaceNode
                => string.Format( "      - [{0}](#{1})", link, uri ),
                GroupNode
                => string.Format( "        - [{0}](#{1})", link, uri ),
                { }
                => throw new NotSupportedException( "Node is not supported: " + node.GetType().ToString() ),
                null
                => throw new ArgumentNullException( nameof( node ) ),
            };
        }
        private static string GetItemString(this Node node) {
            return node switch {
                ProjectNode proj
                => "# " + proj,
                ModuleNode module
                => "## " + module,
                NamespaceNode @namespace
                => "### " + @namespace,
                GroupNode group
                => "#### " + group,
                TypeNode type
                => "* " + type.Name,
                { }
                => throw new NotSupportedException( "Node is not supported: " + node.GetType().ToString() ),
                null
                => throw new ArgumentNullException( nameof( node ) ),
            };
        }
        // Helpers/Node/Links
        private static IEnumerable<(Node, string Link, string Uri)> GetLinks(this IEnumerable<Node> nodes) {
            var prevs = new List<string>();
            return nodes.Select( i => i.GetLink( prevs ) );
        }
        private static (Node Item, string Link, string Uri) GetLink(this Node node, List<string> prevs) {
            var link = node.ToString();
            var uri = node.ToString().ToLowerInvariant()
                .Replace( "  ", " " )
                .Replace( " ", "-" )
                .Replace( ".", "" )
                .Replace( ":", "" )
                .Replace( "/", "" );
            prevs.Add( uri );

            var id = prevs.Count( i => i == uri ) - 1;
            if (id != 0) uri += "-" + id;
            return (node, link, uri);
        }


    }
}
