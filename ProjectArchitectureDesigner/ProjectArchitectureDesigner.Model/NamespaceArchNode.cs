// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public abstract class NamespaceArchNode : ArchNode {

        public bool IsGlobal => Name is (null or "" or "Global");
        // Parent
        public ModuleArchNode Module { get; }
        // Children
        public abstract GroupArchNode[] Groups { get; }


        public NamespaceArchNode(ModuleArchNode module) {
            Module = module;
        }


        // Utils
        public override string ToString() {
            return "Namespace: " + Name;
        }


    }
}
