// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace System.Text.CSharp {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
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
    internal static class AccessLevelExtensions {
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
    }
    internal static class CSharpSyntaxFactoryHelper3 {


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
        public static bool IsNullable(this Type type, [MaybeNullWhen( false )] out Type underlying) {
            return (underlying = Nullable.GetUnderlyingType( type )) != null;
        }
        public static bool IsEnum(this Type type) {
            return type.IsEnum;
        }
        public static bool IsDelegate(this Type type) {
            return type.IsClass && type.IsSubclassOf( typeof( Delegate ) );
        }
        public static bool IsIndexer(this PropertyInfo property) {
            return property.Name == "Item" && property.GetIndexParameters().Length > 0;
        }
        public static bool IsOperator(this MethodInfo method) {
            return method.IsPublic && method.IsStatic && method.IsSpecialName && method.Name.StartsWith( "op_" );
        }


        // GetAccessLevel
        public static AccessLevel GetAccessLevel(this Type type) {
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
        public static AccessLevel GetAccessLevel(this FieldInfo field) {
            if (field.IsPublic) return AccessLevel.Public;
            if (field.IsAssembly) return AccessLevel.Internal;
            if (field.IsFamily) return AccessLevel.Protected;
            if (field.IsFamilyOrAssembly) return AccessLevel.ProtectedInternal;
            if (field.IsFamilyAndAssembly) return AccessLevel.PrivateProtected;
            if (field.IsPrivate) return AccessLevel.Private;
            throw new Exception( "Access level is unknown: " + field );
        }
        public static AccessLevel GetAccessLevel(this PropertyInfo property) {
            if (property.GetAccessors( true ).Any( i => i.IsPublic )) return AccessLevel.Public;
            if (property.GetAccessors( true ).Any( i => i.IsAssembly )) return AccessLevel.Internal;
            if (property.GetAccessors( true ).Any( i => i.IsFamily )) return AccessLevel.Protected;
            if (property.GetAccessors( true ).Any( i => i.IsFamilyOrAssembly )) return AccessLevel.ProtectedInternal;
            if (property.GetAccessors( true ).Any( i => i.IsFamilyAndAssembly )) return AccessLevel.PrivateProtected;
            if (property.GetAccessors( true ).Any( i => i.IsPrivate )) return AccessLevel.Private;
            throw new Exception( "Access level is unknown: " + property );
        }
        public static AccessLevel GetAccessLevel(this EventInfo @event) {
            if (@event.GetAccessors( true ).Any( i => i.IsPublic )) return AccessLevel.Public;
            if (@event.GetAccessors( true ).Any( i => i.IsAssembly )) return AccessLevel.Internal;
            if (@event.GetAccessors( true ).Any( i => i.IsFamily )) return AccessLevel.Protected;
            if (@event.GetAccessors( true ).Any( i => i.IsFamilyOrAssembly )) return AccessLevel.ProtectedInternal;
            if (@event.GetAccessors( true ).Any( i => i.IsFamilyAndAssembly )) return AccessLevel.PrivateProtected;
            if (@event.GetAccessors( true ).Any( i => i.IsPrivate )) return AccessLevel.Private;
            throw new Exception( "Access level is unknown: " + @event );
        }
        public static AccessLevel GetAccessLevel(this MethodBase method) {
            if (method.IsPublic) return AccessLevel.Public;
            if (method.IsAssembly) return AccessLevel.Internal;
            if (method.IsFamily) return AccessLevel.Protected;
            if (method.IsFamilyOrAssembly) return AccessLevel.ProtectedInternal;
            if (method.IsFamilyAndAssembly) return AccessLevel.PrivateProtected;
            if (method.IsPrivate) return AccessLevel.Private;
            throw new Exception( "Access level is unknown: " + method );
        }


        // IsModifier
        public static bool IsStatic(this Type type) {
            return type.IsAbstract && type.IsSealed;
        }
        public static bool IsAbstract(this Type type) {
            return type.IsAbstract && !type.IsSealed;
        }
        public static bool IsSealed(this Type type) {
            return type.IsSealed && !type.IsAbstract;
        }
        public static bool IsContravariant(this Type type) { // in
            return type.GenericParameterAttributes.HasFlag( GenericParameterAttributes.Contravariant );
        }
        public static bool IsCovariant(this Type type) { // out
            return type.GenericParameterAttributes.HasFlag( GenericParameterAttributes.Covariant );
        }
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


        // HasConstraint
        public static bool HasAnyConstraints(this Type type) {
            return type.GenericParameterAttributes.HasFlag( GenericParameterAttributes.SpecialConstraintMask ) || type.GetGenericParameterConstraints().Any();
        }
        public static bool HasReferenceTypeConstraint(this Type type) {
            return type.GenericParameterAttributes.HasFlag( GenericParameterAttributes.ReferenceTypeConstraint );
        }
        public static bool HasValueTypeConstraint(this Type type) {
            return type.GenericParameterAttributes.HasFlag( GenericParameterAttributes.NotNullableValueTypeConstraint );
        }
        public static bool HasDefaultConstructorConstraint(this Type type) {
            return type.GenericParameterAttributes.HasFlag( GenericParameterAttributes.DefaultConstructorConstraint );
        }


        // Helpers/GetAccessors
        internal static IEnumerable<MethodInfo> GetAccessors(this EventInfo @event, bool nonPublic) {
            if (@event.GetAddMethod( nonPublic ) != null) yield return @event.GetAddMethod( nonPublic );
            if (@event.GetRemoveMethod( nonPublic ) != null) yield return @event.GetRemoveMethod( nonPublic );
            if (@event.GetRaiseMethod( nonPublic ) != null) yield return @event.GetRaiseMethod( nonPublic );
        }


    }
}
