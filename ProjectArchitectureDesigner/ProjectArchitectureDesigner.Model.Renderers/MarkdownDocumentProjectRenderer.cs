// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.Markdown;
    using ProjectArchitectureDesigner.Model;

    public class MarkdownDocumentProjectRenderer : ProjectRenderer {

        private MarkdownBuilder Builder { get; } = new MarkdownBuilder();

        public MarkdownDocumentProjectRenderer(INodeRenderer renderer) : base( renderer ) {
        }


        // Render
        public override string Render(ProjectArchNode project, Func<TypeArchNode, bool> predicate) {
            RenderProject( project, predicate );
            return Builder.ToString();
        }
        // Render/Node
        protected override void RenderProject(ProjectArchNode project, string text, Func<TypeArchNode, bool> predicate) {
            Builder.AppendHeader( text, 1 );
            base.RenderProject( project, text, predicate );
        }
        protected override void RenderModule(ModuleArchNode module, string text, Func<TypeArchNode, bool> predicate) {
            Builder.AppendHeader( text, 2 );
            base.RenderModule( module, text, predicate );
        }
        protected override void RenderNamespace(NamespaceArchNode @namespace, string text, Func<TypeArchNode, bool> predicate) {
            Builder.AppendHeader( text, 3 );
            base.RenderNamespace( @namespace, text, predicate );
        }
        protected override void RenderGroup(GroupArchNode group, string text, Func<TypeArchNode, bool> predicate) {
            if (!group.IsDefault) Builder.AppendItem( text.Bold(), 1 );
            base.RenderGroup( group, text, predicate );
        }
        protected override void RenderType(TypeArchNode type, string text) {
            Builder.AppendItem( text, 1 );
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
