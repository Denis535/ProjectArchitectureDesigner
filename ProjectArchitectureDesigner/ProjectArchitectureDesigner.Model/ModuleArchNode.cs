// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public abstract class ModuleArchNode : ArchNode {

        public virtual Assembly? Assembly { get; }
        // Parent
        public ProjectArchNode Project { get; internal set; } = default!;
        // Children
        public abstract NamespaceArchNode[] Namespaces { get; }
        public GroupArchNode[] Groups => Namespaces.SelectMany( i => i.Groups ).ToArray();
        public TypeArchNode[] Types => Namespaces.SelectMany( i => i.Groups ).SelectMany( i => i.Types ).ToArray();
        public ArchNode[] DescendantNodes => GetDescendantNodes( this ).ToArray();
        public ArchNode[] DescendantNodesAndSelf => GetDescendantNodes( this ).Prepend( this ).ToArray();


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
