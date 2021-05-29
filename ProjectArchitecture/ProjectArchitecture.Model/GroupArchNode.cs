// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public abstract class GroupArchNode : ArchNode {

        public override string Name => GetName( this );
        public bool IsDefault => Name is (null or "" or "Default");
        public virtual TypeArchNode[] Types => GetChildren<TypeArchNode>( this ).ToArray();
        public ArchNode[] DescendantNodes => GetDescendantNodes( this ).ToArray();
        public ArchNode[] DescendantNodesAndSelf => GetDescendantNodes( this ).Prepend( this ).ToArray();


        // Utils
        public override string ToString() {
            return "Group: " + Name;
        }


    }
}
