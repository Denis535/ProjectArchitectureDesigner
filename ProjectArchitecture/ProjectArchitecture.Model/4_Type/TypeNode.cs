// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class TypeNode : Node {

        public override string Name => Type.Name;
        public Type Type { get; }


        public TypeNode(Type type) {
            Type = type;
        }


        // Utils
        public override string ToString() {
            return "Type: " + Name;
        }


        // Conversions
        public static implicit operator TypeNode(Type type) => new TypeNode( type );


    }
}
