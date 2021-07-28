// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace System.Text.CSharp {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    // public interface IInterface<in/out T> : IInterface<T> where T : class
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


        // Type
        public static string GetTypeSyntax(IEnumerable<string> modifiers, string name, Type[] parameters_generic, Type? @base, Type[]? interfaces) {
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.Add( name );
            tokens.AddSyntax_Generic_Parameters( parameters_generic );
            tokens.AddSyntax_BaseTypeAndInterfaces( @base, interfaces );
            tokens.AddSyntax_Generic_Constraints( parameters_generic );
            return tokens.Build();
        }
        // Delegate
        public static string GetDelegateSyntax(IEnumerable<string> modifiers, ParameterInfo result, string name, Type[] parameters_generic, ParameterInfo[] parameters) {
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.AddSyntax_Method_Result( result );
            tokens.Add( name );
            tokens.AddSyntax_Generic_Parameters( parameters_generic );
            tokens.AddSyntax_Method_Parameters( parameters );
            tokens.AddSyntax_Generic_Constraints( parameters_generic );
            tokens.Add( ";" );
            return tokens.Build();
        }
        // Field
        public static string GetFieldSyntax(IEnumerable<string> modifiers, Type type, string name, bool isLiteral, object? value) {
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.AddSyntax_Type( type );
            tokens.Add( name );
            if (isLiteral) {
                tokens.Add( "=" );
                tokens.Add( value?.ToString() ?? "null" );
            }
            tokens.Add( ";" );
            return tokens.Build();
        }
        // Property
        public static string GetPropertySyntax(IEnumerable<string> modifiers, Type type, string name, MethodInfo? getter, MethodInfo? setter) {
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.AddSyntax_Type( type );
            tokens.Add( name );
            tokens.AddSyntax_Property_Accessors( getter, setter );
            return tokens.Build();
        }
        // Indexer
        public static string GetIndexerSyntax(IEnumerable<string> modifiers, Type type, ParameterInfo[] indices, MethodInfo? getter, MethodInfo? setter) {
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.AddSyntax_Type( type );
            tokens.Add( "this" );
            tokens.AddSyntax_Property_Indices( indices );
            tokens.AddSyntax_Property_Accessors( getter, setter );
            return tokens.Build();
        }
        // Event
        public static string GetEventSyntax(IEnumerable<string> modifiers, Type type, string name, MethodInfo? adder, MethodInfo? remover, MethodInfo? raiser) {
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.AddSyntax_Type( type );
            tokens.Add( name );
            tokens.AddSyntax_Event_Accessors( adder, remover, raiser );
            return tokens.Build();
        }
        // Constructor
        public static string GetConstructorSyntax(IEnumerable<string> modifiers, string name, ParameterInfo[] parameters) {
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.Add( name );
            tokens.AddSyntax_Method_Parameters( parameters );
            tokens.Add( ";" );
            return tokens.Build();
        }
        // Method
        public static string GetMethodSyntax(IEnumerable<string> modifiers, ParameterInfo result, string name, Type[] parameters_generic, ParameterInfo[] parameters, bool isExtension) {
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.AddSyntax_Method_Result( result );
            tokens.Add( name );
            tokens.AddSyntax_Generic_Parameters( parameters_generic );
            tokens.AddSyntax_Method_Parameters( parameters, isExtension );
            tokens.AddSyntax_Generic_Constraints( parameters_generic );
            tokens.Add( ";" );
            return tokens.Build();
        }
        // Operator
        public static string GetOperatorSyntax(IEnumerable<string> modifiers, ParameterInfo result, string name, Type[] parameters_generic, ParameterInfo[] parameters) {
            // todo:
            var tokens = new List<string>();
            tokens.AddRange( modifiers );
            tokens.AddSyntax_Method_Result( result );
            tokens.Add( name );
            tokens.AddSyntax_Generic_Parameters( parameters_generic );
            tokens.AddSyntax_Method_Parameters( parameters );
            tokens.AddSyntax_Generic_Constraints( parameters_generic );
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
                if (next is "where" or "{" or ";" or null) return true;
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
