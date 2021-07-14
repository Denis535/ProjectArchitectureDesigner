// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

#pragma warning disable CS0067 // The event is never used
namespace System.Text.CSharp {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using NUnit.Framework;
    
    public class Tests_01_MemberSyntaxFactory {

        public abstract class SimpleClass<T> where T : class {
            public const object? Field = default;
            public object? Property { get; set; }
            public event Action? Event;
            public SimpleClass(T arg) {
            }
            public T1? Method<T1, T2>(T arg, T1? arg1, T2? arg2) where T1 : class, IDisposable, new() where T2 : struct {
                return default;
            }
        }


        [Test]
        public void Test_00_MemberSyntax() {
            TestContext.WriteLine( typeof( SimpleClass<> ).GetTypeInfo().DeclaredFields.Single( IsUserDefined ).GetFieldSyntax() );
            TestContext.WriteLine( typeof( SimpleClass<> ).GetTypeInfo().DeclaredProperties.Single( IsUserDefined ).GetPropertySyntax() );
            TestContext.WriteLine( typeof( SimpleClass<> ).GetTypeInfo().DeclaredEvents.Single( IsUserDefined ).GetEventSyntax() );
            TestContext.WriteLine( typeof( SimpleClass<> ).GetTypeInfo().DeclaredConstructors.Single().GetConstructorSyntax() );
            TestContext.WriteLine( typeof( SimpleClass<> ).GetTypeInfo().DeclaredMethods.Single( IsUserDefined ).GetMethodSyntax() );
        }


        // Helpers
        private static bool IsUserDefined(MemberInfo member) {
            return !member.IsSpecialName() && !member.IsDefined( typeof( CompilerGeneratedAttribute ) );
        }


    }
}