# The overview

The **ProjectArchitectureDesigner** package allows you to describe the complex hierarchical structure of your project with a very, very simple code.
Thanks to the Roslyn source generator we can now generate code of any complexity based on your very simple code.

It will give you:
 - The readable and comprehensive view of project.
 - The ability to render the project structure into any convenient format.
 - The ability to validate dependencies between groups of types (third-party library is needed).

# The api overview

- **Project: ProjectArchitectureDesigner**
- | - **Module: ProjectArchitectureDesigner**
-     | - **Namespace: ProjectArchitectureDesigner.Model**
-     |   | - **ArchNode**
-     |   |   ArchNode
-     |   | - **ArchNode/Children**
-     |   |   ProjectArchNode
-     |   |   ModuleArchNode
-     |   |   NamespaceArchNode
-     |   |   GroupArchNode
-     |   |   TypeArchNode
-     | - **Namespace: ProjectArchitectureDesigner.Model.Renderers**
-         | - **ProjectRenderer**
-         |   ProjectRenderer
-         |   TextProjectRenderer
-         |   HierarchicalTextProjectRenderer
-         |   MarkdownDocumentProjectRenderer
-         | - **NodeRenderer**
-         |   INodeRenderer
-         |   DelegateNodeRenderer
-         |   TextNodeRenderer
-         |   LeftAlignedTextNodeRenderer
-         |   RightAlignedTextNodeRenderer
-         |   MarkdownHighlighter

# The getting started

In order to describe your project you just need to write the `ProjectArchNode` and the `ModuleArcNode` classes with the list of modules, namespaces, groups and types entries.

# The examples

## The original source code

```csharp
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
        typeof( SyntaxFormatterHelper3 ),
        typeof( SyntaxTemplateProcessor ),
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

## The generated source code

```csharp
// Project
public sealed partial class Project_ProjectArchitectureDesigner {
    /// Properties
    public override string Name { get; } = "ProjectArchitectureDesigner";
    public override ModuleArchNode[] Modules { get; }
    public Module_ProjectArchitectureDesigner ProjectArchitectureDesigner { get; }
    public Module_ProjectArchitectureDesigner_Analyzer ProjectArchitectureDesigner_Analyzer { get; }
    public Module_ProjectArchitectureDesigner_Internal ProjectArchitectureDesigner_Internal { get; }
    /// Constructor
    public Project_ProjectArchitectureDesigner() {
        this.Modules = new ModuleArchNode[] {
            this.ProjectArchitectureDesigner = new Module_ProjectArchitectureDesigner( this ),
            this.ProjectArchitectureDesigner_Analyzer = new Module_ProjectArchitectureDesigner_Analyzer( this ),
            this.ProjectArchitectureDesigner_Internal = new Module_ProjectArchitectureDesigner_Internal( this ),
        };
    }
}

// Module
public sealed partial class Module_ProjectArchitectureDesigner {
    /// Properties
    public override string Name { get; } = "ProjectArchitectureDesigner";
    public override NamespaceArchNode[] Namespaces { get; }
    public Namespace_ProjectArchitectureDesigner_Model ProjectArchitectureDesigner_Model { get; }
    public Namespace_ProjectArchitectureDesigner_Model_Renderers ProjectArchitectureDesigner_Model_Renderers { get; }
    /// Constructor
    public Module_ProjectArchitectureDesigner(ProjectArchNode project) : base( project ) {
        this.Namespaces = new NamespaceArchNode[] {
            this.ProjectArchitectureDesigner_Model = new Namespace_ProjectArchitectureDesigner_Model( this ),
            this.ProjectArchitectureDesigner_Model_Renderers = new Namespace_ProjectArchitectureDesigner_Model_Renderers( this ),
        };
    }
    // Namespace
    public class Namespace_ProjectArchitectureDesigner_Model : NamespaceArchNode {
        /// Properties
        public override string Name { get; } = "ProjectArchitectureDesigner.Model";
        public override GroupArchNode[] Groups { get; }
        public Group_ArchNode ArchNode { get; }
        public Group_ArchNode_Children ArchNode_Children { get; }
        /// Constructor
        public Namespace_ProjectArchitectureDesigner_Model(ModuleArchNode module) : base( module ) {
            this.Groups = new GroupArchNode[] {
                this.ArchNode = new Group_ArchNode( this ),
                this.ArchNode_Children = new Group_ArchNode_Children( this ),
            };
        }
        // Group
        public class Group_ArchNode : GroupArchNode {
            /// Properties
            public override string Name { get; } = "ArchNode";
            public override TypeArchNode[] Types { get; }
            /// Constructor
            public Group_ArchNode(NamespaceArchNode @namespace) : base( @namespace ) {
                this.Types = new TypeArchNode[] {
                    new TypeArchNode( typeof(ArchNode), this ),
                };
            }
        }
        // Group
        public class Group_ArchNode_Children : GroupArchNode {
            /// Properties
            public override string Name { get; } = "ArchNode/Children";
            public override TypeArchNode[] Types { get; }
            /// Constructor
            public Group_ArchNode_Children(NamespaceArchNode @namespace) : base( @namespace ) {
                this.Types = new TypeArchNode[] {
                    new TypeArchNode( typeof(ProjectArchNode), this ),
                    new TypeArchNode( typeof(ModuleArchNode), this ),
                    new TypeArchNode( typeof(NamespaceArchNode), this ),
                    new TypeArchNode( typeof(GroupArchNode), this ),
                    new TypeArchNode( typeof(TypeArchNode), this ),
                };
            }
        }
    }
    // Namespace
    public class Namespace_ProjectArchitectureDesigner_Model_Renderers : NamespaceArchNode {
        /// Properties
        public override string Name { get; } = "ProjectArchitectureDesigner.Model.Renderers";
        public override GroupArchNode[] Groups { get; }
        public Group_ProjectRenderer ProjectRenderer { get; }
        public Group_NodeRenderer NodeRenderer { get; }
        /// Constructor
        public Namespace_ProjectArchitectureDesigner_Model_Renderers(ModuleArchNode module) : base( module ) {
            this.Groups = new GroupArchNode[] {
                this.ProjectRenderer = new Group_ProjectRenderer( this ),
                this.NodeRenderer = new Group_NodeRenderer( this ),
            };
        }
        // Group
        public class Group_ProjectRenderer : GroupArchNode {
            /// Properties
            public override string Name { get; } = "ProjectRenderer";
            public override TypeArchNode[] Types { get; }
            /// Constructor
            public Group_ProjectRenderer(NamespaceArchNode @namespace) : base( @namespace ) {
                this.Types = new TypeArchNode[] {
                    new TypeArchNode( typeof(ProjectRenderer), this ),
                    new TypeArchNode( typeof(TextProjectRenderer), this ),
                    new TypeArchNode( typeof(HierarchicalTextProjectRenderer), this ),
                    new TypeArchNode( typeof(MarkdownDocumentProjectRenderer), this ),
                };
            }
        }
        // Group
        public class Group_NodeRenderer : GroupArchNode {
            /// Properties
            public override string Name { get; } = "NodeRenderer";
            public override TypeArchNode[] Types { get; }
            /// Constructor
            public Group_NodeRenderer(NamespaceArchNode @namespace) : base( @namespace ) {
                this.Types = new TypeArchNode[] {
                    new TypeArchNode( typeof(INodeRenderer), this ),
                    new TypeArchNode( typeof(DelegateNodeRenderer), this ),
                    new TypeArchNode( typeof(TextNodeRenderer), this ),
                    new TypeArchNode( typeof(LeftAlignedTextNodeRenderer), this ),
                    new TypeArchNode( typeof(RightAlignedTextNodeRenderer), this ),
                };
            }
        }
    }
}

// Module
public sealed partial class Module_ProjectArchitectureDesigner_Analyzer {
    /// Properties
    public override string Name { get; } = "ProjectArchitectureDesigner.Analyzer";
    public override NamespaceArchNode[] Namespaces { get; }
    public Namespace_ProjectArchitectureDesigner_Model ProjectArchitectureDesigner_Model { get; }
    public Namespace_Microsoft_CodeAnalysis_CSharp_Syntax Microsoft_CodeAnalysis_CSharp_Syntax { get; }
    /// Constructor
    public Module_ProjectArchitectureDesigner_Analyzer(ProjectArchNode project) : base( project ) {
        this.Namespaces = new NamespaceArchNode[] {
            this.ProjectArchitectureDesigner_Model = new Namespace_ProjectArchitectureDesigner_Model( this ),
            this.Microsoft_CodeAnalysis_CSharp_Syntax = new Namespace_Microsoft_CodeAnalysis_CSharp_Syntax( this ),
        };
    }
    // Namespace
    public class Namespace_ProjectArchitectureDesigner_Model : NamespaceArchNode {
        /// Properties
        public override string Name { get; } = "ProjectArchitectureDesigner.Model";
        public override GroupArchNode[] Groups { get; }
        public Group_Default Default { get; }
        public Group_ClassInfo ClassInfo { get; }
        public Group_ClassInfo_Entry ClassInfo_Entry { get; }
        /// Constructor
        public Namespace_ProjectArchitectureDesigner_Model(ModuleArchNode module) : base( module ) {
            this.Groups = new GroupArchNode[] {
                this.Default = new Group_Default( this ),
                this.ClassInfo = new Group_ClassInfo( this ),
                this.ClassInfo_Entry = new Group_ClassInfo_Entry( this ),
            };
        }
        // Group
        public class Group_Default : GroupArchNode {
            /// Properties
            public override string Name { get; } = "Default";
            public override TypeArchNode[] Types { get; }
            /// Constructor
            public Group_Default(NamespaceArchNode @namespace) : base( @namespace ) {
                this.Types = new TypeArchNode[] {
                    new TypeArchNode( typeof(SourceGenerator), this ),
                    new TypeArchNode( typeof(SyntaxAnalyzer), this ),
                    new TypeArchNode( typeof(SyntaxGenerator), this ),
                };
            }
        }
        // Group
        public class Group_ClassInfo : GroupArchNode {
            /// Properties
            public override string Name { get; } = "ClassInfo";
            public override TypeArchNode[] Types { get; }
            /// Constructor
            public Group_ClassInfo(NamespaceArchNode @namespace) : base( @namespace ) {
                this.Types = new TypeArchNode[] {
                    new TypeArchNode( typeof(ProjectInfo), this ),
                    new TypeArchNode( typeof(ModuleInfo), this ),
                };
            }
        }
        // Group
        public class Group_ClassInfo_Entry : GroupArchNode {
            /// Properties
            public override string Name { get; } = "ClassInfo/Entry";
            public override TypeArchNode[] Types { get; }
            /// Constructor
            public Group_ClassInfo_Entry(NamespaceArchNode @namespace) : base( @namespace ) {
                this.Types = new TypeArchNode[] {
                    new TypeArchNode( typeof(ModuleEntry), this ),
                    new TypeArchNode( typeof(NamespaceEntry), this ),
                    new TypeArchNode( typeof(GroupEntry), this ),
                    new TypeArchNode( typeof(TypeEntry), this ),
                };
            }
        }
    }
    // Namespace
    public class Namespace_Microsoft_CodeAnalysis_CSharp_Syntax : NamespaceArchNode {
        /// Properties
        public override string Name { get; } = "Microsoft.CodeAnalysis.CSharp.Syntax";
        public override GroupArchNode[] Groups { get; }
        public Group_Default Default { get; }
        /// Constructor
        public Namespace_Microsoft_CodeAnalysis_CSharp_Syntax(ModuleArchNode module) : base( module ) {
            this.Groups = new GroupArchNode[] {
                this.Default = new Group_Default( this ),
            };
        }
        // Group
        public class Group_Default : GroupArchNode {
            /// Properties
            public override string Name { get; } = "Default";
            public override TypeArchNode[] Types { get; }
            /// Constructor
            public Group_Default(NamespaceArchNode @namespace) : base( @namespace ) {
                this.Types = new TypeArchNode[] {
                    new TypeArchNode( typeof(SyntaxFormatter), this ),
                    new TypeArchNode( typeof(SyntaxFormatterHelper), this ),
                    new TypeArchNode( typeof(SyntaxFormatterHelper2), this ),
                    new TypeArchNode( typeof(SyntaxFormatterHelper3), this ),
                    new TypeArchNode( typeof(SyntaxTemplateProcessor), this ),
                    new TypeArchNode( typeof(SyntaxFactory2), this ),
                    new TypeArchNode( typeof(SyntaxUtils), this ),
                };
            }
        }
    }
}

// Module
public sealed partial class Module_ProjectArchitectureDesigner_Internal {
    /// Properties
    public override string Name { get; } = "ProjectArchitectureDesigner.Internal";
    public override NamespaceArchNode[] Namespaces { get; }
    public Namespace_System System { get; }
    public Namespace_System_Collections_Generic System_Collections_Generic { get; }
    public Namespace_System_Text System_Text { get; }
    public Namespace_System_Text_CSharp System_Text_CSharp { get; }
    public Namespace_System_Text_Markdown System_Text_Markdown { get; }
    /// Constructor
    public Module_ProjectArchitectureDesigner_Internal(ProjectArchNode project) : base( project ) {
        this.Namespaces = new NamespaceArchNode[] {
            this.System = new Namespace_System( this ),
            this.System_Collections_Generic = new Namespace_System_Collections_Generic( this ),
            this.System_Text = new Namespace_System_Text( this ),
            this.System_Text_CSharp = new Namespace_System_Text_CSharp( this ),
            this.System_Text_Markdown = new Namespace_System_Text_Markdown( this ),
        };
    }
    // Namespace
    public class Namespace_System : NamespaceArchNode {
        /// Properties
        public override string Name { get; } = "System";
        public override GroupArchNode[] Groups { get; }
        public Group_Default Default { get; }
        /// Constructor
        public Namespace_System(ModuleArchNode module) : base( module ) {
            this.Groups = new GroupArchNode[] {
                this.Default = new Group_Default( this ),
            };
        }
        // Group
        public class Group_Default : GroupArchNode {
            /// Properties
            public override string Name { get; } = "Default";
            public override TypeArchNode[] Types { get; }
            /// Constructor
            public Group_Default(NamespaceArchNode @namespace) : base( @namespace ) {
                this.Types = new TypeArchNode[] {
                    new TypeArchNode( typeof(Option), this ),
                    new TypeArchNode( typeof(Option<>), this ),
                    new TypeArchNode( typeof(CSharpExtensions), this ),
                    new TypeArchNode( typeof(StringExtensions), this ),
                    new TypeArchNode( typeof(TypeExtensions), this ),
                };
            }
        }
    }
    // Namespace
    public class Namespace_System_Collections_Generic : NamespaceArchNode {
        /// Properties
        public override string Name { get; } = "System.Collections.Generic";
        public override GroupArchNode[] Groups { get; }
        public Group_Default Default { get; }
        /// Constructor
        public Namespace_System_Collections_Generic(ModuleArchNode module) : base( module ) {
            this.Groups = new GroupArchNode[] {
                this.Default = new Group_Default( this ),
            };
        }
        // Group
        public class Group_Default : GroupArchNode {
            /// Properties
            public override string Name { get; } = "Default";
            public override TypeArchNode[] Types { get; }
            /// Constructor
            public Group_Default(NamespaceArchNode @namespace) : base( @namespace ) {
                this.Types = new TypeArchNode[] {
                    new TypeArchNode( typeof(EnumerableExtensions), this ),
                    new TypeArchNode( typeof(PeekableEnumeratorExtensions), this ),
                    new TypeArchNode( typeof(PeekableEnumerator<>), this ),
                };
            }
        }
    }
    // Namespace
    public class Namespace_System_Text : NamespaceArchNode {
        /// Properties
        public override string Name { get; } = "System.Text";
        public override GroupArchNode[] Groups { get; }
        public Group_Default Default { get; }
        /// Constructor
        public Namespace_System_Text(ModuleArchNode module) : base( module ) {
            this.Groups = new GroupArchNode[] {
                this.Default = new Group_Default( this ),
            };
        }
        // Group
        public class Group_Default : GroupArchNode {
            /// Properties
            public override string Name { get; } = "Default";
            public override TypeArchNode[] Types { get; }
            /// Constructor
            public Group_Default(NamespaceArchNode @namespace) : base( @namespace ) {
                this.Types = new TypeArchNode[] {
                    new TypeArchNode( typeof(StringBuilderExtensions), this ),
                    new TypeArchNode( typeof(HierarchicalStringBuilder), this ),
                };
            }
        }
    }
    // Namespace
    public class Namespace_System_Text_CSharp : NamespaceArchNode {
        /// Properties
        public override string Name { get; } = "System.Text.CSharp";
        public override GroupArchNode[] Groups { get; }
        public Group_Default Default { get; }
        /// Constructor
        public Namespace_System_Text_CSharp(ModuleArchNode module) : base( module ) {
            this.Groups = new GroupArchNode[] {
                this.Default = new Group_Default( this ),
            };
        }
        // Group
        public class Group_Default : GroupArchNode {
            /// Properties
            public override string Name { get; } = "Default";
            public override TypeArchNode[] Types { get; }
            /// Constructor
            public Group_Default(NamespaceArchNode @namespace) : base( @namespace ) {
                this.Types = new TypeArchNode[] {
                    new TypeArchNode( typeof(CSharpSyntaxFactory), this ),
                    new TypeArchNode( typeof(CSharpSyntaxFactoryHelper), this ),
                    new TypeArchNode( typeof(CSharpSyntaxFactoryHelper2), this ),
                    new TypeArchNode( typeof(CSharpSyntaxFactoryHelper3), this ),
                    new TypeArchNode( typeof(AccessLevelExtensions), this ),
                    new TypeArchNode( typeof(AccessLevel), this ),
                };
            }
        }
    }
    // Namespace
    public class Namespace_System_Text_Markdown : NamespaceArchNode {
        /// Properties
        public override string Name { get; } = "System.Text.Markdown";
        public override GroupArchNode[] Groups { get; }
        public Group_Default Default { get; }
        /// Constructor
        public Namespace_System_Text_Markdown(ModuleArchNode module) : base( module ) {
            this.Groups = new GroupArchNode[] {
                this.Default = new Group_Default( this ),
            };
        }
        // Group
        public class Group_Default : GroupArchNode {
            /// Properties
            public override string Name { get; } = "Default";
            public override TypeArchNode[] Types { get; }
            /// Constructor
            public Group_Default(NamespaceArchNode @namespace) : base( @namespace ) {
                this.Types = new TypeArchNode[] {
                    new TypeArchNode( typeof(MarkdownBuilder), this ),
                    new TypeArchNode( typeof(MarkdownSyntaxFactory), this ),
                };
            }
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