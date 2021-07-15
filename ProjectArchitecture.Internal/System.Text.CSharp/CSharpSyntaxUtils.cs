// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace System.Text.CSharp {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    internal static class CSharpSyntaxUtils {


        // Type
        public static string GetTypeSyntax(IEnumerable<string> keywords, Type type, Type[] generics, Type? @base, Type[]? interfaces) {
            var tokens = new List<string>();
            tokens.AddKeywords( keywords );
            tokens.AddSimpleIdentifier( type );
            tokens.AddGenerics( generics );
            tokens.AddBaseTypeAndInterfaces( @base, interfaces );
            tokens.AddConstraints( generics );
            return tokens.Build();
        }
        // Type/Delegate
        public static string GetDelegateSyntax(IEnumerable<string> keywords, ParameterInfo result, Type type, Type[] generics, ParameterInfo[] parameters) {
            var tokens = new List<string>();
            tokens.AddKeywords( keywords );
            tokens.AddResult( result );
            tokens.AddSimpleIdentifier( type );
            tokens.AddGenerics( generics );
            tokens.AddParameters( parameters );
            tokens.AddConstraints( generics );
            tokens.Add( ";" );
            return tokens.Build();
        }
        // Members
        public static string GetFieldSyntax(IEnumerable<string> keywords, Type type, string name, bool isLiteral, object? value) {
            var tokens = new List<string>();
            tokens.AddKeywords( keywords );
            tokens.AddIdentifier( type );
            tokens.Add( name );
            if (isLiteral) {
                tokens.Add( "=" );
                tokens.Add( value?.ToString() ?? "null" );
            }
            tokens.Add( ";" );
            return tokens.Build();
        }
        public static string GetPropertySyntax(IEnumerable<string> keywords, Type type, string name, MethodInfo? getter, MethodInfo? setter) {
            var tokens = new List<string>();
            tokens.AddKeywords( keywords );
            tokens.AddIdentifier( type );
            tokens.Add( name );
            tokens.AddPropertyAccessors( getter, setter );
            return tokens.Build();
        }
        public static string GetEventSyntax(IEnumerable<string> keywords, Type type, string name, MethodInfo? adder, MethodInfo? remover, MethodInfo? raiser) {
            var tokens = new List<string>();
            tokens.AddKeywords( keywords );
            tokens.AddIdentifier( type );
            tokens.Add( name );
            tokens.AddEventAccessors( adder, remover, raiser );
            return tokens.Build();
        }
        public static string GetConstructorSyntax(IEnumerable<string> keywords, Type type, ParameterInfo[] parameters) {
            var tokens = new List<string>();
            tokens.AddKeywords( keywords );
            tokens.AddSimpleIdentifier( type );
            tokens.AddParameters( parameters );
            tokens.Add( ";" );
            return tokens.Build();
        }
        public static string GetMethodSyntax(IEnumerable<string> keywords, ParameterInfo result, string name, Type[] generics, ParameterInfo[] parameters) {
            var tokens = new List<string>();
            tokens.AddKeywords( keywords );
            tokens.AddResult( result );
            tokens.Add( name );
            tokens.AddGenerics( generics );
            tokens.AddParameters( parameters );
            tokens.AddConstraints( generics );
            tokens.Add( ";" );
            return tokens.Build();
        }


        // Keywords
        private static void AddKeywords(this IList<string> tokens, IEnumerable<string> keywords) {
            tokens.AddRange( keywords );
        }
        // Type/Identifier
        private static void AddIdentifier(this IList<string> tokens, Type type) {
            tokens.Add( type.GetIdentifier() );
        }
        private static void AddSimpleIdentifier(this IList<string> tokens, Type type) {
            tokens.Add( type.GetSimpleIdentifier() );
        }
        // Type/Generics
        private static void AddGenerics(this IList<string> tokens, IEnumerable<Type> generics) {
            if (!generics.Any()) return;
            tokens.Add( "<" );
            tokens.AddRange( ",", generics, AddGeneric );
            tokens.Add( ">" );
        }
        private static void AddGeneric(this IList<string> tokens, Type generic) {
            tokens.AddRange( generic.GetKeywords() );
            tokens.Add( generic.Name );
        }
        // Type/Generics/Constraints
        private static void AddConstraints(this IList<string> tokens, IEnumerable<Type> generics) {
            tokens.AddRange( generics, AddConstraints );
        }
        private static void AddConstraints(this IList<string> tokens, Type generic) {
            if (generic.HasAnyConstraints()) {
                tokens.Add( "where" );
                tokens.Add( generic.Name );
                tokens.Add( ":" );
                tokens.AddRange( ",", generic.GetConstraints() );
            }
        }
        // Type/Base
        private static void AddBaseTypeAndInterfaces(this IList<string> tokens, Type? @base, IEnumerable<Type>? interfaces) {
            if (!Concat( @base, interfaces ).Any()) return;
            tokens.Add( ":" );
            tokens.AddRange( ",", Concat( @base, interfaces ), AddIdentifier );
        }
        // Property/Accessors
        private static void AddPropertyAccessors(this IList<string> tokens, params MethodInfo?[] accessors) {
            tokens.Add( "{" );
            tokens.AddRange( accessors, AddPropertyAccessor );
            tokens.Add( "}" );
        }
        private static void AddPropertyAccessor(this IList<string> tokens, MethodInfo? method) {
            if (method != null && method.Name.StartsWith( "get_" )) {
                tokens.Add( method.GetAccessModifier() );
                tokens.Add( "get" );
                tokens.Add( ";" );
            } else
            if (method != null && method.Name.StartsWith( "set_" )) {
                tokens.Add( method.GetAccessModifier() );
                tokens.Add( "set" );
                tokens.Add( ";" );
            }
        }
        // Event/Accessors
        private static void AddEventAccessors(this IList<string> tokens, params MethodInfo?[] accessors) {
            tokens.Add( "{" );
            tokens.AddRange( accessors, AddEventAccessor );
            tokens.Add( "}" );
        }
        private static void AddEventAccessor(this IList<string> tokens, MethodInfo? method) {
            if (method != null && method.Name.StartsWith( "add_" )) {
                tokens.Add( method.GetAccessModifier() );
                tokens.Add( "add" );
                tokens.Add( ";" );
            } else
            if (method != null && method.Name.StartsWith( "remove_" )) {
                tokens.Add( method.GetAccessModifier() );
                tokens.Add( "remove" );
                tokens.Add( ";" );
            } else
            if (method != null && method.Name.StartsWith( "raise_" )) {
                tokens.Add( method.GetAccessModifier() );
                tokens.Add( "raise" );
                tokens.Add( ";" );
            }
        }
        // Method/Parameters
        private static void AddResult(this IList<string> tokens, ParameterInfo parameter) {
            tokens.Add( parameter.ParameterType.GetIdentifier() );
        }
        private static void AddParameters(this IList<string> tokens, IEnumerable<ParameterInfo> parameters) {
            tokens.Add( "(" );
            tokens.AddRange( ",", parameters, AddParameter );
            tokens.Add( ")" );
        }
        private static void AddParameter(this IList<string> tokens, ParameterInfo parameter) {
            tokens.Add( parameter.ParameterType.GetIdentifier() );
            tokens.Add( parameter.Name );
            if (parameter.HasDefaultValue) {
                tokens.Add( "=" );
                tokens.Add( parameter.DefaultValue?.ToString() ?? "null" );
            }
        }


        // Helpers
        private static IEnumerable<Type> Concat(Type? @base, IEnumerable<Type>? interfaces) {
            if (@base != null && interfaces?.Any() == true) {
                return @base.Append( interfaces );
            }
            if (@base != null) {
                return @base.AsEnumerable();
            }
            if (interfaces?.Any() == true) {
                return interfaces;
            }
            return Enumerable.Empty<Type>();
        }
        // Helpers/AddRange
        private static void AddRange(this IList<string> tokens, IEnumerable<string> collection) {
            foreach (var item in collection) {
                tokens.Add( item );
            }
        }
        private static void AddRange<T>(this IList<string> tokens, IEnumerable<T> collection, Action<IList<string>, T> render) {
            foreach (var item in collection) {
                render( tokens, item );
            }
        }
        // Helpers/AddRange/Separator
        private static void AddRange(this IList<string> tokens, string separator, IEnumerable<string> collection) {
            foreach (var (item, isLast) in collection.TagLast()) {
                tokens.Add( item );
                if (!isLast) tokens.Add( separator );
            }
        }
        private static void AddRange<T>(this IList<string> tokens, string separator, IEnumerable<T> collection, Action<IList<string>, T> render) {
            foreach (var (item, isLast) in collection.TagLast()) {
                render( tokens, item );
                if (!isLast) tokens.Add( separator );
            }
        }
        // Helpers/Build
        private static string Build(this IList<string> tokens) {
            var builder = new StringBuilder();
            foreach (var (item, next) in tokens.WithNext()) {
                builder.Append( item );
                if (next.HasValue && ShouldHaveSpace( item, next.Value )) builder.Append( ' ' );
            }
            return builder.ToString();
        }
        private static bool ShouldHaveSpace(string value, string next) {
            if (value is "<" or "(") return false;
            if (next is "<" or "(") return false;
            if (next is ">" or ")") return false;
            if (next is "," or ";") return false;
            return true;
        }


    }
}
