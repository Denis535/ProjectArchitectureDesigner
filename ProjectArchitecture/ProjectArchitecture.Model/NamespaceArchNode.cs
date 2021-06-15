// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public abstract class NamespaceArchNode : ArchNode {

        public override string Name => GetName( this );
        public bool IsGlobal => Name is (null or "" or "Global");
        // Parent
        public ModuleArchNode Module { get; internal set; } = default!;
        // Children
        public virtual GroupArchNode[] Groups => GetChildren<GroupArchNode>( this ).ToArray();
        public ArchNode[] DescendantNodes => GetDescendantNodes( this ).ToArray();
        public ArchNode[] DescendantNodesAndSelf => GetDescendantNodes( this ).Prepend( this ).ToArray();


        public NamespaceArchNode() {
            foreach (var group in Groups) group.Namespace = this;
        }


        // Utils
        public override string ToString() {
            return "Namespace: " + Name;
        }


    }
}
