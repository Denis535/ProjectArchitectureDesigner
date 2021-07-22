// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace System.Text.CSharp {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    internal static class CSharpSyntaxHelper_Member {


        // GetModifiers
        public static IEnumerable<string> GetModifiers(this FieldInfo field) {
            yield return field.GetAccessLevel().GetModifier();
            if (field.IsLiteral) yield return "const";
            if (field.IsStatic && !field.IsLiteral) yield return "static";
            if (field.IsInitOnly) yield return "readonly";
        }
        public static IEnumerable<string> GetModifiers(this PropertyInfo property) {
            yield return property.GetAccessLevel().GetModifier();
            if (property.GetAccessors( true ).Any( IsStatic )) yield return "static";
            if (property.GetAccessors( true ).Any( IsAbstract )) yield return "abstract";
            if (property.GetAccessors( true ).Any( IsVirtual )) yield return "virtual";
            if (property.GetAccessors( true ).Any( IsOverride )) yield return "override";
            if (property.GetAccessors( true ).Any( IsSealed )) yield return "sealed";
        }
        public static IEnumerable<string> GetModifiers(this EventInfo @event) {
            yield return @event.GetAccessLevel().GetModifier();
            if (@event.GetAccessors( true ).Any( IsStatic )) yield return "static";
            if (@event.GetAccessors( true ).Any( IsAbstract )) yield return "abstract";
            if (@event.GetAccessors( true ).Any( IsVirtual )) yield return "virtual";
            if (@event.GetAccessors( true ).Any( IsOverride )) yield return "override";
            if (@event.GetAccessors( true ).Any( IsSealed )) yield return "sealed";
        }
        public static IEnumerable<string> GetModifiers(this ConstructorInfo constructor) {
            yield return constructor.GetAccessLevel().GetModifier();
            if (constructor.IsStatic) yield return "static";
        }
        public static IEnumerable<string> GetModifiers(this MethodInfo method) {
            yield return method.GetAccessLevel().GetModifier();
            if (method.IsStatic()) yield return "static";
            if (method.IsAbstract()) yield return "abstract";
            if (method.IsVirtual()) yield return "virtual";
            if (method.IsOverride()) yield return "override";
            if (method.IsSealed()) yield return "sealed";
        }


        // Helpers/GetAccessLevel
        private static AccessLevel GetAccessLevel(this FieldInfo field) {
            if (field.IsPublic) return AccessLevel.Public;
            if (field.IsAssembly) return AccessLevel.Internal;
            if (field.IsFamily) return AccessLevel.Protected;
            if (field.IsFamilyOrAssembly) return AccessLevel.ProtectedInternal;
            if (field.IsFamilyAndAssembly) return AccessLevel.PrivateProtected;
            if (field.IsPrivate) return AccessLevel.Private;
            throw new Exception( "Access level is unknown: " + field );
        }
        private static AccessLevel GetAccessLevel(this PropertyInfo property) {
            if (property.GetAccessors( true ).Any( i => i.IsPublic )) return AccessLevel.Public;
            if (property.GetAccessors( true ).Any( i => i.IsAssembly )) return AccessLevel.Internal;
            if (property.GetAccessors( true ).Any( i => i.IsFamily )) return AccessLevel.Protected;
            if (property.GetAccessors( true ).Any( i => i.IsFamilyOrAssembly )) return AccessLevel.ProtectedInternal;
            if (property.GetAccessors( true ).Any( i => i.IsFamilyAndAssembly )) return AccessLevel.PrivateProtected;
            if (property.GetAccessors( true ).Any( i => i.IsPrivate )) return AccessLevel.Private;
            throw new Exception( "Access level is unknown: " + property );
        }
        private static AccessLevel GetAccessLevel(this EventInfo @event) {
            if (@event.GetAccessors( true ).Any( i => i.IsPublic )) return AccessLevel.Public;
            if (@event.GetAccessors( true ).Any( i => i.IsAssembly )) return AccessLevel.Internal;
            if (@event.GetAccessors( true ).Any( i => i.IsFamily )) return AccessLevel.Protected;
            if (@event.GetAccessors( true ).Any( i => i.IsFamilyOrAssembly )) return AccessLevel.ProtectedInternal;
            if (@event.GetAccessors( true ).Any( i => i.IsFamilyAndAssembly )) return AccessLevel.PrivateProtected;
            if (@event.GetAccessors( true ).Any( i => i.IsPrivate )) return AccessLevel.Private;
            throw new Exception( "Access level is unknown: " + @event );
        }
        internal static AccessLevel GetAccessLevel(this MethodBase method) {
            if (method.IsPublic) return AccessLevel.Public;
            if (method.IsAssembly) return AccessLevel.Internal;
            if (method.IsFamily) return AccessLevel.Protected;
            if (method.IsFamilyOrAssembly) return AccessLevel.ProtectedInternal;
            if (method.IsFamilyAndAssembly) return AccessLevel.PrivateProtected;
            if (method.IsPrivate) return AccessLevel.Private;
            throw new Exception( "Access level is unknown: " + method );
        }
        // Helpers/IsModifier
        private static bool IsStatic(this MethodInfo method) {
            return method.IsStatic;
        }
        private static bool IsAbstract(this MethodInfo method) {
            return method.IsAbstract;
        }
        private static bool IsVirtual(this MethodInfo method) {
            return method.IsVirtual && !method.IsAbstract && method.DeclaringType == method.GetBaseDefinition().DeclaringType;
        }
        private static bool IsOverride(this MethodInfo method) {
            return method.IsVirtual && !method.IsAbstract && method.DeclaringType != method.GetBaseDefinition().DeclaringType;
        }
        private static bool IsSealed(this MethodInfo method) {
            return method.IsFinal;
        }
        // Helpers/GetAccessors
        private static IEnumerable<MethodInfo> GetAccessors(this EventInfo @event, bool nonPublic) {
            if (@event.GetAddMethod( nonPublic ) != null) yield return @event.GetAddMethod( nonPublic );
            if (@event.GetRemoveMethod( nonPublic ) != null) yield return @event.GetRemoveMethod( nonPublic );
            if (@event.GetRaiseMethod( nonPublic ) != null) yield return @event.GetRaiseMethod( nonPublic );
        }


    }
}
