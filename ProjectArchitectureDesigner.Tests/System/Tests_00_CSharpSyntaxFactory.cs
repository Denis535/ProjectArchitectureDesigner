// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace System {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.CSharp;
    using NUnit.Framework;

    public class Tests_00_CSharpSyntaxFactory {

        private interface IInterface<in T1, out T2>
            where T1 : class, IDisposable, new()
            where T2 : struct {
        }

        private interface IInterfaceExample<in T1, out T2> : IInterface<T1, T2>
            where T1 : class, IDisposable, new()
            where T2 : struct {
        }

        private class ClassExample<T1, T2> : object, IInterface<T1, T2>
            where T1 : class, IDisposable, new()
            where T2 : struct {
        }

        private record RecordExample<T1, T2>() : object, IInterface<T1, T2>
            where T1 : class, IDisposable, new()
            where T2 : struct {
        }

        private struct StructExample<T1, T2> : IInterface<T1, T2>
            where T1 : class, IDisposable, new()
            where T2 : struct {
        }

        private enum EnumExample {
            Value1, Value2, Value3
        }

        private delegate object DelegateExample<in T1, out T2>(object value)
            where T1 : class, IDisposable, new()
            where T2 : struct;

        private abstract class ClassWithMembersExample {
            public object? Field = default;
            public object? Property { get; set; }
            public object? this[ int ind1, int ind2 ] { get { return null; } set { } }
            public event Action? Event { add { } remove { } }
            public ClassWithMembersExample(object arg) {
            }
            public T1? Method<T1, T2>(object arg, T1? arg1, T2? arg2) where T1 : class, IDisposable, new() where T2 : struct {
                return null;
            }
        }


        [Test]
        public void Test_00_GetTypeSyntax() {
            TestContext.WriteLine( typeof( IInterfaceExample<,> ).GetTypeSyntax() );
            TestContext.WriteLine( typeof( ClassExample<,> ).GetTypeSyntax() );
            TestContext.WriteLine( typeof( RecordExample<,> ).GetTypeSyntax() );
            TestContext.WriteLine( typeof( StructExample<,> ).GetTypeSyntax() );
            TestContext.WriteLine( typeof( EnumExample ).GetTypeSyntax() );
            TestContext.WriteLine( typeof( DelegateExample<,> ).GetTypeSyntax() );
        }
        [Test]
        public void Test_01_GetMemberSyntax() {
            TestContext.WriteLine( typeof( ClassWithMembersExample ).GetTypeInfo().GetDeclaredField( "Field" )!.GetFieldSyntax() );
            TestContext.WriteLine( typeof( ClassWithMembersExample ).GetTypeInfo().GetDeclaredProperty( "Property" )!.GetPropertySyntax() );
            TestContext.WriteLine( typeof( ClassWithMembersExample ).GetTypeInfo().GetDeclaredProperty( "Item" )!.GetPropertySyntax() );
            TestContext.WriteLine( typeof( ClassWithMembersExample ).GetTypeInfo().GetDeclaredEvent( "Event" )!.GetEventSyntax() );
            TestContext.WriteLine( typeof( ClassWithMembersExample ).GetTypeInfo().DeclaredConstructors.Single().GetConstructorSyntax() );
            TestContext.WriteLine( typeof( ClassWithMembersExample ).GetTypeInfo().GetDeclaredMethod( "Method" )!.GetMethodSyntax() );
        }


    }
}