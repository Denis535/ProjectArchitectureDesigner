﻿// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace System.Text.CSharp {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    internal static class CSharpSyntaxFactory {


        // GetTypeSyntax
        public static string GetTypeSyntax(IEnumerable<string> keywords, Type type, Type[] generics, Type? @base, Type[]? interfaces) {
            var tokens = new List<string>();
            tokens.AddRange( keywords );
            tokens.Add( type.GetSimpleIdentifier() );
            tokens.AddGenerics( generics );
            tokens.AddBaseTypeAndInterfaces( @base, interfaces );
            tokens.AddRange( generics, AddConstraints );
            return tokens.Build();
        }
        // GetDelegateSyntax
        public static string GetDelegateSyntax(IEnumerable<string> keywords, ParameterInfo result, Type type, Type[] generics, ParameterInfo[] parameters) {
            var tokens = new List<string>();
            tokens.AddRange( keywords );
            tokens.AddResult( result );
            tokens.Add( type.GetSimpleIdentifier() );
            tokens.AddGenerics( generics );
            tokens.AddParameters( parameters );
            tokens.AddRange( generics, AddConstraints );
            tokens.Add( ";" );
            return tokens.Build();
        }
        // GetMemberSyntax
        public static string GetFieldSyntax(IEnumerable<string> keywords, Type type, string name, bool isLiteral, object? value) {
            var tokens = new List<string>();
            tokens.AddRange( keywords );
            tokens.Add( type.GetIdentifier() );
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
            tokens.AddRange( keywords );
            tokens.Add( type.GetIdentifier() );
            tokens.Add( name );
            tokens.AddPropertyAccessors( getter, setter );
            return tokens.Build();
        }
        public static string GetIndexerSyntax(IEnumerable<string> keywords, Type type, ParameterInfo[] indices, MethodInfo? getter, MethodInfo? setter) {
            var tokens = new List<string>();
            tokens.AddRange( keywords );
            tokens.Add( type.GetIdentifier() );
            tokens.Add( "this" );
            tokens.AddIndices( indices );
            tokens.AddPropertyAccessors( getter, setter );
            return tokens.Build();
        }
        public static string GetEventSyntax(IEnumerable<string> keywords, Type type, string name, MethodInfo? adder, MethodInfo? remover, MethodInfo? raiser) {
            var tokens = new List<string>();
            tokens.AddRange( keywords );
            tokens.Add( type.GetIdentifier() );
            tokens.Add( name );
            tokens.AddEventAccessors( adder, remover, raiser );
            return tokens.Build();
        }
        public static string GetConstructorSyntax(IEnumerable<string> keywords, Type type, ParameterInfo[] parameters) {
            var tokens = new List<string>();
            tokens.AddRange( keywords );
            tokens.Add( type.GetSimpleIdentifier() );
            tokens.AddParameters( parameters );
            tokens.Add( ";" );
            return tokens.Build();
        }
        public static string GetMethodSyntax(IEnumerable<string> keywords, ParameterInfo result, string name, Type[] generics, ParameterInfo[] parameters) {
            var tokens = new List<string>();
            tokens.AddRange( keywords );
            tokens.AddResult( result );
            tokens.Add( name );
            tokens.AddGenerics( generics );
            tokens.AddParameters( parameters );
            tokens.AddRange( generics, AddConstraints );
            tokens.Add( ";" );
            return tokens.Build();
        }
        public static string GetOperatorSyntax(IEnumerable<string> keywords, ParameterInfo result, string name, Type[] generics, ParameterInfo[] parameters) {
            // todo:
            var tokens = new List<string>();
            tokens.AddRange( keywords );
            tokens.AddResult( result );
            tokens.Add( name );
            tokens.AddGenerics( generics );
            tokens.AddParameters( parameters );
            tokens.AddRange( generics, AddConstraints );
            tokens.Add( ";" );
            return tokens.Build();
        }


        // Type/AddGenerics
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
        // Type/AddBaseTypeAndInterfaces
        private static void AddBaseTypeAndInterfaces(this IList<string> tokens, Type? @base, IEnumerable<Type>? interfaces) {
            if (!Concat( @base, interfaces ).Any()) return;
            tokens.Add( ":" );
            tokens.AddRange( ",", Concat( @base, interfaces ), (i, t) => i.Add( t.GetIdentifier() ) );
        }
        // Type/AddConstraints
        private static void AddConstraints(this IList<string> tokens, Type generic) {
            if (!generic.GetConstraints().Any()) return;
            tokens.Add( "where" );
            tokens.Add( generic.Name );
            tokens.Add( ":" );
            tokens.AddRange( ",", generic.GetConstraints() );
        }
        // Property/AddAccessors
        private static void AddPropertyAccessors(this IList<string> tokens, MethodInfo? getter, MethodInfo? setter) {
            tokens.Add( "{" );
            if (getter != null) {
                tokens.Add( getter.GetAccessModifier() ); // todo: don't add default accessor
                tokens.Add( "get" );
                tokens.Add( ";" );
            }
            if (setter != null) {
                tokens.Add( setter.GetAccessModifier() );
                tokens.Add( "set" );
                tokens.Add( ";" );
            }
            tokens.Add( "}" );
        }
        // Property/AddIndices
        private static void AddIndices(this IList<string> tokens, ParameterInfo[] indices) {
            tokens.Add( "[" );
            tokens.AddRange( ",", indices, AddParameter );
            tokens.Add( "]" );
        }
        // Event/AddAccessors
        private static void AddEventAccessors(this IList<string> tokens, MethodInfo? adder, MethodInfo? remover, MethodInfo? raiser) {
            tokens.Add( "{" );
            if (adder != null) {
                tokens.Add( adder.GetAccessModifier() );
                tokens.Add( "add" );
                tokens.Add( ";" );
            }
            if (remover != null) {
                tokens.Add( remover.GetAccessModifier() );
                tokens.Add( "remove" );
                tokens.Add( ";" );
            }
            if (raiser != null) {
                tokens.Add( raiser.GetAccessModifier() );
                tokens.Add( "raise" );
                tokens.Add( ";" );
            }
            tokens.Add( "}" );
        }
        // Method/AddResult
        private static void AddResult(this IList<string> tokens, ParameterInfo parameter) {
            tokens.Add( parameter.ParameterType.GetIdentifier() );
        }
        // Method/AddParameters
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


        // Build
        private static string Build(this IList<string> tokens) {
            var builder = new StringBuilder();
            foreach (var (item, next) in tokens.WithNext()) {
                builder.Append( item );
                if (next.HasValue && ShouldHaveSpaceAfter( item ) && ShouldHaveSpaceBefore( next.Value )) {
                    builder.Append( ' ' );
                }
            }
            return builder.ToString();
        }
        private static bool ShouldHaveSpaceAfter(string value) {
            if (value is "<" or "(" or "[") return false;
            return true;
        }
        private static bool ShouldHaveSpaceBefore(string next) {
            if (next is "<" or "(" or "[") return false;
            if (next is ">" or ")" or "]") return false;
            if (next is "," or ";") return false;
            return true;
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


    }
}