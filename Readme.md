# The overview

The **ProjectArchitectureDesigner** package allows you to describe the complex hierarchical structure of your project with a very, very simple code.
Thanks to the Roslyn source generator we can now generate code of any complexity based on your very simple code.

It will give you:
 - The readable and comprehensive view of project.
 - The ability to render the project structure into any convenient format.
 - The ability to validate dependencies between groups of types (third-party library is needed).

# The api overview

```
Project: ——— ProjectArchitectureDesigner
Module: ———— ProjectArchitectureDesigner
Namespace: — ProjectArchitectureDesigner.Model
Group: ————— ArchNode
Type: —————— ArchNode
Group: ————— ArchNode/Children
Type: —————— ProjectArchNode
Type: —————— ModuleArchNode
Type: —————— NamespaceArchNode
Type: —————— GroupArchNode
Type: —————— TypeArchNode
Namespace: — ProjectArchitectureDesigner.Renderers
Type: —————— ProjectAlignedTextRenderer
Type: —————— ProjectHierarchicalTextRenderer
Type: —————— ProjectMarkdownRenderer
```

# The getting started

In order to describe your project you just need to write the `ProjectArchNode` and the `ModuleArcNode` classes with the list of modules, namespaces, groups and types entries.

# The examples

## The original source

```csharp
// Project/ProjectArchitecture
public sealed partial class Project_ProjectArchitectureDesigner : ProjectArchNode {
    protected override void Initialize() => SetChildren(
        typeof( Module_ProjectArchitectureDesigner ),
        typeof( Module_ProjectArchitectureDesigner_Analyzer ),
        typeof( Module_ProjectArchitectureDesigner_Internal )
    );
    protected override bool IsSupported(Type type) {
        return
            base.IsSupported( type ) &&
            !type.IsNestedPrivate &&
            type.Namespace != "System.Runtime.CompilerServices" &&
            type.Namespace != "System.Diagnostics.CodeAnalysis";
    }
}

// Modules/ProjectArchitectureDesigner
public sealed partial class Module_ProjectArchitectureDesigner : ModuleArchNode {
    public override System.Reflection.Assembly? Assembly => typeof( ArchNode ).Assembly;
    protected override void Initialize() => SetChildren(
        "ProjectArchitecture.Model",
        /// Group: ArchNode
        typeof( ArchNode ),
        /// Group: ArchNode/Children
        typeof( ProjectArchNode ),
        typeof( ModuleArchNode ),
        typeof( NamespaceArchNode ),
        typeof( GroupArchNode ),
        typeof( TypeArchNode ),
        "ProjectArchitecture.Renderers",
        typeof( ProjectAlignedTextRenderer ),
        typeof( ProjectHierarchicalTextRenderer ),
        typeof( ProjectMarkdownRenderer )
    );
}

// Modules/ProjectArchitectureDesigner.Analyzer
public sealed partial class Module_ProjectArchitectureDesigner_Analyzer : ModuleArchNode {
    public override System.Reflection.Assembly? Assembly => typeof( SourceGenerator ).Assembly;
    protected override void Initialize() => SetChildren(
        "ProjectArchitecture.Model",
        typeof( SourceGenerator ),
        typeof( SyntaxAnalyzer ),
        typeof( SyntaxGenerator ),
        /// Group: ClassInfo
        typeof( ProjectInfo ),
        typeof( ModuleInfo ),
        /// Group: ClassInfo/Entries
        typeof( ModuleEntry ),
        typeof( NamespaceEntry ),
        typeof( GroupEntry ),
        typeof( TypeEntry ),
        "Microsoft.CodeAnalysis.CSharp.Syntax",
        typeof( SyntaxFormatter ),
        typeof( SyntaxFormatterRewriter ),
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
```

## The generated source

```csharp
// Project: ProjectArchitectureDesigner
public sealed partial class Project_ProjectArchitectureDesigner : ProjectArchNode {
    public override string Name => "ProjectArchitectureDesigner";
    public override ModuleArchNode[] Modules => new ModuleArchNode[] { ProjectArchitectureDesigner, ProjectArchitectureDesigner_Analyzer, ProjectArchitectureDesigner_Internal };
    public Module_ProjectArchitectureDesigner ProjectArchitectureDesigner { get; } = new Module_ProjectArchitectureDesigner();
    public Module_ProjectArchitectureDesigner_Analyzer ProjectArchitectureDesigner_Analyzer { get; } = new Module_ProjectArchitectureDesigner_Analyzer();
    public Module_ProjectArchitectureDesigner_Internal ProjectArchitectureDesigner_Internal { get; } = new Module_ProjectArchitectureDesigner_Internal();
}

// Module: ProjectArchitectureDesigner
public sealed partial class Module_ProjectArchitectureDesigner : ModuleArchNode {
    public override string Name => "ProjectArchitectureDesigner";
    public override NamespaceArchNode[] Namespaces => new NamespaceArchNode[] { ProjectArchitecture_Model, ProjectArchitecture_Renderers };
    public Namespace_ProjectArchitecture_Model ProjectArchitecture_Model { get; } = new Namespace_ProjectArchitecture_Model();
    public Namespace_ProjectArchitecture_Renderers ProjectArchitecture_Renderers { get; } = new Namespace_ProjectArchitecture_Renderers();
    // Namespace: ProjectArchitecture.Model
    public class Namespace_ProjectArchitecture_Model : NamespaceArchNode {
        public override string Name => "ProjectArchitecture.Model";
        public override GroupArchNode[] Groups => new GroupArchNode[] { ArchNode, ArchNode_Children };
        public Group_ArchNode ArchNode { get; } = new Group_ArchNode();
        public Group_ArchNode_Children ArchNode_Children { get; } = new Group_ArchNode_Children();
        public class Group_ArchNode : GroupArchNode {
            public override string Name => "ArchNode";
            public override TypeArchNode[] Types { get; } = new TypeArchNode[] { typeof(ArchNode) };
        }
        public class Group_ArchNode_Children : GroupArchNode {
            public override string Name => "ArchNode/Children";
            public override TypeArchNode[] Types { get; } = new TypeArchNode[] { typeof(ProjectArchNode), typeof(ModuleArchNode), typeof(NamespaceArchNode), typeof(GroupArchNode), typeof(TypeArchNode) };
        }
    }
    // Namespace: ProjectArchitecture.Renderers
    public class Namespace_ProjectArchitecture_Renderers : NamespaceArchNode {
        public override string Name => "ProjectArchitecture.Renderers";
        public override GroupArchNode[] Groups => new GroupArchNode[] { Default };
        public Group_Default Default { get; } = new Group_Default();
        public class Group_Default : GroupArchNode {
            public override string Name => "Default";
            public override TypeArchNode[] Types { get; } = new TypeArchNode[] { typeof(ProjectAlignedTextRenderer), typeof(ProjectHierarchicalTextRenderer), typeof(ProjectMarkdownRenderer) };
        }
    }
}

// Module: ProjectArchitectureDesigner.Analyzer
public sealed partial class Module_ProjectArchitectureDesigner_Analyzer : ModuleArchNode {
    public override string Name => "ProjectArchitectureDesigner.Analyzer";
    public override NamespaceArchNode[] Namespaces => new NamespaceArchNode[] { ProjectArchitecture_Model, Microsoft_CodeAnalysis_CSharp_Syntax };
    public Namespace_ProjectArchitecture_Model ProjectArchitecture_Model { get; } = new Namespace_ProjectArchitecture_Model();
    public Namespace_Microsoft_CodeAnalysis_CSharp_Syntax Microsoft_CodeAnalysis_CSharp_Syntax { get; } = new Namespace_Microsoft_CodeAnalysis_CSharp_Syntax();
    // Namespace: ProjectArchitecture.Model
    public class Namespace_ProjectArchitecture_Model : NamespaceArchNode {
        public override string Name => "ProjectArchitecture.Model";
        public override GroupArchNode[] Groups => new GroupArchNode[] { Default, ClassInfo, ClassInfo_Entries };
        public Group_Default Default { get; } = new Group_Default();
        public Group_ClassInfo ClassInfo { get; } = new Group_ClassInfo();
        public Group_ClassInfo_Entries ClassInfo_Entries { get; } = new Group_ClassInfo_Entries();
        public class Group_Default : GroupArchNode {
            public override string Name => "Default";
            public override TypeArchNode[] Types { get; } = new TypeArchNode[] { typeof(SourceGenerator), typeof(SyntaxAnalyzer), typeof(SyntaxGenerator) };
        }
        public class Group_ClassInfo : GroupArchNode {
            public override string Name => "ClassInfo";
            public override TypeArchNode[] Types { get; } = new TypeArchNode[] { typeof(ProjectInfo), typeof(ModuleInfo) };
        }
        public class Group_ClassInfo_Entries : GroupArchNode {
            public override string Name => "ClassInfo/Entries";
            public override TypeArchNode[] Types { get; } = new TypeArchNode[] { typeof(ModuleEntry), typeof(NamespaceEntry), typeof(GroupEntry), typeof(TypeEntry) };
        }
    }
    // Namespace: Microsoft.CodeAnalysis.CSharp.Syntax
    public class Namespace_Microsoft_CodeAnalysis_CSharp_Syntax : NamespaceArchNode {
        public override string Name => "Microsoft.CodeAnalysis.CSharp.Syntax";
        public override GroupArchNode[] Groups => new GroupArchNode[] { Default };
        public Group_Default Default { get; } = new Group_Default();
        public class Group_Default : GroupArchNode {
            public override string Name => "Default";
            public override TypeArchNode[] Types { get; } = new TypeArchNode[] { typeof(SyntaxFormatter), typeof(SyntaxFormatterRewriter), typeof(SyntaxFactory2), typeof(SyntaxUtils) };
        }
    }
}

// Module: ProjectArchitectureDesigner.Internal
public sealed partial class Module_ProjectArchitectureDesigner_Internal : ModuleArchNode {
    public override string Name => "ProjectArchitectureDesigner.Internal";
    public override NamespaceArchNode[] Namespaces => new NamespaceArchNode[] { System, System_Collections_Generic, System_Text, System_Text_CSharp, System_Text_Markdown };
    public Namespace_System System { get; } = new Namespace_System();
    public Namespace_System_Collections_Generic System_Collections_Generic { get; } = new Namespace_System_Collections_Generic();
    public Namespace_System_Text System_Text { get; } = new Namespace_System_Text();
    public Namespace_System_Text_CSharp System_Text_CSharp { get; } = new Namespace_System_Text_CSharp();
    public Namespace_System_Text_Markdown System_Text_Markdown { get; } = new Namespace_System_Text_Markdown();
    // Namespace: System
    public class Namespace_System : NamespaceArchNode {
        public override string Name => "System";
        public override GroupArchNode[] Groups => new GroupArchNode[] { Default };
        public Group_Default Default { get; } = new Group_Default();
        public class Group_Default : GroupArchNode {
            public override string Name => "Default";
            public override TypeArchNode[] Types { get; } = new TypeArchNode[] { typeof(Option), typeof(Option<>), typeof(CSharpExtensions), typeof(StringExtensions), typeof(TypeExtensions) };
        }
    }
    // Namespace: System.Collections.Generic
    public class Namespace_System_Collections_Generic : NamespaceArchNode {
        public override string Name => "System.Collections.Generic";
        public override GroupArchNode[] Groups => new GroupArchNode[] { Default };
        public Group_Default Default { get; } = new Group_Default();
        public class Group_Default : GroupArchNode {
            public override string Name => "Default";
            public override TypeArchNode[] Types { get; } = new TypeArchNode[] { typeof(EnumerableExtensions), typeof(PeekableEnumeratorExtensions), typeof(PeekableEnumerator<>) };
        }
    }
    // Namespace: System.Text
    public class Namespace_System_Text : NamespaceArchNode {
        public override string Name => "System.Text";
        public override GroupArchNode[] Groups => new GroupArchNode[] { Default };
        public Group_Default Default { get; } = new Group_Default();
        public class Group_Default : GroupArchNode {
            public override string Name => "Default";
            public override TypeArchNode[] Types { get; } = new TypeArchNode[] { typeof(StringBuilderExtensions), typeof(HierarchicalStringBuilder) };
        }
    }
    // Namespace: System.Text.CSharp
    public class Namespace_System_Text_CSharp : NamespaceArchNode {
        public override string Name => "System.Text.CSharp";
        public override GroupArchNode[] Groups => new GroupArchNode[] { Default };
        public Group_Default Default { get; } = new Group_Default();
        public class Group_Default : GroupArchNode {
            public override string Name => "Default";
            public override TypeArchNode[] Types { get; } = new TypeArchNode[] { typeof(CSharpSyntaxFactory), typeof(CSharpSyntaxFactoryHelper), typeof(CSharpSyntaxFactoryHelper2), typeof(CSharpSyntaxFactoryHelper3), typeof(AccessLevelExtensions), typeof(AccessLevel) };
        }
    }
    // Namespace: System.Text.Markdown
    public class Namespace_System_Text_Markdown : NamespaceArchNode {
        public override string Name => "System.Text.Markdown";
        public override GroupArchNode[] Groups => new GroupArchNode[] { Default };
        public Group_Default Default { get; } = new Group_Default();
        public class Group_Default : GroupArchNode {
            public override string Name => "Default";
            public override TypeArchNode[] Types { get; } = new TypeArchNode[] { typeof(MarkdownBuilder), typeof(MarkdownSyntaxFactory) };
        }
    }
}
```

# The best practices and tips to make your code design better

 - Separate low-level code from hight-level code. In more complex projects you can separate extra, unimportant, utility code. Do it on every level (low/hight-level modules, low/hight-level classes, low/hight-level functions). For example: Presentation, Application, Extensions, Domain, Infrastructure, Internal.
 - Put all your code into folders reflecting the namespaces containing those code.
 - Put your low-level code into existing namespaces: System, Microsoft, MyProject, ThirdPartyProject instead of special namespaces: MyProject.Internal, MyProject.Helpers.
 - Understand the semantic of your types. I can distinguish the following semantic categories:
    * `Attribute` - Set of constants.
    * `Service` - Logic.
    * `Entity` - Data, state and logic.
    * `Utility` - Set of useful methods.
    * `Object` - Low-level data structure.
- Understand the semantic of your type's members. [I can distinguish the following semantic categories](https://softwareengineering.stackexchange.com/a/404752/352915):
    * `Property` - Name, Background, Color, Data, Content, Value, Children, Parent.
    * `Query` (`question`) - IsInitialized, IsRunning, HasValue, CanRun, DoesEqual(value), Equals(value), AreEqual(v1, v2), GetValue().
    * `Directive` - IgnoreXml, RunOnLoad, CloseWhenError.
    * `Event` - OnChange, OnChanged.
    * `Command` - Initialize(), Run(), Stop(), SetValue(value).
    * `Handler` - OnChange(value), OnChanged(value).
    * `Utility` - Sqrt(value), Cos(value), Sin(value), ToString(), GetHashCode().

# The links

 - [ProjectArchitecture](https://github.com/Denis535/ProjectArchitecture)
 - [MakeTypesPublic](https://github.com/Denis535/MakeTypesPublic)