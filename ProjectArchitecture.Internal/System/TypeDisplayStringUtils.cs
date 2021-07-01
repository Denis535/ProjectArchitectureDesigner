// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    // public interface Interface<in/out T> : IInterface<T>
    // public class     Class<T> : Base<T>, IInterface<T>
    // public struct    Struct<T> : IInterface<T>
    // public enum      Enum : int
    // public delegate  T Delegate<in/out T>(T value)
    public static class TypeDisplayStringUtils {


        public static string GetDisplayString(Type type) {
            if (type.IsInterface()) {
                return GetTypeDeclaration( type.GetKeywords(), type, null, type.GetInterfaces() );
            }
            if (type.IsClass()) {
                return GetTypeDeclaration( type.GetKeywords(), type, type.BaseType, type.GetInterfaces() );
            }
            if (type.IsStruct()) {
                return GetTypeDeclaration( type.GetKeywords(), type, null, type.GetInterfaces() );
            }
            if (type.IsEnum()) {
                return GetTypeDeclaration( type.GetKeywords(), type, Enum.GetUnderlyingType( type ), null );
            }
            if (type.IsDelegate()) {
                var method = type.GetMethod( "Invoke" );
                return GetDelegateDeclaration( type.GetKeywords(), method.ReturnParameter, type, method.GetParameters() );
            }
            throw new ArgumentException( "Type is unsupported: " + type );
        }


        // Helpers/GetDeclaration
        private static string GetTypeDeclaration(IEnumerable<string> keywords, Type type, Type? @base, IEnumerable<Type>? interfaces) {
            return GetTypeDeclaration(
                keywords,
                type.GetSimpleDeclaration(),
                @base?.GetIdentifier(),
                interfaces?.Select( TypeExtensions.GetIdentifier ) );
        }
        private static string GetDelegateDeclaration(IEnumerable<string> keywords, ParameterInfo parameter, Type type, IEnumerable<ParameterInfo> parameters) {
            return GetDelegateDeclaration(
                keywords,
                parameter.GetSimpleDeclaration(),
                type.GetSimpleDeclaration(),
                parameters.Select( GetSimpleDeclaration ) );
        }
        // Helpers/GetDeclaration
        private static string GetTypeDeclaration(IEnumerable<string> keywords, string type, string? @base, IEnumerable<string>? interfaces) {
            var builder = new StringBuilder();
            builder.AppendKeywords( keywords );
            builder.Append( type );
            builder.AppendValuesIfAny( " : ", null, Concat( @base, interfaces ) );
            return builder.ToString();
        }
        private static string GetDelegateDeclaration(IEnumerable<string> keywords, string parameter, string type, IEnumerable<string> parameters) {
            var builder = new StringBuilder();
            builder.AppendKeywords( keywords );
            builder.Append( parameter ).Space();
            builder.Append( type );
            builder.AppendValues( "(", ")", parameters );
            return builder.ToString();
        }
        // Helpers/GetSimpleDeclaration
        private static string GetSimpleDeclaration(this Type type) {
            if (type.IsGenericTypeDefinition) {
                var builder = new StringBuilder();
                builder.Append( type.GetSimpleIdentifier() ).AppendValues( "<", ">", type.GetGenericArguments().Select( GetSimpleDeclaration_GenericParameter ) );
                return builder.ToString();
            }
            if (type.IsConstructedGenericType) {
                var builder = new StringBuilder();
                builder.Append( type.GetSimpleIdentifier() ).AppendValues( "<", ">", type.GetGenericArguments().Select( TypeExtensions.GetIdentifier ) );
                return builder.ToString();
            }
            return type.GetSimpleIdentifier();
        }
        private static string GetSimpleDeclaration_GenericParameter(this Type type) {
            var builder = new StringBuilder();
            builder.AppendKeywords( type.GetKeywords_GenericParameter() );
            builder.Append( type.Name );
            return builder.ToString();
        }
        private static string GetSimpleDeclaration(this ParameterInfo parameter) {
            var builder = new StringBuilder();
            if (parameter.Name.IsEmpty()) {
                builder.Append( parameter.ParameterType.GetIdentifier() );
            } else {
                builder.Append( parameter.ParameterType.GetIdentifier() ).Space().Append( parameter.Name );
            }
            return builder.ToString();
        }
        // Helpers/StringBuilder
        private static StringBuilder AppendKeywords(this StringBuilder builder, IEnumerable<string> values) {
            foreach (var value in values) builder.Append( value ).Space();
            return builder;
        }
        private static StringBuilder AppendValues(this StringBuilder builder, string? prefix, string? suffix, IEnumerable<string> values) {
            return builder.Append( prefix ).AppendJoin( ", ", values ).Append( suffix );
        }
        private static StringBuilder AppendValuesIfAny(this StringBuilder builder, string? prefix, string? suffix, IEnumerable<string> values) {
            values = values.ToList();
            if (!values.Any()) return builder;
            return builder.Append( prefix ).AppendJoin( ", ", values ).Append( suffix );
        }
        private static StringBuilder Space(this StringBuilder builder) {
            return builder.Append( ' ' );
        }
        // Helpers/Enumerable
        private static IEnumerable<string> Concat(string? @base, IEnumerable<string>? interfaces) {
            var result = Enumerable.Empty<string>();
            if (@base != null) result = result.Append( @base );
            if (interfaces != null) result = result.Concat( interfaces );
            return result;
        }
        // Helpers/Type
        private static IEnumerable<string> GetKeywords(this Type type) {
            if (!type.IsNested) {
                if (type.IsPublic) yield return "public"; else yield return "internal";
            } else {
                if (type.IsNestedPublic) yield return "public";
                if (type.IsNestedAssembly) yield return "internal";
                if (type.IsNestedFamily) yield return "protected";
                if (type.IsNestedFamORAssem) yield return "protected internal"; // Can be accessed by any code in the assembly in which it's declared, or from within a derived class in another assembly.
                if (type.IsNestedFamANDAssem) yield return "private protected"; // Can be accessed only within its declaring assembly, by code in the same class or in a type that is derived from that class.
                if (type.IsNestedPrivate) yield return "private";
            }

            if (type.IsClass()) {
                if (type.IsStatic()) yield return "static";
                if (type.IsAbstract()) yield return "abstract";
                if (type.IsSealed()) yield return "sealed";
            }

            if (type.IsInterface()) yield return "interface";
            if (type.IsClass()) yield return "class";
            if (type.IsStruct()) yield return "struct";
            if (type.IsEnum()) yield return "enum";
            if (type.IsDelegate()) yield return "delegate";
        }
        private static IEnumerable<string> GetKeywords_GenericParameter(this Type type) {
            if (type.IsContravariant()) yield return "in";
            if (type.IsCovariant()) yield return "out";
        }


    }
}