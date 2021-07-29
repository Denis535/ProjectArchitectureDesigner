// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public abstract class GroupArchNode : ArchNode {

        public bool IsDefault => Name is (null or "" or "Default");
        // Ancestors
        public ProjectArchNode Project => Namespace.Module.Project;
        public ModuleArchNode Module => Namespace.Module;
        public NamespaceArchNode Namespace { get; internal set; } = default!;
        // Children
        public abstract TypeArchNode[] Types { get; }
        public ArchNode[] DescendantNodes => GetDescendantNodes( this ).ToArray();
        public ArchNode[] DescendantNodesAndSelf => GetDescendantNodes( this ).Prepend( this ).ToArray();


        // Utils
        public override string ToString() {
            return "Group: " + Name;
        }


    }
}
