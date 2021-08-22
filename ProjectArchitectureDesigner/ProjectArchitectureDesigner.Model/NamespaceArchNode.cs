// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class NamespaceArchNode : ArchNode {

        private readonly GroupArchNode[] Groups_BackingField = default!;
        public ModuleArchNode Module { get; private set; } = default!;
        public virtual string Name { get; init; } = default!;
        public GroupArchNode[] Groups { get => Groups_BackingField; init => Groups_BackingField = GroupArchNode.WithNamespace( value, this ); }


        public NamespaceArchNode() {
        }
        public NamespaceArchNode(string name, params GroupArchNode[] groups) {
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
