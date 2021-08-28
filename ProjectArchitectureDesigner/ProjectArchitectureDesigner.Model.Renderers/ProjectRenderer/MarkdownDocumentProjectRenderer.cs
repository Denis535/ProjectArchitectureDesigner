// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.CSharp;
    using System.Text.Markdown;
    using ProjectArchitectureDesigner.Model;

    public class MarkdownDocumentProjectRenderer : ProjectRenderer {

        private List<string> Uris { get; } = new List<string>();


        public MarkdownDocumentProjectRenderer() : base( new TextRenderer() ) {
        }
        public MarkdownDocumentProjectRenderer(NodeRenderer renderer) : base( renderer ) {
        }


        // Append/TableOfContents
        protected override void AppendTableOfContents(ProjectArchNode project) {
            Builder.AppendLine( "Table of Contents".Header1() );
            base.AppendTableOfContents( project );
            Builder.AppendLine();
        }
        protected override void AppendTableOfContentsRow(ArchNode node) {
            var text = GetStringWithHighlight( node );
            if (node is ProjectArchNode) {
                Builder.AppendLine( text.Link( Uris ).Item1() );
            }
            if (node is ModuleArchNode) {
                Builder.AppendLine( text.Link( Uris ).Item2() );
            }
            if (node is NamespaceArchNode) {
                Builder.AppendLine( text.Link( Uris ).Item3() );
            }
        }
        // Append/Content
        protected override void AppendContent(ProjectArchNode project) {
            base.AppendContent( project );
        }
        protected override void AppendContentRow(ArchNode node) {
            var text = GetStringWithHighlight( node );
            if (node is ProjectArchNode) {
                Builder.AppendLine( text.Header1() );
            }
            if (node is ModuleArchNode) {
                Builder.AppendLine( text.Header2() );
            }
            if (node is NamespaceArchNode) {
                Builder.AppendLine( text.Header3() );
            }
            if (node is GroupArchNode) {
                Builder.AppendLine( text.Bold().Item1() );
            }
            if (node is TypeArchNode type) {
                Builder.AppendLine( text.Item1() );
                //AppendTypeInfo( Builder, type.TypeInfo );
            }
        }


        // Helpers/AppendTypeInfo
        //private static void AppendTypeInfo(MarkdownBuilder builder, TypeInfo type) {
        //    builder.AppendCSharpCodeBlockStart();
        //    builder.AppendLine( type.GetTypeSyntax() );
        //    AppendMembersInfo( builder, "Fields", type.DeclaredFields );
        //    AppendMembersInfo( builder, "Properties", type.DeclaredProperties );
        //    AppendMembersInfo( builder, "Events", type.DeclaredEvents );
        //    AppendMembersInfo( builder, "Constructors", type.DeclaredConstructors );
        //    AppendMembersInfo( builder, "Methods", type.DeclaredMethods );
        //    builder.AppendCodeBlockEnd();
        //}
        //private static void AppendMembersInfo(MarkdownBuilder builder, string title, IEnumerable<MemberInfo> members) {
        //    if (members.Any()) builder.Append( "// " ).AppendLine( title );
        //    foreach (var member in members.OrderBy( i => i.MetadataToken )) {
        //        builder.AppendLine( member.GetMemberSyntax() );
        //    }
        //}


    }
}
