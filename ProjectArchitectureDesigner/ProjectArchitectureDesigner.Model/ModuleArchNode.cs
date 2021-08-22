// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    public class ModuleArchNode : ArchNode {

        private readonly NamespaceArchNode[] Namespaces_BackingField = default!;
        public ProjectArchNode Project { get; private set; } = default!;
        public virtual Assembly? Assembly { get; init; } = default!;
        public virtual string Name { get; init; } = default!;
        public NamespaceArchNode[] Namespaces { get => Namespaces_BackingField; init => Namespaces_BackingField = NamespaceArchNode.WithModule( value, this ); }


        public ModuleArchNode() {
        }
        public ModuleArchNode(Assembly? assembly, string name, params NamespaceArchNode[] namespaces) {
            Assembly = assembly;
            Name = name;
            Namespaces = namespaces;
        }


        // Initialize
        protected virtual void Initialize() { // Used by source generator
        }
        protected virtual void SetChildren(params object[] children) { // Used by source generator
        }


        // Utils
        public override string ToString() {
            return "Module: " + Name;
        }
        internal static ModuleArchNode[] WithProject(ModuleArchNode[] modules, ProjectArchNode project) {
            foreach (var module in modules) module.Project = project;
            return modules;
        }


    }
}
