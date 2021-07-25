// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace System.Text.CSharp {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    internal static class CSharpSyntaxFactoryHelper2 {


        // Add
        public static void AddIdentifier(this List<string> tokens, Type type) {
            tokens.Add( type );
        }

        // AddSyntax
        public static void AddSyntax_Result(this List<string> tokens, ParameterInfo result) {
            tokens.Add( result.ParameterType );
        }

        // AddSyntax
        public static void AddSyntax_Parameters(this List<string> tokens, Type[] parameters) {
            if (!parameters.Any()) return;
            tokens.Add( "<" );
            foreach (var parameter in parameters) {
                tokens.Add( "in", parameter.IsContravariant() );
                tokens.Add( "out", parameter.IsCovariant() );
                tokens.Add( parameter.Name );
                tokens.Add( "," );
            }
            tokens.Add( ">" );
        }
        public static void AddSyntax_Parameters(this List<string> tokens, ParameterInfo[] parameters) {
            tokens.Add( "(" );
            foreach (var parameter in parameters) {
                tokens.Add( parameter.ParameterType );
                tokens.Add( parameter.Name );
                if (parameter.HasDefaultValue) {
                    tokens.Add( "=" );
                    tokens.Add( parameter.DefaultValue?.ToString() ?? "null" );
                }
                tokens.Add( "," );
            }
            tokens.Add( ")" );
        }
        public static void AddSyntax_Indices(this List<string> tokens, ParameterInfo[] indices) {
            tokens.Add( "[" );
            foreach (var index in indices) {
                tokens.Add( index.ParameterType );
                tokens.Add( index.Name );
                if (index.HasDefaultValue) {
                    tokens.Add( "=" );
                    tokens.Add( index.DefaultValue?.ToString() ?? "null" );
                }
                tokens.Add( "," );
            }
            tokens.Add( "]" );
        }

        // AddSyntax
        public static void AddSyntax_Accessors_Getter_Setter(this List<string> tokens, MethodInfo? getter, MethodInfo? setter) {
            var @default = GetDefaultAccessLevel( getter?.GetAccessLevel(), setter?.GetAccessLevel() );
            tokens.Add( "{" );
            if (getter != null) {
                tokens.Add( getter.GetAccessLevel(), @default );
                tokens.Add( "get" );
                tokens.Add( ";" );
            }
            if (setter != null) {
                tokens.Add( setter.GetAccessLevel(), @default );
                tokens.Add( "set" );
                tokens.Add( ";" );
            }
            tokens.Add( "}" );
        }
        public static void AddSyntax_Accessors_Adder_Remover_Raiser(this List<string> tokens, MethodInfo? adder, MethodInfo? remover, MethodInfo? raiser) {
            var @default = GetDefaultAccessLevel( adder?.GetAccessLevel(), remover?.GetAccessLevel(), raiser?.GetAccessLevel() );
            tokens.Add( "{" );
            if (adder != null) {
                tokens.Add( adder.GetAccessLevel(), @default );
                tokens.Add( "add" );
                tokens.Add( ";" );
            }
            if (remover != null) {
                tokens.Add( remover.GetAccessLevel(), @default );
                tokens.Add( "remove" );
                tokens.Add( ";" );
            }
            if (raiser != null) {
                tokens.Add( raiser.GetAccessLevel(), @default );
                tokens.Add( "raise" );
                tokens.Add( ";" );
            }
            tokens.Add( "}" );
        }

        // AddSyntax
        public static void AddSyntax_BaseTypeAndInterfaces(this List<string> tokens, Type? @base, Type[]? interfaces) {
            if (Concat( @base, interfaces ).Any()) {
                tokens.Add( ":" );
                tokens.Add( ",", Concat( @base, interfaces ) );
            }
        }
        public static void AddSyntax_ParametersConstraints(this List<string> tokens, Type[] parameters) {
            foreach (var parameter in parameters) {
                if (parameter.HasAnyConstraints()) {
                    tokens.Add( "where" );
                    tokens.Add( parameter.Name );
                    tokens.Add( ":" );
                    if (parameter.HasReferenceTypeConstraint()) {
                        tokens.Add( "class" );
                        tokens.Add( "," );
                        tokens.Add( ",", parameter.GetGenericParameterConstraints() );
                        tokens.Add( "new()", parameter.HasDefaultConstructorConstraint() );
                    } else if (parameter.HasValueTypeConstraint()) {
                        tokens.Add( "struct" );
                        tokens.Add( "," );
                        tokens.Add( ",", parameter.GetGenericParameterConstraints().WhereNot( IsValueType ) );
                    } else {
                        tokens.Add( ",", parameter.GetGenericParameterConstraints() );
                        tokens.Add( "new()", parameter.HasDefaultConstructorConstraint() );
                    }
                }
            }
        }


        // Helpers
        private static IEnumerable<Type> Concat(Type? @base, Type[]? interfaces) {
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
        // Helpers/AccessLevel
        private static AccessLevel? GetDefaultAccessLevel(AccessLevel? level1, AccessLevel? level2) {
            var result = default( AccessLevel? );
            if (result == null || level1 < result) result = level1;
            if (result == null || level2 < result) result = level2;
            return result;
        }
        private static AccessLevel? GetDefaultAccessLevel(AccessLevel? level1, AccessLevel? level2, AccessLevel? level3) {
            var result = default( AccessLevel? );
            if (result == null || level1 < result) result = level1;
            if (result == null || level2 < result) result = level2;
            if (result == null || level3 < result) result = level3;
            return result;
        }
        // Helpers/List
        private static void Add(this List<string> tokens, string value, bool condition) {
            if (condition) tokens.Add( value );
        }
        private static void Add(this List<string> tokens, Type value) {
            tokens.Add( value.GetIdentifier() );
        }
        private static void Add(this List<string> tokens, string separator, IEnumerable<Type> values) {
            tokens.AddRange( values.Select( i => i.GetIdentifier() ).WithSuffix( separator ) );
        }
        private static void Add(this List<string> tokens, AccessLevel value, AccessLevel? @default) {
            if (value != @default) tokens.Add( value.GetModifier() );
        }
        // Helpers/Type
        private static bool IsValueType(this Type type) {
            return type == typeof( ValueType );
        }


    }
}
