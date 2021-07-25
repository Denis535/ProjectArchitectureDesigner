// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.CSharp;

    public class TypeArchNode : ArchNode {

        public Type Value { get; }
        public override string Name => Value.GetIdentifier();
        // Parent
        public ProjectArchNode Project => Group.Namespace.Module.Project;
        public ModuleArchNode Module => Group.Namespace.Module;
        public NamespaceArchNode Namespace => Group.Namespace;
        public GroupArchNode Group { get; internal set; } = default!;


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