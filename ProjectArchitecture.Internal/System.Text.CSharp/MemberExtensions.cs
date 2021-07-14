// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace System.Text.CSharp {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    internal static class MemberExtensions {


        // GetKeywords
        public static IEnumerable<string> GetKeywords(this FieldInfo field) {
            yield return field.GetAccessModifier();
            if (field.IsLiteral) yield return "const";
            if (field.IsStatic && !field.IsLiteral) yield return "static";
            if (field.IsInitOnly) yield return "readonly";
        }
        public static IEnumerable<string> GetKeywords(this PropertyInfo property) {
            yield return property.GetAccessModifier();
            if (property.GetAccessors( true ).IsStatic()) yield return "static";
            if (property.GetAccessors( true ).IsAbstract()) yield return "abstract";
            if (property.GetAccessors( true ).IsVirtual()) yield return "virtual";
            if (property.GetAccessors( true ).IsOverride()) yield return "override";
            if (property.GetAccessors( true ).IsSealed()) yield return "sealed";
        }
        public static IEnumerable<string> GetKeywords(this EventInfo @event) {
            yield return @event.GetAccessModifier();
            if (@event.GetAccessors( true ).IsStatic()) yield return "static";
            if (@event.GetAccessors( true ).IsAbstract()) yield return "abstract";
            if (@event.GetAccessors( true ).IsVirtual()) yield return "virtual";
            if (@event.GetAccessors( true ).IsOverride()) yield return "override";
            if (@event.GetAccessors( true ).IsSealed()) yield return "sealed";
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


        // GetAccessModifier
        public static string GetAccessModifier(this FieldInfo field) {
            if (field.IsPublic) return "public";
            if (field.IsAssembly) return "internal";
            if (field.IsFamily) return "protected";
            if (field.IsFamilyOrAssembly) return "protected internal";
            if (field.IsFamilyAndAssembly) return "private protected";
            if (field.IsPrivate) return "private";
            throw new Exception( "Access modifier is unknown: " + field );
        }
        public static string GetAccessModifier(this PropertyInfo property) {
            var result = GetAccessModifier( property.GetMethod, property.SetMethod );
            return result ?? throw new Exception( "Access modifier is unknown: " + property );
        }
        public static string GetAccessModifier(this EventInfo @event) {
            var result = GetAccessModifier( @event.AddMethod, @event.RemoveMethod, @event.RaiseMethod );
            return result ?? throw new Exception( "Access modifier is unknown: " + @event );
        }
        public static string GetAccessModifier(this MethodBase method) {
            if (method.IsPublic) return "public";
            if (method.IsAssembly) return "internal";
            if (method.IsFamily) return "protected";
            if (method.IsFamilyOrAssembly) return "protected internal";
            if (method.IsFamilyAndAssembly) return "private protected";
            if (method.IsPrivate) return "private";
            throw new Exception( "Access modifier is unknown: " + method );
        }


        // IsKeyword/MethodInfo
        public static bool IsStatic(this IEnumerable<MethodInfo> methods) {
            return methods.Any( IsStatic );
        }
        public static bool IsAbstract(this IEnumerable<MethodInfo> methods) {
            return methods.Any( IsAbstract );
        }
        public static bool IsVirtual(this IEnumerable<MethodInfo> methods) {
            return methods.Any( IsVirtual );
        }
        public static bool IsOverride(this IEnumerable<MethodInfo> methods) {
            return methods.Any( IsOverride );
        }
        public static bool IsSealed(this IEnumerable<MethodInfo> methods) {
            return methods.Any( IsSealed );
        }
        // IsKeyword/MethodInfo
        public static bool IsStatic(this MethodInfo method) {
            return method.IsStatic;
        }
        public static bool IsAbstract(this MethodInfo method) {
            return method.IsAbstract;
        }
        public static bool IsVirtual(this MethodInfo method) {
            return method.IsVirtual && !method.IsAbstract && method.DeclaringType == method.GetBaseDefinition().DeclaringType;
        }
        public static bool IsOverride(this MethodInfo method) {
            return method.IsVirtual && !method.IsAbstract && method.DeclaringType != method.GetBaseDefinition().DeclaringType;
        }
        public static bool IsSealed(this MethodInfo method) {
            return method.IsFinal;
        }
        // IsType
        public static bool IsIndexer(this PropertyInfo property) {
            return property.Name == "Item" && property.GetIndexParameters().Length > 0;
        }
        public static bool IsOperator(MethodInfo method) {
            return method.IsPublic && method.IsStatic && method.IsSpecialName && method.Name.StartsWith( "op_" );
        }
        // IsSpecialName
        public static bool IsSpecialName(this MemberInfo member) {
            if (member is FieldInfo field) return field.IsSpecialName;
            if (member is PropertyInfo prop) return prop.IsSpecialName;
            if (member is EventInfo @event) return @event.IsSpecialName;
            if (member is MethodBase method) return method.IsSpecialName;
            return false;
        }


        // Helpers/GetAccessModifier
        private static string? GetAccessModifier(MethodInfo? method1, MethodInfo? method2) {
            if (method1?.IsPublic == true ||
                method2?.IsPublic == true) return "public";

            if (method1?.IsAssembly == true ||
                method2?.IsAssembly == true) return "internal";

            if (method1?.IsFamily == true ||
                method2?.IsFamily == true) return "protected";

            if (method1?.IsFamilyOrAssembly == true ||
                method2?.IsFamilyOrAssembly == true) return "protected internal";

            if (method1?.IsFamilyAndAssembly == true ||
                method2?.IsFamilyAndAssembly == true) return "private protected";

            if (method1?.IsPrivate == true ||
                method2?.IsPrivate == true) return "private";
            return null;
        }
        private static string? GetAccessModifier(MethodInfo? method1, MethodInfo? method2, MethodInfo? method3) {
            if (method1?.IsPublic == true ||
                method2?.IsPublic == true ||
                method3?.IsPublic == true) return "public";

            if (method1?.IsAssembly == true ||
                method2?.IsAssembly == true ||
                method3?.IsAssembly == true) return "internal";

            if (method1?.IsFamily == true ||
                method2?.IsFamily == true ||
                method3?.IsFamily == true) return "protected";

            if (method1?.IsFamilyOrAssembly == true ||
                method2?.IsFamilyOrAssembly == true ||
                method3?.IsFamilyOrAssembly == true) return "protected internal";

            if (method1?.IsFamilyAndAssembly == true ||
                method2?.IsFamilyAndAssembly == true ||
                method3?.IsFamilyAndAssembly == true) return "private protected";

            if (method1?.IsPrivate == true ||
                method2?.IsPrivate == true ||
                method3?.IsPrivate == true) return "private";
            return null;
        }
        // Helpers/GetAccessors
        private static IEnumerable<MethodInfo> GetAccessors(this EventInfo @event, bool nonPublic) {
            if (@event.GetAddMethod( nonPublic ) != null) yield return @event.GetAddMethod( nonPublic );
            if (@event.GetRemoveMethod( nonPublic ) != null) yield return @event.GetRemoveMethod( nonPublic );
            if (@event.GetRaiseMethod( nonPublic ) != null) yield return @event.GetRaiseMethod( nonPublic );
        }


    }
}
