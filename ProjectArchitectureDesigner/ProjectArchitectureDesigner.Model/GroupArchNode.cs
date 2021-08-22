// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class GroupArchNode : ArchNode {

        private readonly TypeArchNode[] Types_BackingField = default!;
        public NamespaceArchNode Namespace { get; private set; } = default!;
        public virtual string Name { get; init; } = default!;
        public TypeArchNode[] Types { get => Types_BackingField; init => Types_BackingField = TypeArchNode.WithGroup( value, this ); }


        public GroupArchNode() {
        }
        public GroupArchNode(string name, params TypeArchNode[] types) {
            Name = name;
            Types = types;
        }


        // Utils
        public override string ToString() {
            return "Group: " + Name;
        }
        internal static GroupArchNode[] WithNamespace(GroupArchNode[] groups, NamespaceArchNode @namespace) {
            foreach (var group in groups) group.Namespace = @namespace;
            return groups;
        }


    }
}
