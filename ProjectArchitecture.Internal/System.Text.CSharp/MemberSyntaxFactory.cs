// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace System.Text.CSharp {
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
    public static class MemberSyntaxFactory {


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
        public static string GetFieldSyntax(this FieldInfo field) {
            if (field.IsLiteral) {
                return CSharpSyntaxFactory.GetFieldSyntax( field.GetKeywords(), field.FieldType, field.Name, field.IsLiteral, field.GetRawConstantValue() );
            } else {
                return CSharpSyntaxFactory.GetFieldSyntax( field.GetKeywords(), field.FieldType, field.Name, field.IsLiteral, null );
            }
        }
        public static string GetPropertySyntax(this PropertyInfo property) {
            if (property.IsIndexer()) {
                return CSharpSyntaxFactory.GetIndexerSyntax( property.GetKeywords(), property.PropertyType, property.GetIndexParameters(), property.GetMethod, property.SetMethod );
            } else {
                return CSharpSyntaxFactory.GetPropertySyntax( property.GetKeywords(), property.PropertyType, property.Name, property.GetMethod, property.SetMethod );
            }
        }
        public static string GetEventSyntax(this EventInfo @event) {
            return CSharpSyntaxFactory.GetEventSyntax( @event.GetKeywords(), @event.EventHandlerType, @event.Name, @event.AddMethod, @event.RemoveMethod, @event.RaiseMethod );
        }
        public static string GetConstructorSyntax(this ConstructorInfo constructor) {
            return CSharpSyntaxFactory.GetConstructorSyntax( constructor.GetKeywords(), constructor.DeclaringType, constructor.GetParameters() );
        }
        public static string GetMethodSyntax(this MethodInfo method) {
            if (method.IsOperator()) {
                return CSharpSyntaxFactory.GetOperatorSyntax( method.GetKeywords(), method.ReturnParameter, method.Name, method.GetGenericArguments(), method.GetParameters() );
            } else {
                return CSharpSyntaxFactory.GetMethodSyntax( method.GetKeywords(), method.ReturnParameter, method.Name, method.GetGenericArguments(), method.GetParameters() );
            }
        }


    }
}
