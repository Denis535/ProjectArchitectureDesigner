// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public abstract class NamespaceArchNode : ArchNode {

        public bool IsGlobal => Name is (null or "" or "Global");
        // Ancestors
        public ProjectArchNode Project => Module.Project;
        public ModuleArchNode Module { get; }
        // Descendant
        public ArchNode[] DescendantNodes => GetDescendantNodes( this ).ToArray();
        public ArchNode[] DescendantNodesAndSelf => GetDescendantNodes( this ).Prepend( this ).ToArray();
        public abstract GroupArchNode[] Groups { get; }
        public TypeArchNode[] Types => Groups.SelectMany( i => i.Types ).ToArray();


        public NamespaceArchNode(ModuleArchNode module) {
            Module = module;
        }


        // Utils
        public override string ToString() {
            return "Namespace: " + Name;
        }


    }
}
