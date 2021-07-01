# The overview

The **ProjectArchitectureDesigner** package allows you to describe the complex hierarchical structure of your project with a very, very simple code.
Thanks to the Roslyn source generator we can now generate code of any complexity based on your very simple code.

It will give you:
 - The readable and comprehensive view of project.
 - The ability to render the project structure into any convenient format.
 - The ability to validate dependencies between groups of types (third-party library is needed).

# The Api overview

    Project: ProjectArchitecture
    | - Module: ProjectArchitecture
    |   | - Namespace: ProjectArchitecture.Model
    |   |   | - ArchNode
    |   |   |   ArchNode
    |   |   | - ArchNode/Children
    |   |   |   ProjectArchNode
    |   |   |   ModuleArchNode
    |   |   |   NamespaceArchNode
    |   |   |   GroupArchNode
    |   |   |   TypeArchNode
    |   | - Namespace: ProjectArchitecture.Renderers
    |       |   ProjectTextRenderer
    |       |   ProjectMarkdownRenderer
    | - Module: ProjectArchitecture.Analyzer
        | - Namespace: ProjectArchitecture.Model
            |   SourceGenerator

# The getting started

In order to describe your project you just need to write the `ProjectArchNode` and the `ModuleArcNode` classes with the list of modules, namespaces, groups and types.

# The examples

## The original source

```csharp
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
```

## The generated source

```csharp
    // Project: ProjectArchitecture
    public sealed partial class Project_ProjectArchitecture : ProjectArchNode {
        public override string Name { get; } = "ProjectArchitecture";
        public Module_ProjectArchitecture ProjectArchitecture { get; } = new Module_ProjectArchitecture(); // Module: Module_ProjectArchitecture
        public Module_ProjectArchitecture_Analyzer ProjectArchitecture_Analyzer { get; } = new Module_ProjectArchitecture_Analyzer(); // Module: Module_ProjectArchitecture_Analyzer
    }

    // Module: ProjectArchitecture
    public sealed partial class Module_ProjectArchitecture : ModuleArchNode {
        public override string Name { get; } = "ProjectArchitecture";
        public Namespace_ProjectArchitecture_Model ProjectArchitecture_Model { get; } = new Namespace_ProjectArchitecture_Model(); // Namespace: ProjectArchitecture.Model
        public Namespace_ProjectArchitecture_Renderers ProjectArchitecture_Renderers { get; } = new Namespace_ProjectArchitecture_Renderers(); // Namespace: ProjectArchitecture.Renderers
        // Namespace: ProjectArchitecture.Model
        public class Namespace_ProjectArchitecture_Model : NamespaceArchNode {
            public override string Name { get; } = "ProjectArchitecture.Model";
            public Group_ArchNode ArchNode { get; } = new Group_ArchNode(); // Group: ArchNode
            public Group_ArchNode_Children ArchNode_Children { get; } = new Group_ArchNode_Children(); // Group: ArchNode/Children
            // Group: ArchNode
            public class Group_ArchNode : GroupArchNode {
                public override string Name { get; } = "ArchNode";
                public TypeArchNode ArchNode { get; } = typeof(ArchNode); // Type: ArchNode
            }
            // Group: ArchNode/Children
            public class Group_ArchNode_Children : GroupArchNode {
                public override string Name { get; } = "ArchNode/Children";
                public TypeArchNode ProjectArchNode { get; } = typeof(ProjectArchNode); // Type: ProjectArchNode
                public TypeArchNode ModuleArchNode { get; } = typeof(ModuleArchNode); // Type: ModuleArchNode
                public TypeArchNode NamespaceArchNode { get; } = typeof(NamespaceArchNode); // Type: NamespaceArchNode
                public TypeArchNode GroupArchNode { get; } = typeof(GroupArchNode); // Type: GroupArchNode
                public TypeArchNode TypeArchNode { get; } = typeof(TypeArchNode); // Type: TypeArchNode
            }
        }
        // Namespace: ProjectArchitecture.Renderers
        public class Namespace_ProjectArchitecture_Renderers : NamespaceArchNode {
            public override string Name { get; } = "ProjectArchitecture.Renderers";
            public Group_Default Default { get; } = new Group_Default(); // Group: Default
            // Group: Default
            public class Group_Default : GroupArchNode {
                public override string Name { get; } = "Default";
                public TypeArchNode ProjectTextRenderer { get; } = typeof(ProjectTextRenderer); // Type: ProjectTextRenderer
                public TypeArchNode ProjectMarkdownRenderer { get; } = typeof(ProjectMarkdownRenderer); // Type: ProjectMarkdownRenderer
            }
        }
    }

    // Module: ProjectArchitecture.Analyzer
    public sealed partial class Module_ProjectArchitecture_Analyzer : ModuleArchNode {
        public override string Name { get; } = "ProjectArchitecture.Analyzer";
        public Namespace_ProjectArchitecture_Model ProjectArchitecture_Model { get; } = new Namespace_ProjectArchitecture_Model(); // Namespace: ProjectArchitecture.Model
        // Namespace: ProjectArchitecture.Model
        public class Namespace_ProjectArchitecture_Model : NamespaceArchNode {
            public override string Name { get; } = "ProjectArchitecture.Model";
            public Group_Default Default { get; } = new Group_Default(); // Group: Default
            // Group: Default
            public class Group_Default : GroupArchNode {
                public override string Name { get; } = "Default";
                public TypeArchNode SourceGenerator { get; } = typeof(SourceGenerator); // Type: SourceGenerator
            }
        }
    }
```

# The tips to make your project design better

 - Separate low-level code from hight-level code. In more complex projects you can separate extra, unimportant, utility code. Do it on every level (low/hight-level modules, low/hight-level classes, low/hight-level functions). For example: Presentation, Application, Extensions, Domain, Infrastructure, Internal.
 - Put all your code into folders reflecting the namespaces containing those code.
 - Put your low-level code into existing namespaces: System, Microsoft, MyProject, ThirdPartyProject instead of special namespaces: MyProject.Internal, MyProject.Helpers.
 - Understand the semantic of your types. I can distinguish the following semantic categories:
    * `Attribute` - ideally only data
    * `Service` - only logic without any data or state
    * `Entity` - data and state
    * `Component` - in component-oriented programming entities can consist of components
    * `Utility, helper`
    * `Object, data structure`
- Understand the semantic of your type's members. [I can distinguish the following semantic categories](https://softwareengineering.stackexchange.com/a/404752/352915):
    * `Property, attribute`: Name, Background, Color, Data, Content, Value, Children, Parent
    * `Query, question`: IsInitialized, IsRunning, HasValue, CanRun, DoesEqual(value), Equals(value), AreEqual(v1, v2), GetValue()
    * `Event`: OnChange, OnChanged
    * `Directive`: IgnoreXml, RunOnLoad, CloseWhenError
    * `Command`: Initialize(), Run(), Stop(), SetValue(value)
    * `Event handler`: OnChange(value), OnChanged(value)

# The links

 - [ProjectArchitecture](https://github.com/Denis535/ProjectArchitecture)
 - [MakeTypesPublic](https://github.com/Denis535/MakeTypesPublic)