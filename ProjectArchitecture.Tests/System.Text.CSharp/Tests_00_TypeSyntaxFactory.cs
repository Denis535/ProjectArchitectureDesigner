// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace System.Text.CSharp {
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    public class Tests_00_TypeSyntaxFactory {

        public interface IInterface<in T1, out T2>
            where T1 : class, IDisposable, new()
            where T2 : struct {
        }

        public interface IExampleInterface<in T1, out T2> : IInterface<T1, T2>
            where T1 : class, IDisposable, new()
            where T2 : struct {
        }

        public class ExampleClass<T1, T2> : object, IInterface<T1, T2>
            where T1 : class, IDisposable, new()
            where T2 : struct {
        }

        public record ExampleRecord<T1, T2>(object Value1, object Value2, object Value3) : object, IInterface<T1, T2>
            where T1 : class, IDisposable, new()
            where T2 : struct {
        }

        public struct ExampleStruct<T1, T2> : IInterface<T1, T2>
            where T1 : class, IDisposable, new()
            where T2 : struct {
        }

        public enum ExampleEnum {
            V1, V2, V3
        }

        public delegate object ExampleDelegate<in T1, out T2>(object value)
            where T1 : class, IDisposable, new()
            where T2 : struct;


        [Test]
        public void Test_00_TypeSyntax() {
            TestContext.WriteLine( typeof( IExampleInterface<,> ).GetTypeSyntax() );
            TestContext.WriteLine( typeof( ExampleClass<,> ).GetTypeSyntax() );
            TestContext.WriteLine( typeof( ExampleRecord<,> ).GetTypeSyntax() );
            TestContext.WriteLine( typeof( ExampleStruct<,> ).GetTypeSyntax() );
            TestContext.WriteLine( typeof( ExampleEnum ).GetTypeSyntax() );
            TestContext.WriteLine( typeof( ExampleDelegate<,> ).GetTypeSyntax() );
        }


    }
}