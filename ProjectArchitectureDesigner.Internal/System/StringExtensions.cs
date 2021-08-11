// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace System {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;

    public static class StringExtensions {


        public static string WithoutPrefix(this string value, string prefix) {
            // prefix[i]value
            if (value.StartsWith( prefix )) return value.Remove( 0, prefix.Length ); // remove prefix
            return value;
        }
        public static string WithoutSuffix(this string value, string suffix) {
            // value[i]suffix
            if (value.EndsWith( suffix )) return value.Remove( value.Length - suffix.Length ); // remove suffix
            return value;
        }

        public static string? GetStringAfter(this string value, string prefix) {
            // ...prefix[i]value
            var i = value.IndexOf( prefix );
            if (i != -1) return value.Remove( 0, i + prefix.Length ); // remove prefix
            return null;
        }
        public static string? GetStringBefore(this string value, string suffix) {
            // value[i]suffix...
            var i = value.LastIndexOf( suffix );
            if (i != -1) return value.Remove( i ); // remove suffix
            return null;
        }

        public static string Join(this IEnumerable<string> values, string separator = ", ") {
            return string.Join( separator, values );
        }
        public static string Join<T>(this IEnumerable<T> values, Func<T, string> selector, string separator = ", ") {
            return string.Join( separator, values.Select( selector ) );
        }

        public static string Format(this string format, params object?[] args) {
            return string.Format( format, args );
        }

        public static bool IsEmpty([NotNullWhen( false )] this string? value) {
            return string.IsNullOrEmpty( value );
        }
        public static bool IsNotEmpty([NotNullWhen( true )] this string? value) {
            return !string.IsNullOrEmpty( value );
        }


    }
}
