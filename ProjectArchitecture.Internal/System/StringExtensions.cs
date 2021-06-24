namespace System {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class StringExtensions {


        public static string WithoutPrefix(this string value, string prefix) {
            if (value.StartsWith( prefix )) return value.Substring( prefix.Length );
            return value;
        }
        public static string Format(this string format, params object?[] args) {
            return string.Format( format, args );
        }
        public static string Join(this IEnumerable<string> values, string separator = ", ") {
            return string.Join( separator, values );
        }
        public static string Join<T>(this IEnumerable<T> values, Func<T, string> selector, string separator = ", ") {
            return string.Join( separator, values.Select( selector ) );
        }
        public static bool IsEmpty(this string value) {
            return string.IsNullOrEmpty( value );
        }
        public static bool IsNotEmpty(this string value) {
            return !string.IsNullOrEmpty( value );
        }


    }
}
