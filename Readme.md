# The overview

The package **ProjectArchitecture** allows you to describe the project architecture with a very simple syntax.

It will give you:
 - The readable list of all types.
 - A comprehensive view of architecture.
 - The ability to render the architecture model in convenient format.
 - The ability to validate dependencies between groups of types (third-party library is needed).

# The getting started

In order to describe the architecture you need to declare the architecture model consisting of: project, modules, namespaces, groups and types.
To achieve it you need to write the `ProjectNode` and the `ModuleNode` classes with the list of modules, namespaces, groups and types.
And the roslyn source generator will generate the complex hierarchical structure based on it.

# The examples

## The original source

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
            // ArchNode
            typeof( ArchNode ),
            // ArchNode/Children
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

## The generated source

    // Project: ProjectArchitecture
    public sealed partial class Project_ProjectArchitecture : ProjectArchNode {
        public override string Name { get; } = "ProjectArchitecture";
        public Module_ProjectArchitecture ProjectArchitecture { get; } = new Module_ProjectArchitecture();
        public Module_ProjectArchitecture_Analyzer ProjectArchitecture_Analyzer { get; } = new Module_ProjectArchitecture_Analyzer();
    }
    // Module: ProjectArchitecture
    public sealed partial class Module_ProjectArchitecture : ModuleArchNode {
        public override string Name { get; } = "ProjectArchitecture";
        public Namespace_ProjectArchitecture_Model ProjectArchitecture_Model { get; } = new Namespace_ProjectArchitecture_Model();
        public Namespace_ProjectArchitecture_Renderers ProjectArchitecture_Renderers { get; } = new Namespace_ProjectArchitecture_Renderers();
        // Namespace: ProjectArchitecture.Model
        public class Namespace_ProjectArchitecture_Model : NamespaceArchNode {
            public override string Name { get; } = "ProjectArchitecture.Model";
            public Group_ArchNode ArchNode { get; } = new Group_ArchNode();
            public Group_ArchNode_Children ArchNode_Children { get; } = new Group_ArchNode_Children();
            // Group: ArchNode
            public class Group_ArchNode : GroupArchNode {
                public override string Name { get; } = "ArchNode";
                public TypeArchNode ArchNode { get; } = typeof(ArchNode);
            }
            // Group: ArchNode/Children
            public class Group_ArchNode_Children : GroupArchNode {
                public override string Name { get; } = "ArchNode/Children";
                public TypeArchNode ProjectArchNode { get; } = typeof(ProjectArchNode);
                public TypeArchNode ModuleArchNode { get; } = typeof(ModuleArchNode);
                public TypeArchNode NamespaceArchNode { get; } = typeof(NamespaceArchNode);
                public TypeArchNode GroupArchNode { get; } = typeof(GroupArchNode);
                public TypeArchNode TypeArchNode { get; } = typeof(TypeArchNode);
            }
        }
        // Namespace: ProjectArchitecture.Renderers
        public class Namespace_ProjectArchitecture_Renderers : NamespaceArchNode {
            public override string Name { get; } = "ProjectArchitecture.Renderers";
            public Group_Default Default { get; } = new Group_Default();
            // Group: Default
            public class Group_Default : GroupArchNode {
                public override string Name { get; } = "Default";
                public TypeArchNode ProjectTextRenderer { get; } = typeof(ProjectTextRenderer);
                public TypeArchNode ProjectMarkdownRenderer { get; } = typeof(ProjectMarkdownRenderer);
            }
        }
    }
    // Module: ProjectArchitecture.Analyzer
    public sealed partial class Module_ProjectArchitecture_Analyzer : ModuleArchNode {
        public override string Name { get; } = "ProjectArchitecture.Analyzer";
        public Namespace_ProjectArchitecture_Model ProjectArchitecture_Model { get; } = new Namespace_ProjectArchitecture_Model();
        // Namespace: ProjectArchitecture.Model
        public class Namespace_ProjectArchitecture_Model : NamespaceArchNode {
            public override string Name { get; } = "ProjectArchitecture.Model";
            public Group_Default Default { get; } = new Group_Default();
            // Group: Default
            public class Group_Default : GroupArchNode {
                public override string Name { get; } = "Default";
                public TypeArchNode SourceGenerator { get; } = typeof(SourceGenerator);
            }
        }
    }

## The rendered architecture model

    Project: ProjectArchitecture
    | - Module: ProjectArchitecture
    |   | - Namespace: ProjectArchitecture.Model
    |   |   | - ArchNode
    |   |   |   | * ArchNode
    |   |   | - ArchNode/Children
    |   |       | * ProjectArchNode
    |   |       | * ModuleArchNode
    |   |       | * NamespaceArchNode
    |   |       | * GroupArchNode
    |   |       | * TypeArchNode
    |   | - Namespace: ProjectArchitecture.Renderers
    |       | - Default
    |           | * ProjectTextRenderer
    |           | * ProjectMarkdownRenderer
    | - Module: ProjectArchitecture.Analyzer
        | - Namespace: ProjectArchitecture.Model
            | - Default
                | * SourceGenerator

# The links

 - [ProjectArchitecture](https://github.com/Denis535/ProjectArchitecture)

 - [MakeTypesPublic](https://github.com/Denis535/MakeTypesPublic)