// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ProjectArchitecture.Renderers;

    // Project/ProjectArchitecture
    public sealed partial class Project_ProjectArchitecture : ProjectArchNode {
        protected override void DefineChildren() => SetChildren(
            typeof( Module_ProjectArchitecture ),
            typeof( Module_ProjectArchitecture_Analyzer )
        );
        protected override bool IsSupported(Type type) {
            return
                base.IsSupported( type ) &&
                type.IsVisible &&
                !type.Namespace!.StartsWith( "System" ) &&
                !type.Namespace!.StartsWith( "Microsoft" );
        }
    }

    // Modules/ProjectArchitecture
    public sealed partial class Module_ProjectArchitecture : ModuleArchNode {
        protected override void DefineChildren() => SetChildren(
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
        protected override void DefineChildren() => SetChildren(
            "ProjectArchitecture.Model",
            typeof( SourceGenerator )
        );
    }

}
