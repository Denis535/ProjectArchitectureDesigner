# Overview

The package **ProjectArchitecture** allows you to describe the project's architecture model with a simple syntax.
And the roslyn source generator will generate the complex hierarchical structure based on it.

It will give you:
 - A comprehensive view of project's architecture.
 - The ability to render the architecture model in convenient format.
 - The ability to validate dependencies between groups of types (third-party library is needed).

# Getting started

The architecture model consists of project, modules, namespaces, groups and types nodes.
So, you just need to write the project class with modules list.
And the modules classes with the namespaces, groups and types list.

    // Project/ProjectArchitecture
    public sealed partial class Project_ProjectArchitecture : ProjectNode {
        public override string Name => "ProjectArchitecture";

        public Project_ProjectArchitecture() => SetModules(
            typeof( Module_ProjectArchitecture ),
            typeof( Module_ProjectArchitecture_Analyzer )
        );

        // Infrastructure
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
        public override string Name => "ProjectArchitecture";

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
        public override string Name => "ProjectArchitecture.Analyzer";

        public Module_ProjectArchitecture_Analyzer() => SetNamespacesAndGroupsAndTypes(
            "ProjectArchitecture.Model",
            typeof( SourceGenerator )
        );
    }

# Links

[Github](https://github.com/Denis535/ProjectArchitecture)

https://github.com/Denis535/MakeTypesPublic