// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ProjectArchNode : ArchNode {

        private readonly ModuleArchNode[] modules = default!;
        public virtual string Name { get; protected init; } = default!;
        public ModuleArchNode[] Modules { get => modules; protected init => modules = ModuleArchNode.WithProject( value, this ); }


        protected ProjectArchNode() {
        }
        public ProjectArchNode(string name, ModuleArchNode[] modules) {
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
