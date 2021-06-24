// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;

    public static class TypeExtensions {


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
                builder.Append( '<' ).AppendRange( type.GetGenericArguments().Select( GetIdentifier ) ).Append( '>' );
                return builder.ToString();
            }
            if (type.IsConstructedGenericType) {
                var builder = new StringBuilder();
                builder.Append( type.GetSimpleIdentifier() );
                builder.Append( '<' ).AppendRange( type.GetGenericArguments().Select( GetIdentifier ) ).Append( '>' );
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


        // IsKeyword/Modifiers
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
        // IsKeyword/Types
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


        // MemberInfo
        //public static bool IsSpecialName(MemberInfo member) {
        //    if (member is FieldInfo field) return field.IsSpecialName;
        //    if (member is PropertyInfo prop) return prop.IsSpecialName;
        //    if (member is EventInfo @event) return @event.IsSpecialName;
        //    if (member is MethodBase method) return method.IsSpecialName;
        //    return false;
        //}
        //public static bool IsIndexer(this PropertyInfo property) {
        //    return property.Name == "Item" && property.GetIndexParameters().Length > 0;
        //}
        //public static bool IsOperator(MethodInfo method) {
        //    return method.IsPublic && method.IsStatic && method.IsSpecialName && method.Name.StartsWith( "op_" );
        //}
        //// MemberInfo/Keywords
        //public static bool IsPublic(this FieldInfo field) {
        //    return field.IsPublic && !field.DeclaringType.IsEnum;
        //}
        //public static bool IsPublic(this PropertyInfo property) {
        //    return property.IsPublic && !property.DeclaringType.IsInterface;
        //}
        //public static bool IsPublic(this MethodInfo method) {
        //    return method.IsPublic && !method.DeclaringType.IsInterface;
        //}
        //public static bool IsAbstract(this PropertyInfo property) {
        //    if (property.DeclaringType.IsInterface) return false;
        //    return property.IsAbstract;
        //}
        //public static bool IsAbstract(this MethodInfo method) {
        //    return method.IsAbstract;
        //}
        //public static bool IsVirtual(this MethodInfo method) {
        //    return method.IsVirtual && !method.IsAbstract && !method.IsOverride();
        //}
        //public static bool IsOverride(this MethodInfo method) {
        //    return method.DeclaringType != method.GetBaseDefinition().DeclaringType;
        //}
        //public static bool IsSealed(this MethodInfo method) {
        //    return method.IsFinal;
        //}


        // Helpers
        private static bool IsNullable(this Type type, [MaybeNullWhen( false )] out Type underlying) {
            underlying = Nullable.GetUnderlyingType( type );
            return underlying != null;
        }


    }
}