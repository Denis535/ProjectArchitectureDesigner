// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace System.Text.CSharp {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    // public interface Interface<in/out T> : IInterface<T> where T : class
    // public class     Class<T>            : Base<T>, IInterface<T> where T : class
    // public struct    Struct<T>           : IInterface<T> where T : class
    // public enum      Enum                : int
    // public delegate  T Delegate<in/out T>(T value) where T : class

    // public object Field;
    // public event Action Event;
    // public object Property { get; set; }
    // public event Action Event { add; remove; raise; }
    // public T Func<T>(T Arg1, T Arg2);
    internal static class CSharpSyntaxFactory {


        // GetTypeSyntax
        public static string GetTypeSyntax(IEnumerable<string> modifiers, Type type, Type[] generics, Type? @base, Type[]? interfaces) {
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.AddType_SimpleIdentifier( type );
            tokens.AddType_GenericParameters( generics );
            tokens.AddType_BaseTypeAndInterfaces( @base, interfaces );
            tokens.AddType_GenericParametersConstraints( generics );
            return tokens.Build();
        }
        // GetDelegateSyntax
        public static string GetDelegateSyntax(IEnumerable<string> modifiers, ParameterInfo result, Type type, Type[] generics, ParameterInfo[] parameters) {
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.AddMethod_Result( result );
            tokens.AddType_SimpleIdentifier( type );
            tokens.AddType_GenericParameters( generics );
            tokens.AddMethod_Parameters( parameters );
            tokens.AddType_GenericParametersConstraints( generics );
            tokens.Add( ";" );
            return tokens.Build();
        }
        // GetMemberSyntax
        public static string GetFieldSyntax(IEnumerable<string> modifiers, Type type, string name, bool isLiteral, object? value) {
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.AddType_Identifier( type );
            tokens.Add( name );
            if (isLiteral) {
                tokens.Add( "=" );
                tokens.Add( value?.ToString() ?? "null" );
            }
            tokens.Add( ";" );
            return tokens.Build();
        }
        public static string GetPropertySyntax(IEnumerable<string> modifiers, Type type, string name, MethodInfo? getter, MethodInfo? setter) {
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.AddType_Identifier( type );
            tokens.Add( name );
            tokens.AddProperty_Accessors( getter, setter );
            return tokens.Build();
        }
        public static string GetIndexerSyntax(IEnumerable<string> modifiers, Type type, ParameterInfo[] indices, MethodInfo? getter, MethodInfo? setter) {
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.AddType_Identifier( type );
            tokens.Add( "this" );
            tokens.AddProperty_Indices( indices );
            tokens.AddProperty_Accessors( getter, setter );
            return tokens.Build();
        }
        public static string GetEventSyntax(IEnumerable<string> modifiers, Type type, string name, MethodInfo? adder, MethodInfo? remover, MethodInfo? raiser) {
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.AddType_Identifier( type );
            tokens.Add( name );
            tokens.AddEvent_Accessors( adder, remover, raiser );
            return tokens.Build();
        }
        public static string GetConstructorSyntax(IEnumerable<string> modifiers, Type type, ParameterInfo[] parameters) {
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.AddType_SimpleIdentifier( type );
            tokens.AddMethod_Parameters( parameters );
            tokens.Add( ";" );
            return tokens.Build();
        }
        public static string GetMethodSyntax(IEnumerable<string> modifiers, ParameterInfo result, string name, Type[] generics, ParameterInfo[] parameters) {
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.AddMethod_Result( result );
            tokens.Add( name );
            tokens.AddType_GenericParameters( generics );
            tokens.AddMethod_Parameters( parameters );
            tokens.AddType_GenericParametersConstraints( generics );
            tokens.Add( ";" );
            return tokens.Build();
        }
        public static string GetOperatorSyntax(IEnumerable<string> modifiers, ParameterInfo result, string name, Type[] generics, ParameterInfo[] parameters) {
            // todo:
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.AddMethod_Result( result );
            tokens.Add( name );
            tokens.AddType_GenericParameters( generics );
            tokens.AddMethod_Parameters( parameters );
            tokens.AddType_GenericParametersConstraints( generics );
            tokens.Add( ";" );
            return tokens.Build();
        }


    }
}
