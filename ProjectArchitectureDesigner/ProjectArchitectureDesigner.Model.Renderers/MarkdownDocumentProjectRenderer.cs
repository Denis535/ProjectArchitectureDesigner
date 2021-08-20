// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.CSharp;
    using System.Text.Markdown;
    using ProjectArchitectureDesigner.Model;

    public class MarkdownDocumentProjectRenderer : ProjectRenderer {

        private MarkdownBuilder Builder { get; } = new MarkdownBuilder();


        public MarkdownDocumentProjectRenderer() : base( new TextRenderer() ) {
        }
        public MarkdownDocumentProjectRenderer(NodeRenderer renderer) : base( renderer ) {
        }


        // Render
        public override MarkdownDocumentProjectRenderer Render(ProjectArchNode project) {
            AppendTableOfContents( project );
            AppendHierarchy( project );
            return this;
        }
        public MarkdownDocumentProjectRenderer Render(ProjectArchNode project, bool withTableOfContents) {
            if (withTableOfContents) AppendTableOfContents( project );
            AppendHierarchy( project );
            return this;
        }


        // AppendTableOfContents
        private void AppendTableOfContents(ProjectArchNode project) {
            var prevs = new List<string>();
            Builder.AppendHeader( "Table of Contents", 1 );
            Builder.AppendItemLink( GetString( project ), 1, prevs );
            foreach (var module in project.Modules) {
                Builder.AppendItemLink( GetString( module ), 2, prevs );
                foreach (var @namespace in module.Namespaces) {
                    Builder.AppendItemLink( GetString( @namespace ), 3, prevs );
                }
            }
            Builder.AppendLine();
        }


        // AppendNode
        protected override void AppendNode(ProjectArchNode project, string text) {
            Builder.AppendHeader( text, 1 );
        }
        protected override void AppendNode(ModuleArchNode module, string text) {
            Builder.AppendHeader( text, 2 );
        }
        protected override void AppendNode(NamespaceArchNode @namespace, string text) {
            Builder.AppendHeader( text, 3 );
        }
        protected override void AppendNode(GroupArchNode group, string text) {
            if (group.IsDefault()) return;
            Builder.AppendItem( text.Bold(), 1 );
        }
        protected override void AppendNode(TypeArchNode type, string text) {
            Builder.AppendItem( text, 1 );
            //AppendTypeInfo( Builder, type.TypeInfo );
        }


        // Utils
        public override string ToString() {
            return Builder.ToString();
        }


        // Helpers/AppendTypeInfo
        private static void AppendTypeInfo(MarkdownBuilder builder, TypeInfo type) {
            builder.AppendCSharpCodeBlockStart();
            builder.AppendLine( type.GetTypeSyntax() );
            AppendMembersInfo( builder, "Fields", type.DeclaredFields );
            AppendMembersInfo( builder, "Properties", type.DeclaredProperties );
            AppendMembersInfo( builder, "Events", type.DeclaredEvents );
            AppendMembersInfo( builder, "Constructors", type.DeclaredConstructors );
            AppendMembersInfo( builder, "Methods", type.DeclaredMethods );
            builder.AppendCodeBlockEnd();
        }
        private static void AppendMembersInfo(MarkdownBuilder builder, string title, IEnumerable<MemberInfo> members) {
            if (members.Any()) builder.Append( "// " ).AppendLine( title );
            foreach (var member in members.OrderBy( i => i.MetadataToken )) {
                builder.AppendLine( member.GetMemberSyntax() );
            }
        }


    }
}
