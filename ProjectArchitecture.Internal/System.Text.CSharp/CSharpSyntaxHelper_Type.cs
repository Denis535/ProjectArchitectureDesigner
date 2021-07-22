// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace System.Text.CSharp {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    internal enum AccessLevel {
        Public,
        Internal,
        Protected,
        ProtectedInternal,
        PrivateProtected,
        Private,
    }
    internal static class CSharpSyntaxHelper_Type {


        // GetModifiers
        public static IEnumerable<string> GetModifiers(this Type type) {
            if (type.IsGenericParameter) {
                if (type.IsContravariant()) yield return "in";
                if (type.IsCovariant()) yield return "out";
            } else {
                yield return type.GetAccessLevel().GetModifier();

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
        public static string GetModifier(this AccessLevel level) {
            return level switch {
                AccessLevel.Public => "public",
                AccessLevel.Internal => "internal",
                AccessLevel.Protected => "protected",
                AccessLevel.ProtectedInternal => "protected internal",
                AccessLevel.PrivateProtected => "private protected",
                AccessLevel.Private => "private",
                _ => throw new ArgumentException( "Access level is invalid: " + level ),
            };
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


        // Helpers/GetAccessLevel
        private static AccessLevel GetAccessLevel(this Type type) {
            if (!type.IsNested) {
                return type.IsPublic ? AccessLevel.Public : AccessLevel.Internal;
            } else {
                if (type.IsNestedPublic) return AccessLevel.Public;
                if (type.IsNestedAssembly) return AccessLevel.Internal;
                if (type.IsNestedFamily) return AccessLevel.Protected;
                if (type.IsNestedFamORAssem) return AccessLevel.ProtectedInternal; // protected or internal
                if (type.IsNestedFamANDAssem) return AccessLevel.PrivateProtected; // protected but only internal
                if (type.IsNestedPrivate) return AccessLevel.Private;
            }
            throw new Exception( "Access level is unknown: " + type );
        }
        // Helpers/IsModifier
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
        // Helpers/IsType
        private static bool IsInterface(this Type type) {
            return type.IsInterface;
        }
        private static bool IsClass(this Type type) {
            return type.IsClass && !type.IsSubclassOf( typeof( Delegate ) );
        }
        private static bool IsStruct(this Type type) {
            return type.IsValueType && !type.IsEnum;
        }
        private static bool IsEnum(this Type type) {
            return type.IsEnum;
        }
        private static bool IsDelegate(this Type type) {
            return type.IsClass && type.IsSubclassOf( typeof( Delegate ) );
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