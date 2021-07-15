// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace System.Text.CSharp {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    internal static class MemberSyntaxUtils {


        // IsType
        public static bool IsIndexer(this PropertyInfo property) {
            return property.Name == "Item" && property.GetIndexParameters().Length > 0;
        }
        public static bool IsOperator(MethodInfo method) {
            return method.IsPublic && method.IsStatic && method.IsSpecialName && method.Name.StartsWith( "op_" );
        }
        // GetKeywords
        public static IEnumerable<string> GetKeywords(this FieldInfo field) {
            yield return field.GetAccessModifier();
            if (field.IsLiteral) yield return "const";
            if (field.IsStatic && !field.IsLiteral) yield return "static";
            if (field.IsInitOnly) yield return "readonly";
        }
        public static IEnumerable<string> GetKeywords(this PropertyInfo property) {
            yield return property.GetAccessModifier();
            if (property.GetAccessors( true ).Any( IsStatic )) yield return "static";
            if (property.GetAccessors( true ).Any( IsAbstract )) yield return "abstract";
            if (property.GetAccessors( true ).Any( IsVirtual )) yield return "virtual";
            if (property.GetAccessors( true ).Any( IsOverride )) yield return "override";
            if (property.GetAccessors( true ).Any( IsSealed )) yield return "sealed";
        }
        public static IEnumerable<string> GetKeywords(this EventInfo @event) {
            yield return @event.GetAccessModifier();
            if (@event.GetAccessors( true ).Any( IsStatic )) yield return "static";
            if (@event.GetAccessors( true ).Any( IsAbstract )) yield return "abstract";
            if (@event.GetAccessors( true ).Any( IsVirtual )) yield return "virtual";
            if (@event.GetAccessors( true ).Any( IsOverride )) yield return "override";
            if (@event.GetAccessors( true ).Any( IsSealed )) yield return "sealed";
        }
        public static IEnumerable<string> GetKeywords(this ConstructorInfo constructor) {
            yield return constructor.GetAccessModifier();
            if (constructor.IsStatic) yield return "static";
        }
        public static IEnumerable<string> GetKeywords(this MethodInfo method) {
            yield return method.GetAccessModifier();
            if (method.IsStatic()) yield return "static";
            if (method.IsAbstract()) yield return "abstract";
            if (method.IsVirtual()) yield return "virtual";
            if (method.IsOverride()) yield return "override";
            if (method.IsSealed()) yield return "sealed";
        }


        // Helpers/GetAccessModifier
        private static string GetAccessModifier(this FieldInfo field) {
            if (field.IsPublic) return "public";
            if (field.IsAssembly) return "internal";
            if (field.IsFamily) return "protected";
            if (field.IsFamilyOrAssembly) return "protected internal";
            if (field.IsFamilyAndAssembly) return "private protected";
            if (field.IsPrivate) return "private";
            throw new Exception( "Access modifier is unknown: " + field );
        }
        private static string GetAccessModifier(this PropertyInfo property) {
            if (property.GetAccessors( true ).Any( i => i.IsPublic )) return "public";
            if (property.GetAccessors( true ).Any( i => i.IsAssembly )) return "internal";
            if (property.GetAccessors( true ).Any( i => i.IsFamily )) return "protected";
            if (property.GetAccessors( true ).Any( i => i.IsFamilyOrAssembly )) return "protected internal";
            if (property.GetAccessors( true ).Any( i => i.IsFamilyAndAssembly )) return "private protected";
            if (property.GetAccessors( true ).Any( i => i.IsPrivate )) return "private";
            throw new Exception( "Access modifier is unknown: " + property );
        }
        private static string GetAccessModifier(this EventInfo @event) {
            if (@event.GetAccessors( true ).Any( i => i.IsPublic )) return "public";
            if (@event.GetAccessors( true ).Any( i => i.IsAssembly )) return "internal";
            if (@event.GetAccessors( true ).Any( i => i.IsFamily )) return "protected";
            if (@event.GetAccessors( true ).Any( i => i.IsFamilyOrAssembly )) return "protected internal";
            if (@event.GetAccessors( true ).Any( i => i.IsFamilyAndAssembly )) return "private protected";
            if (@event.GetAccessors( true ).Any( i => i.IsPrivate )) return "private";
            throw new Exception( "Access modifier is unknown: " + @event );
        }
        internal static string GetAccessModifier(this MethodBase method) {
            if (method.IsPublic) return "public";
            if (method.IsAssembly) return "internal";
            if (method.IsFamily) return "protected";
            if (method.IsFamilyOrAssembly) return "protected internal";
            if (method.IsFamilyAndAssembly) return "private protected";
            if (method.IsPrivate) return "private";
            throw new Exception( "Access modifier is unknown: " + method );
        }
        // Helpers/IsKeyword
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
