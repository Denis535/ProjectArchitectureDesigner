// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace System.Runtime.CompilerServices {
    using System.ComponentModel;
    [EditorBrowsable( EditorBrowsableState.Never )]
    internal class IsExternalInit {
    }
}
namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;

    internal static class Utils {


        // String
        public static string WithoutPrefix(this string value, string prefix) {
            var i = value.IndexOf( prefix );
            if (i != -1) value = value.Substring( i + prefix.Length );
            return value;
        }
        public static string Format(this string format, params string?[] args) {
            return string.Format( format, args );
        }
        public static string Join(this IEnumerable<string> values, string separator = ", ") {
            return string.Join( separator, values );
        }
        public static string Join<T>(this IEnumerable<T> values, Func<T, string> selector, string separator = ", ") {
            return string.Join( separator, values.Select( selector ) );
        }


        // Type
        public static bool IsObsolete(this Type? type) {
            while (type != null) {
                if (type.IsDefined( typeof( ObsoleteAttribute ) )) return true;
                type = type.DeclaringType;
            }
            return false;
        }
        public static bool IsCompilerGenerated(this Type? type) {
            while (type != null) {
                if (type.IsDefined( typeof( CompilerGeneratedAttribute ) )) return true;
                type = type.DeclaringType;
            }
            return false;
        }


        // System
        [EditorBrowsable( EditorBrowsableState.Never )]
        public static TResult Map<TSource, TResult>(this TSource source, Func<TSource, TResult> selector) {
            return selector( source );
        }


    }
}