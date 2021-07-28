// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.CSharp;
    using System.Text.Markdown;
    using ProjectArchitectureDesigner.Model;

    public static class MarkdownDocumentProjectRenderer {


        // Render
        public static string RenderToMarkdownDocument(this ProjectArchNode project, INodeRenderer renderer, Func<TypeArchNode, bool> predicate) {
            var builder = new MarkdownBuilder();
            builder.AppendTableOfContents( project, renderer, predicate );
            builder.AppendBody( project, renderer, predicate );
            return builder.ToString();
        }
        private static void AppendTableOfContents(this MarkdownBuilder builder, ProjectArchNode project, INodeRenderer renderer, Func<TypeArchNode, bool> predicate) {
            var prevs = new List<string>();
            builder.AppendHeader( "Table of Contents", 1 );
            // Project
            builder.AppendProject( project, renderer, prevs );
            foreach (var module in project.GetModules( predicate )) {
                // Module
                builder.AppendModule( module, renderer, prevs );
                foreach (var @namespace in module.GetNamespaces( predicate )) {
                    // Namespace
                    builder.AppendNamespace( @namespace, renderer, prevs );
                }
            }
        }
        private static void AppendBody(this MarkdownBuilder builder, ProjectArchNode project, INodeRenderer renderer, Func<TypeArchNode, bool> predicate) {
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
        }


        // AppendObject
        private static void AppendProject(this MarkdownBuilder builder, ProjectArchNode project, INodeRenderer renderer, IList<string> prevs) {
            builder.AppendItemLink( renderer.Render( project ), 1, prevs );
        }
        private static void AppendModule(this MarkdownBuilder builder, ModuleArchNode module, INodeRenderer renderer, IList<string> prevs) {
            builder.AppendItemLink( renderer.Render( module ), 2, prevs );
        }
        private static void AppendNamespace(this MarkdownBuilder builder, NamespaceArchNode @namespace, INodeRenderer renderer, IList<string> prevs) {
            builder.AppendItemLink( renderer.Render( @namespace ), 3, prevs );
        }
        // AppendObject
        private static void AppendProject(this MarkdownBuilder builder, ProjectArchNode project, INodeRenderer renderer) {
            builder.AppendHeader( renderer.Render( project ), 1 );
        }
        private static void AppendModule(this MarkdownBuilder builder, ModuleArchNode module, INodeRenderer renderer) {
            builder.AppendHeader( renderer.Render( module ), 2 );
        }
        private static void AppendNamespace(this MarkdownBuilder builder, NamespaceArchNode @namespace, INodeRenderer renderer) {
            builder.AppendHeader( renderer.Render( @namespace ), 3 );
        }
        private static void AppendGroup(this MarkdownBuilder builder, GroupArchNode group, INodeRenderer renderer) {
            if (group.IsDefault) return;
            builder.AppendItem( renderer.Render( group ).Bold(), 1 );
        }
        private static void AppendType(this MarkdownBuilder builder, TypeArchNode type, INodeRenderer renderer) {
            builder.AppendItem( renderer.Render( type ), 1 );
            //builder.AppendCSharpCodeBlockStart();
            //builder.AppendLine( type.TypeInfo.GetTypeSyntax() );
            //builder.AppendMembersInfo( "Fields", type.DeclaredFields );
            //builder.AppendMembersInfo( "Properties", type.DeclaredProperties );
            //builder.AppendMembersInfo( "Events", type.DeclaredEvents );
            //builder.AppendMembersInfo( "Constructors", type.DeclaredConstructors );
            //builder.AppendMembersInfo( "Methods", type.DeclaredMethods );
            //builder.AppendCodeBlockEnd();
        }
        //private static void AppendMembersInfo(this MarkdownBuilder builder, string title, IEnumerable<MemberInfo> members) {
        //    if (members.Any()) builder.Append( "// " ).AppendLine( title );
        //    foreach (var member in members.OrderBy( i => i.MetadataToken )) {
        //        builder.AppendLine( member.GetMemberSyntax() );
        //    }
        //}


    }
}
