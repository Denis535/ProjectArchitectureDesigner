// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace System.Text.DisplayStringUtils {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    internal static class CSharpSyntaxUtils {


        // AppendObjects
        public static void AppendKeywords(this StringBuilder builder, IEnumerable<string> keywords) {
            foreach (var value in keywords) {
                builder.Append( value ).Append( ' ' );
            }
        }
        // AppendObjects/Type/Generics
        public static void AppendGenerics(this StringBuilder builder, IEnumerable<Type> generics) {
            if (!generics.Any()) return;
            builder.Append( '<' );
            builder.AppendJoin( ", ", generics, AppendGeneric );
            builder.Append( '>' );
        }
        public static void AppendConstraints(this StringBuilder builder, IEnumerable<Type> generics) {
            builder.AppendRange( generics, AppendConstraints );
        }
        // AppendObjects/Type/Base
        public static void AppendBaseTypeAndInterfaces(this StringBuilder builder, Type? @base, IEnumerable<Type>? interfaces) {
            if (!Concat( @base, interfaces ).Any()) return;
            builder.Append( " : " );
            builder.AppendJoin( ", ", Concat( @base, interfaces ), AppendIdentifier );
        }
        // AppendObjects/Property/Accessors
        public static void AppendPropertyAccessors(this StringBuilder builder, params MethodInfo?[] accessors) {
            builder.Append( "{ " );
            builder.AppendJoin( " ", accessors.OfType<MethodInfo>(), AppendPropertyAccessor );
            builder.Append( " }" );
        }
        // AppendObjects/Event/Accessors
        public static void AppendEventAccessors(this StringBuilder builder, params MethodInfo?[] accessors) {
            builder.Append( "{ " );
            builder.AppendJoin( " ", accessors.OfType<MethodInfo>(), AppendEventAccessor );
            builder.Append( " }" );
        }
        // AppendObjects/Method/Parameters
        public static void AppendParameters(this StringBuilder builder, IEnumerable<ParameterInfo> parameters) {
            builder.Append( '(' );
            builder.AppendJoin( ", ", parameters, AppendParameter );
            builder.Append( ')' );
        }


        // AppendObject/Type/Generic
        public static void AppendGeneric(this StringBuilder builder, Type generic) {
            builder.AppendKeywords( generic.GetKeywords() );
            builder.Append( generic.Name );
        }
        public static void AppendConstraints(this StringBuilder builder, Type generic) {
            if (generic.HasAnyConstraints()) {
                builder.Append( ' ' );
                builder.Append( "where" );
                builder.Append( ' ' );
                builder.Append( generic.Name );
                builder.Append( " : " );
                builder.AppendJoin( ", ", generic.GetConstraints() );
            }
        }
        // AppendObject/Property/Accessor
        public static void AppendPropertyAccessor(this StringBuilder builder, MethodInfo? method) {
            if (method != null && method.Name.StartsWith( "get_" )) {
                builder.Append( method.GetAccessModifier() ).Append( ' ' ).Append( "get;" );
            } else
            if (method != null && method.Name.StartsWith( "set_" )) {
                builder.Append( method.GetAccessModifier() ).Append( ' ' ).Append( "set;" );
            }
        }
        public static void AppendEventAccessor(this StringBuilder builder, MethodInfo? method) {
            if (method != null && method.Name.StartsWith( "add_" )) {
                builder.Append( method.GetAccessModifier() ).Append( ' ' ).Append( "add;" );
            } else
            if (method != null && method.Name.StartsWith( "remove_" )) {
                builder.Append( method.GetAccessModifier() ).Append( ' ' ).Append( "remove;" );
            } else
            if (method != null && method.Name.StartsWith( "raise_" )) {
                builder.Append( method.GetAccessModifier() ).Append( ' ' ).Append( "raise;" );
            }
        }
        // AppendObject/Method/Parameter
        public static void AppendResult(this StringBuilder builder, ParameterInfo parameter) {
            builder.Append( parameter.ParameterType.GetIdentifier() );
        }
        public static void AppendParameter(this StringBuilder builder, ParameterInfo parameter) {
            builder.Append( parameter.ParameterType.GetIdentifier() );
            builder.Append( ' ' );
            builder.Append( parameter.Name );
            if (parameter.HasDefaultValue) {
                builder.Append( " = " );
                builder.Append( parameter.DefaultValue?.ToString() ?? "null" );
            }
        }


        // AppendObject/Identifier
        public static void AppendIdentifier(this StringBuilder builder, Type type) {
            builder.Append( type.GetIdentifier() );
        }
        public static void AppendSimpleIdentifier(this StringBuilder builder, Type type) {
            builder.Append( type.GetSimpleIdentifier() );
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


    }
}
