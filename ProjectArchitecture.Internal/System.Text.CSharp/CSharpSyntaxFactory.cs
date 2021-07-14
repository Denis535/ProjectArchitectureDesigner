// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace System.Text.CSharp {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    internal static class CSharpSyntaxFactory {


        // Type
        public static string GetTypeSyntax(IEnumerable<string> keywords, Type type, Type[] generics, Type? @base, Type[]? interfaces) {
            var builder = new List<string>();
            builder.AppendKeywords( keywords );
            builder.AppendSimpleIdentifier( type );
            builder.AppendGenerics( generics );
            builder.AppendBaseTypeAndInterfaces( @base, interfaces );
            builder.AppendConstraints( generics );
            return builder.GetDeclarationString();
        }
        // Type/Delegate
        public static string GetDelegateSyntax(IEnumerable<string> keywords, ParameterInfo result, Type type, Type[] generics, ParameterInfo[] parameters) {
            var builder = new List<string>();
            builder.AppendKeywords( keywords );
            builder.AppendResult( result );
            builder.AppendSimpleIdentifier( type );
            builder.AppendGenerics( generics );
            builder.AppendParameters( parameters );
            builder.AppendConstraints( generics );
            return builder.GetDeclarationString();
        }
        // Members
        public static string GetFieldSyntax(IEnumerable<string> keywords, Type type, string name, bool isLiteral, object? value) {
            var builder = new List<string>();
            builder.AppendKeywords( keywords );
            builder.AppendIdentifier( type );
            builder.Append( name );
            if (isLiteral) {
                builder.Append( "=" );
                builder.Append( value?.ToString() ?? "null" );
            }
            builder.Add( ";" );
            return builder.GetDeclarationString();
        }
        public static string GetPropertySyntax(IEnumerable<string> keywords, Type type, string name, MethodInfo? getter, MethodInfo? setter) {
            var builder = new List<string>();
            builder.AppendKeywords( keywords );
            builder.AppendIdentifier( type );
            builder.Append( name );
            builder.AppendPropertyAccessors( getter, setter );
            return builder.GetDeclarationString();
        }
        public static string GetEventSyntax(IEnumerable<string> keywords, Type type, string name, MethodInfo? adder, MethodInfo? remover, MethodInfo? raiser) {
            var builder = new List<string>();
            builder.AppendKeywords( keywords );
            builder.AppendIdentifier( type );
            builder.Append( name );
            builder.AppendEventAccessors( adder, remover, raiser );
            return builder.GetDeclarationString();
        }
        public static string GetConstructorSyntax(IEnumerable<string> keywords, Type type, ParameterInfo[] parameters) {
            var builder = new List<string>();
            builder.AppendKeywords( keywords );
            builder.AppendSimpleIdentifier( type );
            builder.AppendParameters( parameters );
            builder.Add( ";" );
            return builder.GetDeclarationString();
        }
        public static string GetMethodSyntax(IEnumerable<string> keywords, ParameterInfo result, string name, Type[] generics, ParameterInfo[] parameters) {
            var builder = new List<string>();
            builder.AppendKeywords( keywords );
            builder.AppendResult( result );
            builder.Append( name );
            builder.AppendGenerics( generics );
            builder.AppendParameters( parameters );
            builder.AppendConstraints( generics );
            builder.Add( ";" );
            return builder.GetDeclarationString();
        }


        // Keywords
        private static void AppendKeywords(this IList<string> builder, IEnumerable<string> keywords) {
            builder.AddRange( keywords );
        }
        // Type/Identifier
        private static void AppendIdentifier(this IList<string> builder, Type type) {
            builder.Add( type.GetIdentifier() );
        }
        private static void AppendSimpleIdentifier(this IList<string> builder, Type type) {
            builder.Add( type.GetSimpleIdentifier() );
        }
        // Type/Generics
        private static void AppendGenerics(this IList<string> builder, IEnumerable<Type> generics) {
            if (!generics.Any()) return;
            builder.Add( "<" );
            builder.AddRange( ",", generics, AppendGeneric );
            builder.Add( ">" );
        }
        private static void AppendGeneric(this IList<string> builder, Type generic) {
            builder.AddRange( generic.GetKeywords() );
            builder.Add( generic.Name );
        }
        // Type/Generics/Constraints
        private static void AppendConstraints(this IList<string> builder, IEnumerable<Type> generics) {
            builder.AddRange( generics, AppendConstraints );
        }
        private static void AppendConstraints(this IList<string> builder, Type generic) {
            if (generic.HasAnyConstraints()) {
                builder.Add( "where" );
                builder.Add( generic.Name );
                builder.Add( ":" );
                builder.AddRange( ",", generic.GetConstraints() );
            }
        }
        // Type/Base
        private static void AppendBaseTypeAndInterfaces(this IList<string> builder, Type? @base, IEnumerable<Type>? interfaces) {
            if (!Concat( @base, interfaces ).Any()) return;
            builder.Add( ":" );
            builder.AddRange( ",", Concat( @base, interfaces ), AppendIdentifier );
        }
        // Property/Accessors
        private static void AppendPropertyAccessors(this IList<string> builder, params MethodInfo?[] accessors) {
            builder.Add( "{" );
            builder.AddRange( accessors, AppendPropertyAccessor );
            builder.Add( "}" );
        }
        private static void AppendPropertyAccessor(this IList<string> builder, MethodInfo? method) {
            if (method != null && method.Name.StartsWith( "get_" )) {
                builder.Add( method.GetAccessModifier() );
                builder.Add( "get" );
                builder.Add( ";" );
            } else
            if (method != null && method.Name.StartsWith( "set_" )) {
                builder.Add( method.GetAccessModifier() );
                builder.Add( "set" );
                builder.Add( ";" );
            }
        }
        // Event/Accessors
        private static void AppendEventAccessors(this IList<string> builder, params MethodInfo?[] accessors) {
            builder.Add( "{" );
            builder.AddRange( accessors, AppendEventAccessor );
            builder.Add( "}" );
        }
        private static void AppendEventAccessor(this IList<string> builder, MethodInfo? method) {
            if (method != null && method.Name.StartsWith( "add_" )) {
                builder.Add( method.GetAccessModifier() );
                builder.Add( "add" );
                builder.Add( ";" );
            } else
            if (method != null && method.Name.StartsWith( "remove_" )) {
                builder.Add( method.GetAccessModifier() );
                builder.Add( "remove" );
                builder.Add( ";" );
            } else
            if (method != null && method.Name.StartsWith( "raise_" )) {
                builder.Add( method.GetAccessModifier() );
                builder.Add( "raise" );
                builder.Add( ";" );
            }
        }
        // Method/Parameters
        private static void AppendResult(this IList<string> builder, ParameterInfo parameter) {
            builder.Add( parameter.ParameterType.GetIdentifier() );
        }
        private static void AppendParameters(this IList<string> builder, IEnumerable<ParameterInfo> parameters) {
            builder.Add( "(" );
            builder.AddRange( ",", parameters, AppendParameter );
            builder.Add( ")" );
        }
        private static void AppendParameter(this IList<string> builder, ParameterInfo parameter) {
            builder.Add( parameter.ParameterType.GetIdentifier() );
            builder.Add( parameter.Name );
            if (parameter.HasDefaultValue) {
                builder.Add( "=" );
                builder.Add( parameter.DefaultValue?.ToString() ?? "null" );
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
        private static void AddRange(this IList<string> builder, IEnumerable<string> collection) {
            foreach (var item in collection) {
                builder.Add( item );
            }
        }
        private static void AddRange<T>(this IList<string> builder, IEnumerable<T> collection, Action<IList<string>, T> render) {
            foreach (var item in collection) {
                render( builder, item );
            }
        }
        // Helpers/AddRange/Separator
        private static void AddRange(this IList<string> builder, string separator, IEnumerable<string> collection) {
            foreach (var (item, isLast) in collection.TagLast()) {
                builder.Add( item );
                if (!isLast) builder.Add( separator );
            }
        }
        private static void AddRange<T>(this IList<string> builder, string separator, IEnumerable<T> collection, Action<IList<string>, T> render) {
            foreach (var (item, isLast) in collection.TagLast()) {
                render( builder, item );
                if (!isLast) builder.Add( separator );
            }
        }
        // Helpers/GetDeclarationString
        private static string GetDeclarationString(this IList<string> tokens) {
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
