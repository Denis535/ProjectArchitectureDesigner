// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace System.Text.CSharp {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

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
                return CSharpSyntaxFactory.GetFieldSyntax( field.GetModifiers(), field.FieldType, field.Name, field.IsLiteral, field.GetRawConstantValue() );
            } else {
                return CSharpSyntaxFactory.GetFieldSyntax( field.GetModifiers(), field.FieldType, field.Name, field.IsLiteral, null );
            }
        }
        public static string GetPropertySyntax(this PropertyInfo property) {
            if (property.IsIndexer()) {
                return CSharpSyntaxFactory.GetIndexerSyntax( property.GetModifiers(), property.PropertyType, property.GetIndexParameters(), property.GetMethod, property.SetMethod );
            } else {
                return CSharpSyntaxFactory.GetPropertySyntax( property.GetModifiers(), property.PropertyType, property.Name, property.GetMethod, property.SetMethod );
            }
        }
        public static string GetEventSyntax(this EventInfo @event) {
            return CSharpSyntaxFactory.GetEventSyntax( @event.GetModifiers(), @event.EventHandlerType, @event.Name, @event.AddMethod, @event.RemoveMethod, @event.RaiseMethod );
        }
        public static string GetConstructorSyntax(this ConstructorInfo constructor) {
            return CSharpSyntaxFactory.GetConstructorSyntax( constructor.GetModifiers(), constructor.DeclaringType, constructor.GetParameters() );
        }
        public static string GetMethodSyntax(this MethodInfo method) {
            if (method.IsOperator()) {
                return CSharpSyntaxFactory.GetOperatorSyntax( method.GetModifiers(), method.ReturnParameter, method.Name, method.GetGenericArguments(), method.GetParameters() );
            } else {
                return CSharpSyntaxFactory.GetMethodSyntax( method.GetModifiers(), method.ReturnParameter, method.Name, method.GetGenericArguments(), method.GetParameters() );
            }
        }


        // Helpers/IsType
        private static bool IsIndexer(this PropertyInfo property) {
            return property.Name == "Item" && property.GetIndexParameters().Length > 0;
        }
        private static bool IsOperator(this MethodInfo method) {
            return method.IsPublic && method.IsStatic && method.IsSpecialName && method.Name.StartsWith( "op_" );
        }


    }
}
