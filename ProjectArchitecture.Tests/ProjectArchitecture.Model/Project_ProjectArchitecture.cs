// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.CSharp;
    using System.Text.Markdown;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using ProjectArchitecture.Renderers;

    // Project/ProjectArchitecture
    public sealed partial class Project_ProjectArchitecture : ProjectArchNode {
        protected override void Initialize() => SetChildren(
            typeof( Module_ProjectArchitecture ),
            typeof( Module_ProjectArchitecture_Analyzer ),
            typeof( Module_ProjectArchitecture_Internal )
        );
        protected override bool IsSupported(Type type) {
            return
                base.IsSupported( type ) &&
                !type.IsNestedPrivate &&
                type.Namespace != "System.Runtime.CompilerServices" &&
                type.Namespace != "System.Diagnostics.CodeAnalysis";
        }
    }

    // Modules/ProjectArchitecture
    public sealed partial class Module_ProjectArchitecture : ModuleArchNode {
        protected override void Initialize() => SetChildren(
            "ProjectArchitecture.Model",
            /// ArchNode
            typeof( ArchNode ),
            /// ArchNode/Children
            typeof( ProjectArchNode ),
            typeof( ModuleArchNode ),
            typeof( NamespaceArchNode ),
            typeof( GroupArchNode ),
            typeof( TypeArchNode ),
            "ProjectArchitecture.Renderers",
            typeof( ProjectTextRenderer ),
            typeof( ProjectMarkdownRenderer )
        );
    }

    // Modules/ProjectArchitecture.Analyzer
    public sealed partial class Module_ProjectArchitecture_Analyzer : ModuleArchNode {
        protected override void Initialize() => SetChildren(
            "ProjectArchitecture.Model",
            typeof( SourceGenerator ),
            typeof( SyntaxAnalyzer ),
            typeof( SyntaxGenerator ),
            /// Analysis
            typeof( ProjectInfo ),
            typeof( ModuleInfo ),
            /// Analysis/Entries
            typeof( ModuleEntry ),
            typeof( NamespaceEntry ),
            typeof( GroupEntry ),
            typeof( TypeEntry ),
            "Microsoft.CodeAnalysis.CSharp.Syntax",
            typeof( SyntaxFormatter ),
            typeof( SyntaxFormatterRewriter ),
            typeof( SyntaxUtils ),
            typeof( SyntaxFactoryUtils )
        );
    }

    // Modules/ProjectArchitecture.Internal
    public sealed partial class Module_ProjectArchitecture_Internal : ModuleArchNode {
        protected override void Initialize() => SetChildren(
            "System",
            typeof( Option ),
            typeof( Option<> ),
            typeof( CSharpExtensions ),
            typeof( StringExtensions ),
            typeof( System.TypeExtensions ),
            "System.Collections.Generic",
            typeof( EnumerableExtensions ),
            typeof( PeekableEnumeratorExtensions ),
            typeof( PeekableEnumerator<> ),
            "System.Text",
            typeof( StringBuilderExtensions ),
            typeof( HierarchicalStringBuilder ),
            "System.Text.Markdown",
            typeof( MarkdownBuilder ),
            typeof( MarkdownSyntaxFactory ),
            "System.Text.CSharp",
            typeof( CSharpSyntaxFactory ),
            typeof( MemberExtensions ),
            typeof( MemberSyntaxFactory ),
            typeof( System.Text.CSharp.TypeExtensions ),
            typeof( TypeSyntaxFactory )
        );
    }

}
