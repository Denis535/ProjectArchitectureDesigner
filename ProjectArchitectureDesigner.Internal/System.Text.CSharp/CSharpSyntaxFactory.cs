// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace System.Text.CSharp {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public static class CSharpSyntaxFactory {


        // Type
        public static string GetTypeSyntax(this Type type) {
            if (type.IsInterface()) {
                return CSharpSyntaxFactoryHelper.GetTypeSyntax( type.GetModifiers(), type.GetSimpleIdentifier(), type.GetGenericArguments(), null, type.GetInterfaces() );
            }
            if (type.IsClass()) {
                return CSharpSyntaxFactoryHelper.GetTypeSyntax( type.GetModifiers(), type.GetSimpleIdentifier(), type.GetGenericArguments(), type.BaseType, type.GetInterfaces() );
            }
            if (type.IsStruct()) {
                return CSharpSyntaxFactoryHelper.GetTypeSyntax( type.GetModifiers(), type.GetSimpleIdentifier(), type.GetGenericArguments(), null, type.GetInterfaces() );
            }
            if (type.IsEnum()) {
                return CSharpSyntaxFactoryHelper.GetTypeSyntax( type.GetModifiers(), type.GetSimpleIdentifier(), type.GetGenericArguments(), Enum.GetUnderlyingType( type ), null );
            }
            if (type.IsDelegate()) {
                var method = type.GetMethod( "Invoke" );
                return CSharpSyntaxFactoryHelper.GetDelegateSyntax( type.GetModifiers(), method.ReturnParameter, type.GetSimpleIdentifier(), type.GetGenericArguments(), method.GetParameters() );
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
        // Member
        public static string GetMemberSyntax(this MemberInfo member) {
            if (member is FieldInfo field) {
                return GetFieldSyntax( field );
            }
            if (member is PropertyInfo property) {
                return GetPropertySyntax( property );
            }
            if (member is EventInfo @event) {
                return GetEventSyntax( @event );
            }
            if (member is ConstructorInfo constructor) {
                return GetConstructorSyntax( constructor );
            }
            if (member is MethodInfo method) {
                return GetMethodSyntax( method );
            }
            throw new ArgumentException( "Member is unsupported: " + member );
        }
        // Field
        public static string GetFieldSyntax(this FieldInfo field) {
            if (field.IsLiteral) {
                return CSharpSyntaxFactoryHelper.GetFieldSyntax( field.GetModifiers(), field.FieldType, field.Name, field.IsLiteral, field.GetRawConstantValue() );
            } else {
                return CSharpSyntaxFactoryHelper.GetFieldSyntax( field.GetModifiers(), field.FieldType, field.Name, field.IsLiteral, null );
            }
        }
        // Property
        public static string GetPropertySyntax(this PropertyInfo property) {
            if (property.IsIndexer()) {
                return CSharpSyntaxFactoryHelper.GetIndexerSyntax( property.GetModifiers(), property.PropertyType, property.GetIndexParameters(), property.GetMethod, property.SetMethod );
            } else {
                return CSharpSyntaxFactoryHelper.GetPropertySyntax( property.GetModifiers(), property.PropertyType, property.Name, property.GetMethod, property.SetMethod );
            }
        }
        // Event
        public static string GetEventSyntax(this EventInfo @event) {
            return CSharpSyntaxFactoryHelper.GetEventSyntax( @event.GetModifiers(), @event.EventHandlerType, @event.Name, @event.AddMethod, @event.RemoveMethod, @event.RaiseMethod );
        }
        // Constructor
        public static string GetConstructorSyntax(this ConstructorInfo constructor) {
            return CSharpSyntaxFactoryHelper.GetConstructorSyntax( constructor.GetModifiers(), constructor.DeclaringType.GetSimpleIdentifier(), constructor.GetParameters() );
        }
        // Method
        public static string GetMethodSyntax(this MethodInfo method) {
            if (method.IsOperator()) {
                return CSharpSyntaxFactoryHelper.GetOperatorSyntax( method.GetModifiers(), method.ReturnParameter, method.Name, method.GetGenericArguments(), method.GetParameters() );
            } else {
                return CSharpSyntaxFactoryHelper.GetMethodSyntax( method.GetModifiers(), method.ReturnParameter, method.Name, method.GetGenericArguments(), method.GetParameters() );
            }
        }


        // Helpers/GetModifiers
        private static IEnumerable<string> GetModifiers(this Type type) {
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
        private static IEnumerable<string> GetModifiers(this FieldInfo field) {
            yield return field.GetAccessLevel().GetModifier();
            if (field.IsLiteral) yield return "const";
            if (field.IsStatic && !field.IsLiteral) yield return "static";
            if (field.IsInitOnly) yield return "readonly";
        }
        private static IEnumerable<string> GetModifiers(this PropertyInfo property) {
            yield return property.GetAccessLevel().GetModifier();
            if (property.GetAccessors( true ).Any( CSharpSyntaxFactoryHelper3.IsStatic )) yield return "static";
            if (property.GetAccessors( true ).Any( CSharpSyntaxFactoryHelper3.IsAbstract )) yield return "abstract";
            if (property.GetAccessors( true ).Any( CSharpSyntaxFactoryHelper3.IsVirtual )) yield return "virtual";
            if (property.GetAccessors( true ).Any( CSharpSyntaxFactoryHelper3.IsOverride )) yield return "override";
            if (property.GetAccessors( true ).Any( CSharpSyntaxFactoryHelper3.IsSealed )) yield return "sealed";
        }
        private static IEnumerable<string> GetModifiers(this EventInfo @event) {
            yield return @event.GetAccessLevel().GetModifier();
            if (@event.GetAccessors( true ).Any( CSharpSyntaxFactoryHelper3.IsStatic )) yield return "static";
            if (@event.GetAccessors( true ).Any( CSharpSyntaxFactoryHelper3.IsAbstract )) yield return "abstract";
            if (@event.GetAccessors( true ).Any( CSharpSyntaxFactoryHelper3.IsVirtual )) yield return "virtual";
            if (@event.GetAccessors( true ).Any( CSharpSyntaxFactoryHelper3.IsOverride )) yield return "override";
            if (@event.GetAccessors( true ).Any( CSharpSyntaxFactoryHelper3.IsSealed )) yield return "sealed";
        }
        private static IEnumerable<string> GetModifiers(this ConstructorInfo constructor) {
            yield return constructor.GetAccessLevel().GetModifier();
            if (constructor.IsStatic) yield return "static";
        }
        private static IEnumerable<string> GetModifiers(this MethodInfo method) {
            yield return method.GetAccessLevel().GetModifier();
            if (method.IsStatic()) yield return "static";
            if (method.IsAbstract()) yield return "abstract";
            if (method.IsVirtual()) yield return "virtual";
            if (method.IsOverride()) yield return "override";
            if (method.IsSealed()) yield return "sealed";
        }


    }
}
