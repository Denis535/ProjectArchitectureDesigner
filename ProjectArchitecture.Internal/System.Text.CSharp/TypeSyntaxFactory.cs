// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace System.Text.CSharp {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;

    // public interface Interface<in/out T> : IInterface<T> where T : class
    // public class     Class<T>            : Base<T>, IInterface<T> where T : class
    // public struct    Struct<T>           : IInterface<T> where T : class
    // public enum      Enum                : int
    // public delegate  T Delegate<in/out T>(T value) where T : class
    public static class TypeSyntaxFactory {


        public static string GetTypeSyntax(this Type type) {
            if (type.IsInterface()) {
                return CSharpSyntaxFactory.GetTypeSyntax( type.GetKeywords(), type, type.GetGenericArguments(), null, type.GetInterfaces() );
            }
            if (type.IsClass()) {
                return CSharpSyntaxFactory.GetTypeSyntax( type.GetKeywords(), type, type.GetGenericArguments(), type.BaseType, type.GetInterfaces() );
            }
            if (type.IsStruct()) {
                return CSharpSyntaxFactory.GetTypeSyntax( type.GetKeywords(), type, type.GetGenericArguments(), null, type.GetInterfaces() );
            }
            if (type.IsEnum()) {
                return CSharpSyntaxFactory.GetTypeSyntax( type.GetKeywords(), type, type.GetGenericArguments(), Enum.GetUnderlyingType( type ), null );
            }
            if (type.IsDelegate()) {
                var method = type.GetMethod( "Invoke" );
                return CSharpSyntaxFactory.GetDelegateSyntax( type.GetKeywords(), method.ReturnParameter, type, type.GetGenericArguments(), method.GetParameters() );
            }
            throw new ArgumentException( "Type is unsupported: " + type );
        }
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
        internal static string GetSimpleIdentifier(this Type type) {
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


        // Helpers/IsNullable
        private static bool IsNullable(this Type type, [MaybeNullWhen( false )] out Type underlying) {
            underlying = Nullable.GetUnderlyingType( type );
            return underlying != null;
        }


    }
}