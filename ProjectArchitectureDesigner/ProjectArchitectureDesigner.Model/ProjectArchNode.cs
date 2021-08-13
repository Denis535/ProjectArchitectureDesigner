// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ProjectArchNode : ArchNode {

        public virtual string Name { get; }
        public ModuleArchNode[] Modules { get; protected init; }


        protected ProjectArchNode() {
            Name = null!;
            Modules = null!;
        }
        public ProjectArchNode(string name, ModuleArchNode[] modules) {
            Name = name;
            Modules = ModuleArchNode.SetProject( modules, this );
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
