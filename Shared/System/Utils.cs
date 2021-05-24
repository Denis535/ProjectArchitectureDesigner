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

    [EditorBrowsable( EditorBrowsableState.Never )]
    internal static class Utils {


        // IEnumerable
        public static void Compare<T>(IEnumerable<T> actual, IEnumerable<T> expected, out IList<T> intersected, out IList<T> missing, out IList<T> extra) {
            intersected = new List<T>();
            missing = new List<T>();
            extra = new List<T>();
            var expected_ = new LinkedList<T>( expected );
            foreach (var item in actual) {
                if (expected_.Remove( item )) {
                    intersected.Add( item );
                } else {
                    extra.Add( item );
                }
            }
            foreach (var item in expected_) {
                missing.Add( item );
            }
        }

        public static IEnumerable<(T? Key, T[] Children)> Unflatten<T>(this IEnumerable<T> source, Func<T, bool> predicate) {
            var key = default( T );
            var hasKey = false;
            var children = new List<T>();
            foreach (var item in source) {
                if (!predicate( item )) {
                    children.Add( item );
                } else {
                    if (hasKey || children.Any()) yield return (key, children.ToArray());
                    key = item;
                    hasKey = true;
                    children.Clear();
                }
            }
            if (hasKey || children.Any()) yield return (key, children.ToArray());
        }

        public static IEnumerable<T[]> Split<T>(this IEnumerable<T> source, Func<T, bool> predicate) {
            var slice = new List<T>();
            foreach (var item in source) {
                if (predicate( item )) {
                    if (slice.Any()) yield return slice.ToArray();
                    slice.Clear();
                }
                slice.Add( item );
            }
            if (slice.Any()) yield return slice.ToArray();
        }


        // Queue
        public static IEnumerable<T> TakeWhile<T>(this Queue<T> queue, Predicate<T> predicate) {
            while (queue.Count > 0) {
                if (predicate( queue.Peek() )) yield return queue.Dequeue(); else break;
            }
        }
        public static T? TakeIf<T>(this Queue<T> queue, Predicate<T> predicate) {
            if (queue.Count > 0) {
                if (predicate( queue.Peek() )) return queue.Dequeue();
            }
            return default;
        }


        // String
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
        public static TResult Map<TSource, TResult>(this TSource source, Func<TSource, TResult> selector) {
            return selector( source );
        }


    }
}
