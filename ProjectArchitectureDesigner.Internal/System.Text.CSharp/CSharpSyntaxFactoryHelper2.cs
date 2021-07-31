// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace System.Text.CSharp {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    internal static class CSharpSyntaxFactoryHelper2 {


        // Type
        public static void AddSyntax_Type(this List<string> tokens, Type type) {
            tokens.AddToken( type );
        }

        // Generic
        public static void AddSyntax_Generic_Parameters(this List<string> tokens, Type[] parameters) {
            if (!parameters.Any()) return;
            tokens.AddToken( "<" );
            foreach (var parameter in parameters) {
                tokens.AddToken( "in", parameter.IsContravariant() ).AddToken( "out", parameter.IsCovariant() ).AddToken( parameter.Name ).AddToken( "," );
            }
            tokens.AddToken( ">" );
        }
        public static void AddSyntax_Generic_Constraints(this List<string> tokens, Type[] parameters) {
            foreach (var parameter in parameters) {
                if (parameter.HasAnyConstraints()) {
                    tokens.AddToken( "where" ).AddToken( parameter.Name ).AddToken( ":" );
                    if (parameter.HasReferenceTypeConstraint()) {
                        tokens.AddTokens( "class" ).AddTokens( parameter.GetGenericParameterConstraints() ).AddTokens( "new()", parameter.HasDefaultConstructorConstraint() );
                    } else
                    if (parameter.HasValueTypeConstraint()) {
                        tokens.AddTokens( "struct" ).AddTokens( parameter.GetGenericParameterConstraints().WhereNot( IsValueType ) );
                    } else {
                        tokens.AddTokens( parameter.GetGenericParameterConstraints() ).AddTokens( "new()", parameter.HasDefaultConstructorConstraint() );
                    }
                }
            }
        }

        // Property
        public static void AddSyntax_Property_Indices(this List<string> tokens, ParameterInfo[] indices) {
            tokens.AddToken( "[" );
            foreach (var index in indices) {
                if (!index.HasDefaultValue) {
                    tokens.AddToken( index.ParameterType ).AddToken( index.Name ).AddToken( "," );
                } else {
                    tokens.AddToken( index.ParameterType ).AddToken( index.Name ).AddToken( "=" ).AddToken( index.DefaultValue?.ToString() ?? "null" ).AddToken( "," );
                }
            }
            tokens.AddToken( "]" );
        }
        public static void AddSyntax_Property_Accessors(this List<string> tokens, MethodInfo? getter, MethodInfo? setter) {
            var @default = GetDefaultAccessLevel( getter?.GetAccessLevel(), setter?.GetAccessLevel() );
            tokens.AddToken( "{" );
            if (getter != null) {
                tokens.AddToken( getter.GetAccessLevel(), @default ).AddToken( "get" ).AddToken( ";" );
            }
            if (setter != null) {
                tokens.AddToken( setter.GetAccessLevel(), @default ).AddToken( "set" ).AddToken( ";" );
            }
            tokens.AddToken( "}" );
        }

        // Event
        public static void AddSyntax_Event_Accessors(this List<string> tokens, MethodInfo? adder, MethodInfo? remover, MethodInfo? raiser) {
            var @default = GetDefaultAccessLevel( adder?.GetAccessLevel(), remover?.GetAccessLevel(), raiser?.GetAccessLevel() );
            tokens.AddToken( "{" );
            if (adder != null) {
                tokens.AddToken( adder.GetAccessLevel(), @default ).AddToken( "add" ).AddToken( ";" );
            }
            if (remover != null) {
                tokens.AddToken( remover.GetAccessLevel(), @default ).AddToken( "remove" ).AddToken( ";" );
            }
            if (raiser != null) {
                tokens.AddToken( raiser.GetAccessLevel(), @default ).AddToken( "raise" ).AddToken( ";" );
            }
            tokens.AddToken( "}" );
        }

        // Method
        public static void AddSyntax_Method_Result(this List<string> tokens, ParameterInfo result) {
            tokens.AddToken( result.ParameterType );
        }
        public static void AddSyntax_Method_Parameters(this List<string> tokens, ParameterInfo[] parameters, bool isExtension = false) {
            tokens.AddToken( "(" );
            tokens.AddToken( "this", isExtension );
            foreach (var parameter in parameters) {
                if (!parameter.HasDefaultValue) {
                    tokens.AddToken( parameter.ParameterType ).AddToken( parameter.Name ).AddToken( "," );
                } else {
                    tokens.AddToken( parameter.ParameterType ).AddToken( parameter.Name ).AddToken( "=" ).AddToken( parameter.DefaultValue?.ToString() ?? "null" ).AddToken( "," );
                }
            }
            tokens.AddToken( ")" );
        }

        // Misc
        public static void AddSyntax_BaseTypeAndInterfaces(this List<string> tokens, Type? @base, Type[]? interfaces) {
            if (Concat( @base, interfaces ).Any()) {
                tokens.AddToken( ":" ).AddTokens( Concat( @base, interfaces ) );
            }
        }


        // Helpers
        private static IEnumerable<Type> Concat(Type? @base, Type[]? interfaces) {
            if (@base != null && interfaces != null) {
                return @base.Append( interfaces );
            }
            if (@base != null) {
                return @base.ToEnumerable();
            }
            if (interfaces != null) {
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
        // Helpers/List/AddToken
        private static List<string> AddToken(this List<string> tokens, string value) {
            tokens.Add( value );
            return tokens;
        }
        private static List<string> AddToken(this List<string> tokens, string value, bool condition) {
            if (!condition) return tokens;
            tokens.Add( value );
            return tokens;
        }
        private static List<string> AddToken(this List<string> tokens, Type value) {
            tokens.Add( value.GetIdentifier() );
            return tokens;
        }
        private static List<string> AddToken(this List<string> tokens, AccessLevel value, AccessLevel? @default) {
            if (value == @default) return tokens;
            tokens.Add( value.GetModifier() );
            return tokens;
        }
        // Helpers/List/AddTokens
        private static List<string> AddTokens(this List<string> tokens, string value) {
            tokens.Add( value );
            tokens.Add( "," );
            return tokens;
        }
        private static List<string> AddTokens(this List<string> tokens, string value, bool condition) {
            if (!condition) return tokens;
            tokens.Add( value );
            tokens.Add( "," );
            return tokens;
        }
        private static List<string> AddTokens(this List<string> tokens, IEnumerable<Type> values) {
            foreach (var value in values) {
                tokens.Add( value.GetIdentifier() );
                tokens.Add( "," );
            }
            return tokens;
        }
        // Helpers/Type
        private static bool IsValueType(this Type type) {
            return type == typeof( ValueType );
        }


    }
}
