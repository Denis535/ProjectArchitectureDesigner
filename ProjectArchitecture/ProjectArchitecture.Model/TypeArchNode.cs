// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;
    
    public class TypeArchNode : ArchNode {

        public override string Name => Value.Name;
        public Type Value { get; }


        public TypeArchNode(Type value) {
            Value = value;
        }


        // Utils
        public override string ToString() {
            return "Type: " + Name;
        }


        // Conversions
        public static implicit operator TypeArchNode(Type value) => new TypeArchNode( value );
        public static implicit operator Type(TypeArchNode type) => type.Value;


    }
}
