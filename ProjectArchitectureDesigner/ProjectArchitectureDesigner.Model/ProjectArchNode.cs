// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    public class ProjectArchNode : ArchNode {

        private readonly ModuleArchNode[] Modules_BackingField = default!;
        public virtual Assembly[]? Assemblies { get; init; }
        public virtual string Name { get; init; } = default!;
        public ModuleArchNode[] Modules { get => Modules_BackingField; init => Modules_BackingField = ModuleArchNode.WithProject( value, this ); }


        public ProjectArchNode() {
        }
        public ProjectArchNode(Assembly[]? assemblies, string name, params ModuleArchNode[] modules) {
            Assemblies = assemblies;
            Name = name;
            Modules = modules;
        }


        // Initialize
        protected virtual void Initialize() { // Used by source generator
        }
        protected virtual void SetChildren(params Type[] children) { // Used by source generator
        }


        // IsSupported
        public virtual bool IsSupported(Type type) {
            return !type.IsObsolete() && !type.IsCompilerGenerated();
        }
        // IsVisible
        public virtual bool IsVisible(TypeArchNode type) {
            return type.Value.IsVisible;
        }


        // Utils
        public override string ToString() {
            return "Project: " + Name;
        }


    }
}
