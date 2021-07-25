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
    internal static class CSharpSyntaxFactoryHelper {


        // GetTypeSyntax
        public static string GetTypeSyntax(IEnumerable<string> modifiers, string name, Type[] parameters, Type? @base, Type[]? interfaces) {
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.Add( name );
            tokens.AddSyntax_Parameters( parameters );
            tokens.AddSyntax_BaseTypeAndInterfaces( @base, interfaces );
            tokens.AddSyntax_ParametersConstraints( parameters );
            return tokens.Build();
        }
        // GetDelegateSyntax
        public static string GetDelegateSyntax(IEnumerable<string> modifiers, ParameterInfo result, string name, Type[] parameters, ParameterInfo[] parameters2) {
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.AddSyntax_Result( result );
            tokens.Add( name );
            tokens.AddSyntax_Parameters( parameters );
            tokens.AddSyntax_Parameters( parameters2 );
            tokens.AddSyntax_ParametersConstraints( parameters );
            tokens.Add( ";" );
            return tokens.Build();
        }
        // GetMemberSyntax
        public static string GetFieldSyntax(IEnumerable<string> modifiers, Type type, string name, bool isLiteral, object? value) {
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.AddIdentifier( type );
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
            tokens.AddIdentifier( type );
            tokens.Add( name );
            tokens.AddSyntax_Accessors_Getter_Setter( getter, setter );
            return tokens.Build();
        }
        public static string GetIndexerSyntax(IEnumerable<string> modifiers, Type type, ParameterInfo[] indices, MethodInfo? getter, MethodInfo? setter) {
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.AddIdentifier( type );
            tokens.Add( "this" );
            tokens.AddSyntax_Indices( indices );
            tokens.AddSyntax_Accessors_Getter_Setter( getter, setter );
            return tokens.Build();
        }
        public static string GetEventSyntax(IEnumerable<string> modifiers, Type type, string name, MethodInfo? adder, MethodInfo? remover, MethodInfo? raiser) {
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.AddIdentifier( type );
            tokens.Add( name );
            tokens.AddSyntax_Accessors_Adder_Remover_Raiser( adder, remover, raiser );
            return tokens.Build();
        }
        public static string GetConstructorSyntax(IEnumerable<string> modifiers, string name, ParameterInfo[] parameters) {
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.Add( name );
            tokens.AddSyntax_Parameters( parameters );
            tokens.Add( ";" );
            return tokens.Build();
        }
        public static string GetMethodSyntax(IEnumerable<string> modifiers, ParameterInfo result, string name, Type[] parameters, ParameterInfo[] parameters2) {
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.AddSyntax_Result( result );
            tokens.Add( name );
            tokens.AddSyntax_Parameters( parameters );
            tokens.AddSyntax_Parameters( parameters2 );
            tokens.AddSyntax_ParametersConstraints( parameters );
            tokens.Add( ";" );
            return tokens.Build();
        }
        public static string GetOperatorSyntax(IEnumerable<string> modifiers, ParameterInfo result, string name, Type[] parameters, ParameterInfo[] parameters2) {
            // todo:
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.AddSyntax_Result( result );
            tokens.Add( name );
            tokens.AddSyntax_Parameters( parameters );
            tokens.AddSyntax_Parameters( parameters2 );
            tokens.AddSyntax_ParametersConstraints( parameters );
            tokens.Add( ";" );
            return tokens.Build();
        }


        // Helpers/Build
        private static string Build(this IList<string> tokens) {
            var builder = new StringBuilder();
            foreach (var (item, next) in tokens.WithNext()) {
                if (!IsExtra( item, next.ValueOrDefault )) builder.Append( item );
                if (ShouldHaveSpaceAfter( item ) && ShouldHaveSpaceBefore( next.ValueOrDefault )) builder.Append( ' ' );
            }
            return builder.ToString();
        }
        private static bool IsExtra(string value, string? next) {
            if (value is ",") {
                if (next is ">" or ")" or "]") return true;
                if (next is "where" or ";" or null) return true;
            }
            return false;
        }
        private static bool ShouldHaveSpaceAfter(string value) {
            if (value is "<" or "(" or "[") return false;
            return true;
        }
        private static bool ShouldHaveSpaceBefore(string? next) {
            if (next is "<" or "(" or "[") return false;
            if (next is ">" or ")" or "]") return false;
            if (next is "," or ";") return false;
            if (next is null) return false;
            return true;
        }


    }
}
