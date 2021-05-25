// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class TypeNode : ArchitectureNode {

        public override string Name => Value.Name;
        public Type Value { get; }


        public TypeNode(Type value) {
            Value = value;
        }


        // Utils
        public override string ToString() {
            return "Type: " + Name;
        }


        // Conversions
        public static implicit operator TypeNode(Type type) => new TypeNode( type );
        public static implicit operator Type(TypeNode type) => type.Value;


    }
}
