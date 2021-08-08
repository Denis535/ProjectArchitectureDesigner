// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public abstract class GroupArchNode : ArchNode {

        public bool IsDefault => Name is (null or "" or "Default");
        // Parent
        public NamespaceArchNode Namespace { get; }
        // Children
        public abstract TypeArchNode[] Types { get; }


        public GroupArchNode(NamespaceArchNode @namespace) {
            Namespace = @namespace;
        }


        // Utils
        public override string ToString() {
            return "Group: " + Name;
        }


    }
}
