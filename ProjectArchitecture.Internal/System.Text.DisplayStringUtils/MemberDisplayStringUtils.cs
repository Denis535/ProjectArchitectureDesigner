// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace System.Text.DisplayStringUtils {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    // public object Field;
    // public event Action Event;
    // public object Property { get; set; }
    // public event Action Event { add; remove; raise; }
    // public T Func<T>(T Arg1, T Arg2);
    public static class MemberDisplayStringUtils {


        public static string GetDisplayString(MemberInfo member) {
            if (member is FieldInfo field) {
                return GetDisplayString( field );
            }
            if (member is PropertyInfo property) {
                return GetDisplayString( property );
            }
            if (member is EventInfo @event) {
                return GetDisplayString( @event );
            }
            if (member is ConstructorInfo constructor) {
                return GetDisplayString( constructor );
            }
            if (member is MethodInfo method) {
                return GetDisplayString( method );
            }
            throw new ArgumentException( "Member is unsupported: " + member );
        }
        public static string GetDisplayString(FieldInfo field) {
            if (field.IsLiteral) {
                return GetFieldDeclaration( field.GetKeywords(), field.FieldType, field.Name, field.IsLiteral, field.GetRawConstantValue() );
            } else {
                return GetFieldDeclaration( field.GetKeywords(), field.FieldType, field.Name, field.IsLiteral, null );
            }
        }
        public static string GetDisplayString(PropertyInfo property) {
            return GetPropertyDeclaration( property.GetKeywords(), property.PropertyType, property.Name, property.GetMethod, property.SetMethod );
        }
        public static string GetDisplayString(EventInfo @event) {
            return GetEventDeclaration( @event.GetKeywords(), @event.EventHandlerType, @event.Name, @event.AddMethod, @event.RemoveMethod, @event.RaiseMethod );
        }
        public static string GetDisplayString(ConstructorInfo constructor) {
            return GetConstructorDeclaration( constructor.GetKeywords(), constructor.DeclaringType, constructor.GetParameters() );
        }
        public static string GetDisplayString(MethodInfo method) {
            return GetMethodDeclaration( method.GetKeywords(), method.ReturnParameter, method.Name, method.GetGenericArguments(), method.GetParameters() );
        }


        // Helpers/GetObjectDeclaration
        private static string GetFieldDeclaration(IEnumerable<string> keywords, Type type, string name, bool isLiteral, object? value) {
            var builder = new StringBuilder();
            builder.AppendKeywords( keywords );
            builder.AppendIdentifier( type );
            builder.Append( ' ' );
            builder.Append( name );
            if (isLiteral) {
                builder.Append( " = " );
                builder.Append( value?.ToString() ?? "null" );
            }
            builder.Append( ';' );
            return builder.ToString();
        }
        private static string GetPropertyDeclaration(IEnumerable<string> keywords, Type type, string name, MethodInfo? getter, MethodInfo? setter) {
            var builder = new StringBuilder();
            builder.AppendKeywords( keywords );
            builder.AppendIdentifier( type );
            builder.Append( ' ' );
            builder.Append( name );
            builder.Append( ' ' );
            builder.AppendPropertyAccessors( getter, setter );
            return builder.ToString();
        }
        private static string GetEventDeclaration(IEnumerable<string> keywords, Type type, string name, MethodInfo? adder, MethodInfo? remover, MethodInfo? raiser) {
            var builder = new StringBuilder();
            builder.AppendKeywords( keywords );
            builder.AppendIdentifier( type );
            builder.Append( ' ' );
            builder.Append( name );
            builder.Append( ' ' );
            builder.AppendEventAccessors( adder, remover, raiser );
            return builder.ToString();
        }
        private static string GetConstructorDeclaration(IEnumerable<string> keywords, Type type, ParameterInfo[] parameters) {
            var builder = new StringBuilder();
            builder.AppendKeywords( keywords );
            builder.AppendSimpleIdentifier( type );
            builder.AppendParameters( parameters );
            builder.Append( ';' );
            return builder.ToString();
        }
        private static string GetMethodDeclaration(IEnumerable<string> keywords, ParameterInfo result, string name, Type[] generics, ParameterInfo[] parameters) {
            var builder = new StringBuilder();
            builder.AppendKeywords( keywords );
            builder.AppendResult( result );
            builder.Append( ' ' );
            builder.Append( name );
            builder.AppendGenerics( generics );
            builder.AppendParameters( parameters );
            builder.AppendConstraints( generics );
            builder.Append( ';' );
            return builder.ToString();
        }


    }
}
