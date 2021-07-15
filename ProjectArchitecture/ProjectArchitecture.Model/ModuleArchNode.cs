// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public abstract class ModuleArchNode : ArchNode {

        public override string Name => GetName( this );
        // Parent
        public ProjectArchNode Project { get; internal set; } = default!;
        // Children
        public virtual NamespaceArchNode[] Namespaces => GetChildren<NamespaceArchNode>( this ).ToArray();
        public ArchNode[] DescendantNodes => GetDescendantNodes( this ).ToArray();
        public ArchNode[] DescendantNodesAndSelf => GetDescendantNodes( this ).Prepend( this ).ToArray();


        public ModuleArchNode() {
            foreach (var @namespace in Namespaces) @namespace.Module = this;
        }


        // Initialize
        protected abstract void Initialize(); // Used by source generator
        protected virtual void SetChildren(params object[] children) { // Used by source generator
        }


        // Utils
        public override string ToString() {
            return "Module: " + Name;
        }


    }
}
