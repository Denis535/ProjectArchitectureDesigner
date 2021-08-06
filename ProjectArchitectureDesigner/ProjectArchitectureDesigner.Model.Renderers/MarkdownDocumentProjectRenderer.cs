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

        public MarkdownDocumentProjectRenderer() : base( new TextNodeRenderer() ) {
        }
        public MarkdownDocumentProjectRenderer(INodeRenderer renderer) : base( renderer ) {
        }


        // Render
        public override string Render(ProjectArchNode project, Func<TypeArchNode, bool> predicate) {
            RenderProject( project, predicate );
            return Builder.ToString();
        }
        // Render/Node
        protected override void RenderProject(ProjectArchNode project, Func<TypeArchNode, bool> predicate) {
            Builder.AppendHeader( Render( project ), 1 );
            base.RenderProject( project, predicate );
        }
        protected override void RenderModule(ModuleArchNode module, Func<TypeArchNode, bool> predicate) {
            Builder.AppendHeader( Render( module ), 2 );
            base.RenderModule( module, predicate );
        }
        protected override void RenderNamespace(NamespaceArchNode @namespace, Func<TypeArchNode, bool> predicate) {
            Builder.AppendHeader( Render( @namespace ), 3 );
            base.RenderNamespace( @namespace, predicate );
        }
        protected override void RenderGroup(GroupArchNode group, Func<TypeArchNode, bool> predicate) {
            if (!group.IsDefault) Builder.AppendItem( Render( group ).Bold(), 1 );
            base.RenderGroup( group, predicate );
        }
        protected override void RenderType(TypeArchNode type) {
            Builder.AppendItem( Render( type ), 1 );
            //AppendTypeInfo( Builder, type.TypeInfo );
        }
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
