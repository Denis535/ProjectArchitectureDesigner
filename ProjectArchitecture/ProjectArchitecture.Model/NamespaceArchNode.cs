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
        public GroupArchNode[] Groups => GetChildren<GroupArchNode>( this ).ToArray();


        // Utils
        public override string ToString() {
            return "Namespace: " + Name;
        }


    }
}
