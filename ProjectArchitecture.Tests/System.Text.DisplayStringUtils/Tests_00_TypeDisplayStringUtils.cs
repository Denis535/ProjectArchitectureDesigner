// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace System.Text.DisplayStringUtils {
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    public class Tests_00_TypeDisplayStringUtils {

        public interface IInterface<in T1, out T2>
            where T1 : class, IDisposable, new()
            where T2 : struct {
        }

        public interface ISimpleInterface<in T1, out T2> : IInterface<T1, T2>
            where T1 : class, IDisposable, new()
            where T2 : struct {
        }

        public class SimpleClass<T1, T2> : object, IInterface<T1, T2>
            where T1 : class, IDisposable, new()
            where T2 : struct {
        }

        public record SimpleRecord<T1, T2>(object Value1, object Value2, object Value3) : object, IInterface<T1, T2>
            where T1 : class, IDisposable, new()
            where T2 : struct {
        }

        public struct SimpleStruct<T1, T2> : IInterface<T1, T2>
            where T1 : class, IDisposable, new()
            where T2 : struct {
        }

        public enum SimpleEnum {
            V1, V2, V3
        }

        public delegate object SimpleDelegate<in T1, out T2>(object value)
            where T1 : class, IDisposable, new()
            where T2 : struct;


        [Test]
        public void Test_00_GetDisplayString() {
            TestContext.WriteLine( TypeDisplayStringUtils.GetDisplayString( typeof( ISimpleInterface<,> ) ) );
            TestContext.WriteLine( TypeDisplayStringUtils.GetDisplayString( typeof( SimpleClass<,> ) ) );
            TestContext.WriteLine( TypeDisplayStringUtils.GetDisplayString( typeof( SimpleRecord<,> ) ) );
            TestContext.WriteLine( TypeDisplayStringUtils.GetDisplayString( typeof( SimpleStruct<,> ) ) );
            TestContext.WriteLine( TypeDisplayStringUtils.GetDisplayString( typeof( SimpleEnum ) ) );
            TestContext.WriteLine( TypeDisplayStringUtils.GetDisplayString( typeof( SimpleDelegate<,> ) ) );
        }


    }
}