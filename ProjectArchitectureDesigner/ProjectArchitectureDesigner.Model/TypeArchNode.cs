// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.CSharp;

    public class TypeArchNode : ArchNode {

        public Type Value { get; }
        public string Name => Value.GetIdentifier();
        public GroupArchNode Group { get; private set; }
        // TypeInfo
        public TypeInfo TypeInfo => Value.GetTypeInfo();
        public IEnumerable<MemberInfo> DeclaredMembers => Value.GetTypeInfo().DeclaredMembers.Where( IsUserDefined );
        public IEnumerable<TypeInfo> DeclaredNestedTypes => Value.GetTypeInfo().DeclaredNestedTypes.Where( IsUserDefined );
        public IEnumerable<FieldInfo> DeclaredFields => Value.GetTypeInfo().DeclaredFields.Where( IsUserDefined );
        public IEnumerable<PropertyInfo> DeclaredProperties => Value.GetTypeInfo().DeclaredProperties.Where( IsUserDefined );
        public IEnumerable<EventInfo> DeclaredEvents => Value.GetTypeInfo().DeclaredEvents.Where( IsUserDefined );
        public IEnumerable<ConstructorInfo> DeclaredConstructors => Value.GetTypeInfo().DeclaredConstructors.Where( IsUserDefined );
        public IEnumerable<MethodInfo> DeclaredMethods => Value.GetTypeInfo().DeclaredMethods.Where( IsUserDefined );


        public TypeArchNode(Type value) {
            Value = value;
            Group = null!;
        }
        public TypeArchNode(Type value, GroupArchNode group) {
            Value = value;
            Group = group;
        }


        // Utils
        public override string ToString() {
            return "Type: " + Name;
        }
        internal static TypeArchNode[] SetGroup(TypeArchNode[] types, GroupArchNode group) {
            foreach (var type in types) type.Group = group;
            return types;
        }


        // Helpers
        private static bool IsUserDefined(MemberInfo member) {
            if (member is FieldInfo field && field.IsSpecialName) return false;
            if (member is PropertyInfo prop && prop.IsSpecialName) return false;
            if (member is EventInfo @event && @event.IsSpecialName) return false;
            if (member is MethodBase method && method.IsSpecialName) return false;
            if (member.IsDefined( typeof( GeneratedCodeAttribute ) )) return false;
            if (member.Name.StartsWith( "<" )) return false;
            return true;
        }


    }
}
