// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace System.Text.CSharp {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    internal static class TypeSyntaxUtils {


        // IsType
        public static bool IsInterface(this Type type) {
            return type.IsInterface;
        }
        public static bool IsClass(this Type type) {
            return type.IsClass && !type.IsSubclassOf( typeof( Delegate ) );
        }
        public static bool IsStruct(this Type type) {
            return type.IsValueType && !type.IsEnum;
        }
        public static bool IsEnum(this Type type) {
            return type.IsEnum;
        }
        public static bool IsDelegate(this Type type) {
            return type.IsClass && type.IsSubclassOf( typeof( Delegate ) );
        }
        // GetKeywords
        public static IEnumerable<string> GetKeywords(this Type type) {
            if (type.IsGenericParameter) {
                if (type.IsContravariant()) yield return "in";
                if (type.IsCovariant()) yield return "out";
            } else {
                yield return type.GetAccessModifier();

                if (type.IsClass()) {
                    if (type.IsStatic()) yield return "static";
                    if (type.IsAbstract()) yield return "abstract";
                    if (type.IsSealed()) yield return "sealed";
                }

                if (type.IsInterface()) yield return "interface";
                if (type.IsClass()) yield return "class";
                if (type.IsStruct()) yield return "struct";
                if (type.IsEnum()) yield return "enum";
                if (type.IsDelegate()) yield return "delegate";
            }
        }
        // GetConstraints
        public static IEnumerable<string> GetConstraints(this Type type) {
            if (type.HasReferenceTypeConstraint()) {
                yield return "class";
            }
            if (type.HasValueTypeConstraint()) {
                yield return "struct";
            }
            foreach (var constraint in type.GetGenericParameterConstraints()) {
                if (constraint != typeof( ValueType )) {
                    yield return constraint.GetIdentifier();
                }
            }
            if (type.HasDefaultConstructorConstraint() && !type.HasValueTypeConstraint()) {
                yield return "new()";
            }
        }


        // Helpers/GetAccessModifier
        private static string GetAccessModifier(this Type type) {
            if (!type.IsNested) {
                return type.IsPublic ? "public" : "internal";
            } else {
                if (type.IsNestedPublic) return "public";
                if (type.IsNestedAssembly) return "internal";
                if (type.IsNestedFamily) return "protected";
                if (type.IsNestedFamORAssem) return "protected internal"; // protected or internal
                if (type.IsNestedFamANDAssem) return "private protected"; // protected but only internal
                if (type.IsNestedPrivate) return "private";
            }
            throw new Exception( "Access modifier is unknown: " + type );
        }
        // Helpers/IsKeyword
        private static bool IsStatic(this Type type) {
            return type.IsAbstract && type.IsSealed;
        }
        private static bool IsAbstract(this Type type) {
            return type.IsAbstract && !type.IsSealed;
        }
        private static bool IsSealed(this Type type) {
            return type.IsSealed && !type.IsAbstract;
        }
        private static bool IsContravariant(this Type type) { // in
            return type.GenericParameterAttributes.HasFlag( GenericParameterAttributes.Contravariant );
        }
        private static bool IsCovariant(this Type type) { // out
            return type.GenericParameterAttributes.HasFlag( GenericParameterAttributes.Covariant );
        }
        // Helpers/HasConstraint
        private static bool HasReferenceTypeConstraint(this Type type) {
            return type.GenericParameterAttributes.HasFlag( GenericParameterAttributes.ReferenceTypeConstraint );
        }
        private static bool HasValueTypeConstraint(this Type type) {
            return type.GenericParameterAttributes.HasFlag( GenericParameterAttributes.NotNullableValueTypeConstraint );
        }
        private static bool HasDefaultConstructorConstraint(this Type type) {
            return type.GenericParameterAttributes.HasFlag( GenericParameterAttributes.DefaultConstructorConstraint );
        }


    }
}