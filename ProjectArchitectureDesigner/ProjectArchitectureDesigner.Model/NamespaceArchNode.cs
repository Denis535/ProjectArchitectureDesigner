// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class NamespaceArchNode : ArchNode {

        public virtual string Name { get; }
        public ModuleArchNode Module { get; protected set; }
        public GroupArchNode[] Groups { get; protected init; }


        protected NamespaceArchNode() {
            Name = null!;
            Module = null!;
            Groups = null!;
        }
        public NamespaceArchNode(string name, GroupArchNode[] groups) {
            Name = name;
            Module = null!;
            Groups = GroupArchNode.SetNamespace( groups, this );
        }


        // Utils
        public override string ToString() {
            return "Namespace: " + Name;
        }
        internal static NamespaceArchNode[] SetModule(NamespaceArchNode[] namespaces, ModuleArchNode module) {
            foreach (var @namespace in namespaces) @namespace.Module = module;
            return namespaces;
        }


    }
}
