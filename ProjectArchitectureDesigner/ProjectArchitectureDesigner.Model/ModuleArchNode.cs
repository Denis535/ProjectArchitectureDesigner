// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    public class ModuleArchNode : ArchNode {

        public virtual Assembly? Assembly { get; }
        public virtual string Name { get; }
        public ProjectArchNode Project { get; protected set; }
        public NamespaceArchNode[] Namespaces { get; protected init; }


        protected ModuleArchNode() {
            Assembly = null!;
            Name = null!;
            Project = null!;
            Namespaces = null!;
        }
        public ModuleArchNode(Assembly? assembly, string name, NamespaceArchNode[] namespaces) {
            Assembly = assembly;
            Name = name;
            Project = null!;
            Namespaces = NamespaceArchNode.SetModule( namespaces, this );
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
        internal static ModuleArchNode[] SetProject(ModuleArchNode[] modules, ProjectArchNode project) {
            foreach (var module in modules) module.Project = project;
            return modules;
        }


    }
}
