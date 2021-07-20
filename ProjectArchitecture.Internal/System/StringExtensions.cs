// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace System {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class StringExtensions {


        public static string Format(this string format, params object?[] args) {
            return string.Format( format, args );
        }
        public static string Join(this IEnumerable<string> values, string separator = ", ") {
            return string.Join( separator, values );
        }
        public static string Join<T>(this IEnumerable<T> values, Func<T, string> selector, string separator = ", ") {
            return string.Join( separator, values.Select( selector ) );
        }
        public static bool IsEmpty(this string? value) {
            return string.IsNullOrEmpty( value );
        }
        public static bool IsNotEmpty(this string? value) {
            return !string.IsNullOrEmpty( value );
        }


    }
}
