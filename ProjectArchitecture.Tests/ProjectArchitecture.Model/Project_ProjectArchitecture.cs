// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ProjectArchitecture.Renderers;

    // Project/ProjectArchitecture
    public sealed partial class Project_ProjectArchitecture : ProjectNode {
        public Project_ProjectArchitecture() => SetModules(
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
    public sealed partial class Module_ProjectArchitecture : ModuleNode {
        public Module_ProjectArchitecture() => SetNamespacesAndGroupsAndTypes(
            "ProjectArchitecture.Model",
            typeof( Node ),
            // Children
            typeof( ProjectNode ),
            typeof( ModuleNode ),
            typeof( NamespaceNode ),
            typeof( GroupNode ),
            typeof( TypeNode ),
            "ProjectArchitecture.Renderers",
            typeof( ProjectRenderer ),
            typeof( ProjectMarkdownRenderer )
        );
    }

    // Modules/ProjectArchitecture.Analyzer
    public sealed partial class Module_ProjectArchitecture_Analyzer : ModuleNode {
        public Module_ProjectArchitecture_Analyzer() => SetNamespacesAndGroupsAndTypes(
            "ProjectArchitecture.Model",
            typeof( SourceGenerator )
        );
    }

}
