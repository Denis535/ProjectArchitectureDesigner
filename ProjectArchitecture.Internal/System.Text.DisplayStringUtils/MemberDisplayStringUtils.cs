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
                return CSharpSyntaxUtils.GetFieldDeclaration( field.GetKeywords(), field.FieldType, field.Name, field.IsLiteral, field.GetRawConstantValue() );
            } else {
                return CSharpSyntaxUtils.GetFieldDeclaration( field.GetKeywords(), field.FieldType, field.Name, field.IsLiteral, null );
            }
        }
        public static string GetDisplayString(PropertyInfo property) {
            return CSharpSyntaxUtils.GetPropertyDeclaration( property.GetKeywords(), property.PropertyType, property.Name, property.GetMethod, property.SetMethod );
        }
        public static string GetDisplayString(EventInfo @event) {
            return CSharpSyntaxUtils.GetEventDeclaration( @event.GetKeywords(), @event.EventHandlerType, @event.Name, @event.AddMethod, @event.RemoveMethod, @event.RaiseMethod );
        }
        public static string GetDisplayString(ConstructorInfo constructor) {
            return CSharpSyntaxUtils.GetConstructorDeclaration( constructor.GetKeywords(), constructor.DeclaringType, constructor.GetParameters() );
        }
        public static string GetDisplayString(MethodInfo method) {
            return CSharpSyntaxUtils.GetMethodDeclaration( method.GetKeywords(), method.ReturnParameter, method.Name, method.GetGenericArguments(), method.GetParameters() );
        }


    }
}
