// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

#pragma warning disable CS0067 // The event is never used
namespace System.Text.CSharp {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using NUnit.Framework;

    public class Tests_01_MemberSyntaxFactory {

        public abstract class ExampleClass<T> {
            public object? Field = default;
            public object? Property { get; set; }
            public object? this[ int ind1, int ind2 ] { get { return default; } set { } }
            public event Action? Event { add { } remove { } }
            public ExampleClass(T arg) {
            }
            public T1? Method<T1, T2>(T arg, T1? arg1, T2? arg2) where T1 : class, IDisposable, new() where T2 : struct {
                return default;
            }
        }


        [Test]
        public void Test_00_GetMemberSyntax() {
            TestContext.WriteLine( typeof( ExampleClass<> ).GetTypeInfo().GetDeclaredField( "Field" )!.GetFieldSyntax() );
            TestContext.WriteLine( typeof( ExampleClass<> ).GetTypeInfo().GetDeclaredProperty( "Property" )!.GetPropertySyntax() );
            TestContext.WriteLine( typeof( ExampleClass<> ).GetTypeInfo().GetDeclaredProperty( "Item" )!.GetPropertySyntax() );
            TestContext.WriteLine( typeof( ExampleClass<> ).GetTypeInfo().GetDeclaredEvent( "Event" )!.GetEventSyntax() );
            TestContext.WriteLine( typeof( ExampleClass<> ).GetTypeInfo().DeclaredConstructors.Single().GetConstructorSyntax() );
            TestContext.WriteLine( typeof( ExampleClass<> ).GetTypeInfo().GetDeclaredMethod( "Method" )!.GetMethodSyntax() );
        }


    }
}