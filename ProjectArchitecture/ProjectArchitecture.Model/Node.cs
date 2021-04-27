// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public abstract class Node {

        public abstract string Name { get; }


        // Utils
        public override string ToString() {
            return "Node: " + Name;
        }


        // Conversion
        public static implicit operator Node(string group) => new Group( group );
        public static implicit operator Node(Type type) => new TypeItem( type );


    }
}
