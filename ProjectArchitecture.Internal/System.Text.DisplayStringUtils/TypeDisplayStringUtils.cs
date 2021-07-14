﻿// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

namespace System.Text.DisplayStringUtils {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    // public interface Interface<in/out T> : IInterface<T> where T : class, new()
    // public class     Class<T>            : Base<T>, IInterface<T> where T : class, new()
    // public struct    Struct<T>           : IInterface<T> where T : class, new()
    // public enum      Enum                 : int
    // public delegate  T Delegate<in/out T>(T value) where T : class, new()
    public static class TypeDisplayStringUtils {


        public static string GetDisplayString(Type type) {
            if (type.IsInterface()) {
                return CSharpSyntaxUtils.GetTypeDeclaration( type.GetKeywords(), type, type.GetGenericArguments(), null, type.GetInterfaces() );
            }
            if (type.IsClass()) {
                return CSharpSyntaxUtils.GetTypeDeclaration( type.GetKeywords(), type, type.GetGenericArguments(), type.BaseType, type.GetInterfaces() );
            }
            if (type.IsStruct()) {
                return CSharpSyntaxUtils.GetTypeDeclaration( type.GetKeywords(), type, type.GetGenericArguments(), null, type.GetInterfaces() );
            }
            if (type.IsEnum()) {
                return CSharpSyntaxUtils.GetTypeDeclaration( type.GetKeywords(), type, type.GetGenericArguments(), Enum.GetUnderlyingType( type ), null );
            }
            if (type.IsDelegate()) {
                var method = type.GetMethod( "Invoke" );
                return CSharpSyntaxUtils.GetDelegateDeclaration( type.GetKeywords(), method.ReturnParameter, type, type.GetGenericArguments(), method.GetParameters() );
            }
            throw new ArgumentException( "Type is unsupported: " + type );
        }


    }
}