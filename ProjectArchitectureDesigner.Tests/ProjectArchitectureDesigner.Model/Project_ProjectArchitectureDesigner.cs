// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.CSharp;
    using System.Text.Markdown;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using ProjectArchitectureDesigner.Model.Renderers;

    // Project/ProjectArchitectureDesigner
    public sealed partial class Project_ProjectArchitectureDesigner : ProjectArchNode {
        protected override void Initialize() => SetChildren(
            typeof( Module_ProjectArchitectureDesigner ),
            typeof( Module_ProjectArchitectureDesigner_Analyzer ),
            typeof( Module_ProjectArchitectureDesigner_Internal )
        );
        public override bool IsSupported(Type type) {
            return
                base.IsSupported( type ) &&
                !type.IsNestedPrivate &&
                type.Namespace != "System.Runtime.CompilerServices" &&
                type.Namespace != "System.Diagnostics.CodeAnalysis";
        }
        public override bool IsVisible(TypeArchNode type) {
            return
                base.IsVisible( type ) &&
                !type.Module.Name.EndsWith( ".Internal" ) &&
                !type.Module.Name.EndsWith( ".Analyzer" );
        }
    }

    // Modules/ProjectArchitectureDesigner
    public sealed partial class Module_ProjectArchitectureDesigner : ModuleArchNode {
        public override System.Reflection.Assembly? Assembly => typeof( ArchNode ).Assembly;
        protected override void Initialize() => SetChildren(
            "ProjectArchitectureDesigner.Model",
            /// Group: ArchNode
            typeof( ArchNode ),
            /// Group: ArchNode/Children
            typeof( ProjectArchNode ),
            typeof( ModuleArchNode ),
            typeof( NamespaceArchNode ),
            typeof( GroupArchNode ),
            typeof( TypeArchNode ),
            "ProjectArchitectureDesigner.Model.Renderers",
            /// Group: ProjectRenderer
            typeof( ProjectRenderer ),
            typeof( TextProjectRenderer ),
            typeof( HierarchicalTextProjectRenderer ),
            typeof( MarkdownDocumentProjectRenderer ),
            /// Group: NodeRenderer
            typeof( INodeRenderer ),
            typeof( DelegateNodeRenderer ),
            typeof( TextNodeRenderer ),
            typeof( LeftAlignedTextNodeRenderer ),
            typeof( RightAlignedTextNodeRenderer )
        );
    }

    // Modules/ProjectArchitectureDesigner.Analyzer
    public sealed partial class Module_ProjectArchitectureDesigner_Analyzer : ModuleArchNode {
        public override System.Reflection.Assembly? Assembly => typeof( SourceGenerator ).Assembly;
        protected override void Initialize() => SetChildren(
            "ProjectArchitectureDesigner.Model",
            typeof( SourceGenerator ),
            typeof( SyntaxAnalyzer ),
            typeof( SyntaxGenerator ),
            /// Group: ClassInfo
            typeof( ProjectInfo ),
            typeof( ModuleInfo ),
            /// Group: ClassInfo/Entry
            typeof( ModuleEntry ),
            typeof( NamespaceEntry ),
            typeof( GroupEntry ),
            typeof( TypeEntry ),
            "Microsoft.CodeAnalysis.CSharp.Syntax",
            typeof( SyntaxFormatter ),
            typeof( SyntaxFormatterHelper ),
            typeof( SyntaxFormatterHelper2 ),
            typeof( SyntaxFactory2 ),
            typeof( SyntaxUtils )
        );
    }

    // Modules/ProjectArchitectureDesigner.Internal
    public sealed partial class Module_ProjectArchitectureDesigner_Internal : ModuleArchNode {
        public override System.Reflection.Assembly? Assembly => typeof( Option ).Assembly;
        protected override void Initialize() => SetChildren(
            "System",
            typeof( Option ),
            typeof( Option<> ),
            typeof( CSharpExtensions ),
            typeof( StringExtensions ),
            typeof( TypeExtensions ),
            "System.Collections.Generic",
            typeof( EnumerableExtensions ),
            typeof( PeekableEnumeratorExtensions ),
            typeof( PeekableEnumerator<> ),
            "System.Text",
            typeof( StringBuilderExtensions ),
            typeof( HierarchicalStringBuilder ),
            "System.Text.CSharp",
            typeof( CSharpSyntaxFactory ),
            typeof( CSharpSyntaxFactoryHelper ),
            typeof( CSharpSyntaxFactoryHelper2 ),
            typeof( CSharpSyntaxFactoryHelper3 ),
            typeof( AccessLevelExtensions ),
            typeof( AccessLevel ),
            "System.Text.Markdown",
            typeof( MarkdownBuilder ),
            typeof( MarkdownSyntaxFactory )
        );
    }

}
