// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;

    public static class TypeExtensions {


        // IsInState
        public static bool IsObsolete(this Type? type) {
            while (type != null) {
                if (type.IsDefined( typeof( ObsoleteAttribute ) )) return true;
                type = type.DeclaringType;
            }
            return false;
        }
        public static bool IsCompilerGenerated(this Type? type) {
            while (type != null) {
                if (type.IsDefined( typeof( CompilerGeneratedAttribute ) )) return true;
                type = type.DeclaringType;
            }
            return false;
        }


        // GetUnboundType
        public static Type GetUnboundType(this Type type) {
            if (type.IsGenericType) {
                if (type.IsGenericTypeDefinition) return type;
                return type.GetGenericTypeDefinition();
            } else {
                return type;
            }
        }


    }
}