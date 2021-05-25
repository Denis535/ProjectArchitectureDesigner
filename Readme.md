# The overview

The package **ProjectArchitecture** allows you to describe the project's architecture model with a simple syntax.
And the roslyn source generator will generate the complex hierarchical structure based on it.

It will give you:
 - A comprehensive view of project's architecture.
 - The ability to render the architecture model in convenient format.
 - The ability to validate dependencies between groups of types (third-party library is needed).

# The getting started

The architecture model consists of project, modules, namespaces, groups and types.
So, you just need to declare the project class with modules list.
And the modules classes with the namespaces, groups and types list.

## The example

    // Project/ProjectArchitecture
    public partial class Project_ProjectArchitecture : ProjectNode {
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
    public partial class Module_ProjectArchitecture : ModuleNode {
        protected override void DefineChildren() => SetChildren(
            "ProjectArchitecture.Model",
            typeof( ArchitectureNode ),
            // Node/Children
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
    public partial class Module_ProjectArchitecture_Analyzer : ModuleNode {
        protected override void DefineChildren() => SetChildren(
            "ProjectArchitecture.Model",
            typeof( SourceGenerator )
        );
    }

## The generation example

    // Project: ProjectArchitecture
    public partial class Project_ProjectArchitecture : ProjectNode
    {
        public override string Name { get; } = "ProjectArchitecture";
        public Module_ProjectArchitecture Module_ProjectArchitecture { get; } = new Module_ProjectArchitecture();
        public Module_ProjectArchitecture_Analyzer Module_ProjectArchitecture_Analyzer { get; } = new Module_ProjectArchitecture_Analyzer();
    }

    // Module: ProjectArchitecture
    public partial class Module_ProjectArchitecture : ModuleNode
    {
        public override string Name { get; } = "ProjectArchitecture";
        public Namespace_ProjectArchitecture_Model ProjectArchitecture_Model { get; } = new Namespace_ProjectArchitecture_Model();
        public Namespace_ProjectArchitecture_Renderers ProjectArchitecture_Renderers { get; } = new Namespace_ProjectArchitecture_Renderers();
        
        // Namespace: ProjectArchitecture.Model
        public class Namespace_ProjectArchitecture_Model : NamespaceNode
        {
            public override string Name { get; } = "ProjectArchitecture.Model";
            public Group_Default Default { get; } = new Group_Default();
            public Group_Node_Children Node_Children { get; } = new Group_Node_Children();
            
            // Group: Default
            public class Group_Default : GroupNode
            {
                public override string Name { get; } = "Default";
                public TypeNode ArchitectureNode { get; } = typeof(ArchitectureNode);
            }

            // Group: Node/Children
            public class Group_Node_Children : GroupNode
            {
                public override string Name { get; } = "Node/Children";
                public TypeNode ProjectNode { get; } = typeof(ProjectNode);
                public TypeNode ModuleNode { get; } = typeof(ModuleNode);
                public TypeNode NamespaceNode { get; } = typeof(NamespaceNode);
                public TypeNode GroupNode { get; } = typeof(GroupNode);
                public TypeNode TypeNode { get; } = typeof(TypeNode);
            }
        }

        // Namespace: ProjectArchitecture.Renderers
        public class Namespace_ProjectArchitecture_Renderers : NamespaceNode
        {
            public override string Name { get; } = "ProjectArchitecture.Renderers";
            public Group_Default Default { get; } = new Group_Default();
            
            // Group: Default
            public class Group_Default : GroupNode
            {
                public override string Name { get; } = "Default";
                public TypeNode ProjectRenderer { get; } = typeof(ProjectRenderer);
                public TypeNode ProjectMarkdownRenderer { get; } = typeof(ProjectMarkdownRenderer);
            }
        }
    }

    // Module: ProjectArchitecture.Analyzer
    public partial class Module_ProjectArchitecture_Analyzer : ModuleNode
    {
        public override string Name { get; } = "ProjectArchitecture.Analyzer";
        public Namespace_ProjectArchitecture_Model ProjectArchitecture_Model { get; } = new Namespace_ProjectArchitecture_Model();
        
        // Namespace: ProjectArchitecture.Model
        public class Namespace_ProjectArchitecture_Model : NamespaceNode
        {
            public override string Name { get; } = "ProjectArchitecture.Model";
            public Group_Default Default { get; } = new Group_Default();
            
            // Group: Default
            public class Group_Default : GroupNode
            {
                public override string Name { get; } = "Default";
                public TypeNode SourceGenerator { get; } = typeof(SourceGenerator);
            }
        }
    }

## The rendering example

    Project: ProjectArchitecture
    | - Module: ProjectArchitecture
    |   | - Namespace: ProjectArchitecture.Model
    |   |   | - Default
    |   |   |   | * ArchitectureNode
    |   |   | - Node/Children
    |   |       | * ProjectNode
    |   |       | * ModuleNode
    |   |       | * NamespaceNode
    |   |       | * GroupNode
    |   |       | * TypeNode
    |   | - Namespace: ProjectArchitecture.Renderers
    |       | - Default
    |           | * ProjectRenderer
    |           | * ProjectMarkdownRenderer
    | - Module: ProjectArchitecture.Analyzer
        | - Namespace: ProjectArchitecture.Model
            | - Default
                | * SourceGenerator


# The links

 - [ProjectArchitecture](https://github.com/Denis535/ProjectArchitecture)

 - [MakeTypesPublic](https://github.com/Denis535/MakeTypesPublic)