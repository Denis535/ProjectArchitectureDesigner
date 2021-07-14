// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace System.Text.CSharp {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    internal static class TypeExtensions {


        // GetIdentifier
        public static string GetIdentifier(this Type type) {
            if (type.IsByRef) {
                return "ref " + type.GetElementType().GetIdentifier();
            }
            if (type.IsNullable( out var underlying )) {
                return underlying.GetIdentifier() + "?";
            }
            if (type.IsGenericTypeDefinition) {
                var builder = new StringBuilder();
                builder.Append( type.GetSimpleIdentifier() );
                builder.Append( '<' ).AppendJoin( ", ", type.GetGenericArguments().Select( GetIdentifier ) ).Append( '>' );
                return builder.ToString();
            }
            if (type.IsConstructedGenericType) {
                var builder = new StringBuilder();
                builder.Append( type.GetSimpleIdentifier() );
                builder.Append( '<' ).AppendJoin( ", ", type.GetGenericArguments().Select( GetIdentifier ) ).Append( '>' );
                return builder.ToString();
            }
            if (type.IsGenericParameter) {
                return type.Name;
            }
            return type.GetSimpleIdentifier();
        }
        public static string GetSimpleIdentifier(this Type type) {
            if (type == typeof( void )) return "void";
            if (type == typeof( object )) return "object";
            if (type == typeof( string )) return "string";

            if (type == typeof( bool )) return "bool";
            if (type == typeof( char )) return "char";

            if (type == typeof( byte )) return "byte";
            if (type == typeof( short )) return "short";
            if (type == typeof( int )) return "int";
            if (type == typeof( long )) return "long";

            if (type == typeof( sbyte )) return "sbyte";
            if (type == typeof( ushort )) return "ushort";
            if (type == typeof( uint )) return "uint";
            if (type == typeof( ulong )) return "ulong";

            if (type == typeof( float )) return "float";
            if (type == typeof( double )) return "double";

            if (type == typeof( decimal )) return "decimal";

            if (type.IsGenericType) {
                var name = type.Name;
                return name.Remove( name.IndexOf( '`' ) );
            }
            return type.Name;
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


        // GetConstraints
        public static IEnumerable<string> GetConstraints(this Type type) {
            if (type.HasReferenceTypeConstraint()) {
                yield return "class";
            }
            if (type.HasNotNullableValueTypeConstraint()) {
                yield return "struct";
            }
            foreach (var constraint in type.GetGenericParameterConstraints()) {
                if (constraint == typeof( ValueType )) continue;
                yield return constraint.GetIdentifier();
            }
            if (type.HasDefaultConstructorConstraint() && !type.HasNotNullableValueTypeConstraint()) {
                yield return "new()";
            }
        }


        // IsKeyword
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
        // HasConstraint
        public static bool HasAnyConstraints(this Type type) {
            return type.GenericParameterAttributes.HasFlag( GenericParameterAttributes.SpecialConstraintMask ) || type.GetGenericParameterConstraints().Any();
        }
        public static bool HasReferenceTypeConstraint(this Type type) {
            return type.GenericParameterAttributes.HasFlag( GenericParameterAttributes.ReferenceTypeConstraint );
        }
        public static bool HasNotNullableValueTypeConstraint(this Type type) {
            return type.GenericParameterAttributes.HasFlag( GenericParameterAttributes.NotNullableValueTypeConstraint );
        }
        public static bool HasDefaultConstructorConstraint(this Type type) {
            return type.GenericParameterAttributes.HasFlag( GenericParameterAttributes.DefaultConstructorConstraint );
        }


        // Helpers/IsNullable
        private static bool IsNullable(this Type type, [MaybeNullWhen( false )] out Type underlying) {
            underlying = Nullable.GetUnderlyingType( type );
            return underlying != null;
        }


    }
}