// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ProjectArchitecture.Model;

    public static class ProjectMarkdownRenderer {


        public static string RenderMarkdown(this Project project) {
            var builder = new StringBuilder();
            builder.AppendTableOfContents( project );
            builder.AppendLine();
            builder.AppendBody( project );
            return builder.ToString();
        }


        // Helpers/Project
        private static void AppendTableOfContents(this StringBuilder builder, Project project) {
            builder.AppendLine( "# Table of Contents" );
            foreach (var (node, link, uri) in project.Flatten().Where( i => i is Project or Module or Namespace ).GetLinks()) {
                builder.AppendLine( node.GetString( link, uri ) );
            }
        }
        private static void AppendBody(this StringBuilder builder, Project project) {
            foreach (var node in project.Flatten()) {
                builder.AppendLine( node.GetString() );
            }
        }
        // Helpers/Node
        private static string GetString(this Node node, string link, string uri) {
            return node switch {
                Project proj
                => string.Format( "  - [{0}](#{1})", link, uri ),
                Module module
                => string.Format( "    - [{0}](#{1})", link, uri ),
                Namespace @namespace
                => string.Format( "      - [{0}](#{1})", link, uri ),
                Group group
                => string.Format( "        - [{0}](#{1})", link, uri ),
                { }
                => throw new NotImplementedException( node.ToString() ),
                null
                => throw new NullReferenceException( "Null" ),
            };
        }
        private static string GetString(this Node node) {
            return node switch {
                Project proj
                => "# " + proj,
                Module module
                => "## " + module,
                Namespace @namespace
                => "### " + @namespace,
                Group group
                => "#### " + group,
                TypeNode type
                => "* " + type.Name,
                { }
                => throw new NotImplementedException( node.ToString() ),
                null
                => throw new NullReferenceException( "Null" ),
            };
        }
        // Helpers/Node/Link
        private static IEnumerable<(Node, string, string)> GetLinks(this IEnumerable<Node> nodes) {
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
        // Helpers/Linq
        //private static IEnumerable<(T, IEnumerable<T>)> WithPrevious<T>(this IEnumerable<T> source) {
        //    var previous = new List<T>();
        //    foreach (var item in source) {
        //        yield return (item, previous);
        //        previous.Add( item );
        //    }
        //}


    }
}
