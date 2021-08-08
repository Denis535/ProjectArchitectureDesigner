// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public abstract class ModuleArchNode : ArchNode {

        public virtual Assembly? Assembly { get; }
        // Parent
        public ProjectArchNode Project { get; }
        // Children
        public abstract NamespaceArchNode[] Namespaces { get; }
        // Descendant
        public IEnumerable<GroupArchNode> Groups => Namespaces.SelectMany( i => i.Groups ).ToArray();
        public IEnumerable<TypeArchNode> Types => Namespaces.SelectMany( i => i.Groups ).SelectMany( i => i.Types ).ToArray();


        public ModuleArchNode(ProjectArchNode project) {
            Project = project;
        }


        // Initialize
        protected abstract void Initialize(); // Used by source generator
        protected virtual void SetChildren(params object[] children) { // Used by source generator
        }


        // Utils
        public override string ToString() {
            return "Module: " + Name;
        }


    }
}
