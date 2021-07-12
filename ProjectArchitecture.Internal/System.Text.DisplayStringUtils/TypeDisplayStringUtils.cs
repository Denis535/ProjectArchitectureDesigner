// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace System.Text.DisplayStringUtils {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    // public interface Interface<in/out T> : IInterface<T> where T : class, new()
    // public class     Class<T>            : Base<T>, IInterface<T> where T : class, new()
    // public struct    Struct<T>           : IInterface<T> where T : class, new()
    // public enum      Enum                 : int
    // public delegate  T Delegate<in/out T>(T value) where T : class, new()
    public static class TypeDisplayStringUtils {


        public static string GetDisplayString(Type type) {
            if (type.IsInterface()) {
                return GetTypeDeclaration( type.GetKeywords(), type, type.GetGenericArguments(), null, type.GetInterfaces() );
            }
            if (type.IsClass()) {
                return GetTypeDeclaration( type.GetKeywords(), type, type.GetGenericArguments(), type.BaseType, type.GetInterfaces() );
            }
            if (type.IsStruct()) {
                return GetTypeDeclaration( type.GetKeywords(), type, type.GetGenericArguments(), null, type.GetInterfaces() );
            }
            if (type.IsEnum()) {
                return GetTypeDeclaration( type.GetKeywords(), type, type.GetGenericArguments(), Enum.GetUnderlyingType( type ), null );
            }
            if (type.IsDelegate()) {
                var method = type.GetMethod( "Invoke" );
                return GetDelegateDeclaration( type.GetKeywords(), method.ReturnParameter, type, type.GetGenericArguments(), method.GetParameters() );
            }
            throw new ArgumentException( "Type is unsupported: " + type );
        }


        // Helpers/GetObjectDeclaration
        private static string GetTypeDeclaration(IEnumerable<string> keywords, Type type, Type[] generics, Type? @base, Type[]? interfaces) {
            var builder = new StringBuilder();
            builder.AppendKeywords( keywords );
            builder.AppendSimpleIdentifier( type );
            builder.AppendGenerics( generics );
            builder.AppendBaseTypeAndInterfaces( @base, interfaces );
            builder.AppendConstraints( generics );
            return builder.ToString();
        }
        private static string GetDelegateDeclaration(IEnumerable<string> keywords, ParameterInfo result, Type type, Type[] generics, ParameterInfo[] parameters) {
            var builder = new StringBuilder();
            builder.AppendKeywords( keywords );
            builder.AppendResult( result );
            builder.Append( ' ' );
            builder.AppendSimpleIdentifier( type );
            builder.AppendGenerics( generics );
            builder.AppendParameters( parameters );
            builder.AppendConstraints( generics );
            return builder.ToString();
        }


    }
}