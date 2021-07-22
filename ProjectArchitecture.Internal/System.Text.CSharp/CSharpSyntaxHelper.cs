// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace System.Text.CSharp {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    internal static class CSharpSyntaxHelper {


        // AddType/Identifier
        public static void AddType_Identifier(this List<string> tokens, Type type) {
            tokens.Add( type.GetIdentifier() );
        }
        // AddType/SimpleIdentifier
        public static void AddType_SimpleIdentifier(this List<string> tokens, Type type) {
            tokens.Add( type.GetSimpleIdentifier() );
        }
        // AddType/GenericParameters
        public static void AddType_GenericParameters(this List<string> tokens, IEnumerable<Type> parameters) {
            if (!parameters.Any()) return;
            tokens.Add( "<" );
            foreach (var (parameter, isLast) in parameters.TagLast()) {
                tokens.AddRange( parameter.GetModifiers() );
                tokens.Add( parameter.Name );
                tokens.AddIf( !isLast, "," );
            }
            tokens.Add( ">" );
        }
        // AddType/GenericParametersConstraints
        public static void AddType_GenericParametersConstraints(this List<string> tokens, IEnumerable<Type> parameters) {
            foreach (var parameter in parameters) {
                if (parameter.GetConstraints().Any()) {
                    tokens.Add( "where" );
                    tokens.Add( parameter.Name );
                    tokens.Add( ":" );
                    foreach (var (constraint, isLast) in parameter.GetConstraints().TagLast()) {
                        tokens.Add( constraint );
                        tokens.AddIf( !isLast, "," );
                    }
                }
            }
        }
        // AddType/BaseTypeAndInterfaces
        public static void AddType_BaseTypeAndInterfaces(this List<string> tokens, Type? @base, IEnumerable<Type>? interfaces) {
            if (!Concat( @base, interfaces ).Any()) return;
            tokens.Add( ":" );
            foreach (var (type, isLast) in Concat( @base, interfaces ).TagLast()) {
                tokens.Add( type.GetIdentifier() );
                tokens.AddIf( !isLast, "," );
            }
        }

        // AddProperty/Indices
        public static void AddProperty_Indices(this List<string> tokens, IEnumerable<ParameterInfo> indices) {
            tokens.Add( "[" );
            foreach (var (parameter, isLast) in indices.TagLast()) {
                tokens.Add( parameter.ParameterType.GetIdentifier() );
                tokens.Add( parameter.Name );
                if (parameter.HasDefaultValue) {
                    tokens.Add( "=" );
                    tokens.Add( parameter.DefaultValue?.ToString() ?? "null" );
                }
                tokens.AddIf( !isLast, "," );
            }
            tokens.Add( "]" );
        }
        // AddProperty/Accessors
        public static void AddProperty_Accessors(this List<string> tokens, MethodInfo? getter, MethodInfo? setter) {
            tokens.Add( "{" );
            if (getter != null) {
                tokens.Add( getter.GetAccessLevel().GetModifier() ); // todo: don't add default accessor
                tokens.Add( "get" );
                tokens.Add( ";" );
            }
            if (setter != null) {
                tokens.Add( setter.GetAccessLevel().GetModifier() );
                tokens.Add( "set" );
                tokens.Add( ";" );
            }
            tokens.Add( "}" );
        }

        // AddEvent/Accessors
        public static void AddEvent_Accessors(this List<string> tokens, MethodInfo? adder, MethodInfo? remover, MethodInfo? raiser) {
            tokens.Add( "{" );
            if (adder != null) {
                tokens.Add( adder.GetAccessLevel().GetModifier() );
                tokens.Add( "add" );
                tokens.Add( ";" );
            }
            if (remover != null) {
                tokens.Add( remover.GetAccessLevel().GetModifier() );
                tokens.Add( "remove" );
                tokens.Add( ";" );
            }
            if (raiser != null) {
                tokens.Add( raiser.GetAccessLevel().GetModifier() );
                tokens.Add( "raise" );
                tokens.Add( ";" );
            }
            tokens.Add( "}" );
        }

        // AddMethod/Result
        public static void AddMethod_Result(this List<string> tokens, ParameterInfo result) {
            tokens.Add( result.ParameterType.GetIdentifier() );
        }
        // AddMethod/Parameters
        public static void AddMethod_Parameters(this List<string> tokens, IEnumerable<ParameterInfo> parameters) {
            tokens.Add( "(" );
            foreach (var (parameter, isLast) in parameters.TagLast()) {
                tokens.Add( parameter.ParameterType.GetIdentifier() );
                tokens.Add( parameter.Name );
                if (parameter.HasDefaultValue) {
                    tokens.Add( "=" );
                    tokens.Add( parameter.DefaultValue?.ToString() ?? "null" );
                }
                tokens.AddIf( !isLast, "," );
            }
            tokens.Add( ")" );
        }


        // Build
        public static string Build(this IList<string> tokens) {
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
        private static void AddIf(this List<string> tokens, bool condition, string value) {
            if (!condition) return;
            tokens.Add( value );
        }


    }
}
