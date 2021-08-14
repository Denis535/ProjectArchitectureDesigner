// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class NamespaceArchNode : ArchNode {

        private readonly GroupArchNode[] groups = default!;
        public virtual string Name { get; protected init; } = default!;
        public ModuleArchNode Module { get; private set; } = default!;
        public GroupArchNode[] Groups { get => groups; protected init => groups = GroupArchNode.WithNamespace( value, this ); }


        protected NamespaceArchNode() {
        }
        public NamespaceArchNode(string name, GroupArchNode[] groups) {
            Name = name;
            Groups = groups;
        }


        // Utils
        public override string ToString() {
            return "Namespace: " + Name;
        }
        internal static NamespaceArchNode[] WithModule(NamespaceArchNode[] namespaces, ModuleArchNode module) {
            foreach (var @namespace in namespaces) @namespace.Module = module;
            return namespaces;
        }


    }
}
