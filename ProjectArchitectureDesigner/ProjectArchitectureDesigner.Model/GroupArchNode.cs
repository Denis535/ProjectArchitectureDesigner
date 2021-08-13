// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class GroupArchNode : ArchNode {

        public virtual string Name { get; }
        public NamespaceArchNode Namespace { get; protected set; }
        public TypeArchNode[] Types { get; protected init; }


        protected GroupArchNode() {
            Name = null!;
            Namespace = null!;
            Types = null!;
        }
        public GroupArchNode(string name, TypeArchNode[] types) {
            Name = name;
            Namespace = null!;
            Types = TypeArchNode.SetGroup( types, this );
        }


        // Utils
        public override string ToString() {
            return "Group: " + Name;
        }
        internal static GroupArchNode[] SetNamespace(GroupArchNode[] groups, NamespaceArchNode @namespace) {
            foreach (var group in groups) group.Namespace = @namespace;
            return groups;
        }


    }
}
