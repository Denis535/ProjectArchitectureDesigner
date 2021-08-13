// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace System.Collections.Generic {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;

    public static class EnumerableExtensions {


        // Compare
        public static void Compare<T>(this IEnumerable<T> actual, IEnumerable<T> expected, out List<T> missing, out List<T> extra) {
            var expected_ = new LinkedList<T>( expected );
            extra = actual.Where( i => !expected_.Remove( i ) ).ToList();
            missing = expected_.ToList();
        }

        // With
        public static IEnumerable<T> WithPrefix<T>(this IEnumerable<T> source, T prefix) {
            foreach (var item in source) {
                yield return prefix;
                yield return item;
            }
        }
        public static IEnumerable<T> WithSuffix<T>(this IEnumerable<T> source, T suffix) {
            foreach (var item in source) {
                yield return item;
                yield return suffix;
            }
        }
        public static IEnumerable<T> WithPrefixSuffix<T>(this IEnumerable<T> source, T prefix, T suffix) {
            foreach (var item in source) {
                yield return prefix;
                yield return item;
                yield return suffix;
            }
        }
        public static IEnumerable<T> WithSeparator<T>(this IEnumerable<T> source, T separator) {
            using var source_enumerator = source.GetPeekableEnumerator();
            while (source_enumerator.TryTake( out var current )) {
                yield return current;
                if (source_enumerator.HasNext) yield return separator;
            }
        }

        // With
        public static IEnumerable<(T Current, Option<T> Prev)> WithPrevious<T>(this IEnumerable<T> source) {
            using var source_enumerator = source.GetEnumerator();
            var prev = default( Option<T> );
            while (source_enumerator.TryTake( out var current )) {
                yield return (current, prev);
                prev = current;
            }
        }
        public static IEnumerable<(T Current, Option<T> Next)> WithNext<T>(this IEnumerable<T> source) {
            using var source_enumerator = source.GetPeekableEnumerator();
            while (source_enumerator.TryTake( out var current )) {
                yield return (current, source_enumerator.Peek());
            }
        }

        // Tag
        public static IEnumerable<(T Value, bool IsFirst)> TagFirst<T>(this IEnumerable<T> source) {
            using var source_enumerator = source.GetEnumerator();
            if (source_enumerator.TryTake( out var first )) {
                yield return (first, true);
            }
            while (source_enumerator.TryTake( out var value )) {
                yield return (value, false);
            }
        }
        public static IEnumerable<(T Value, bool IsLast)> TagLast<T>(this IEnumerable<T> source) {
            using var source_enumerator = source.GetPeekableEnumerator();
            while (source_enumerator.HasNext && source_enumerator.TryTake( out var value )) {
                yield return (value, false);
            }
            if (source_enumerator.TryTake( out var last )) {
                yield return (last, true);
            }
        }

        // Unflatten
        public static IEnumerable<(Option<T> Key, T[] Values)> Unflatten<T>(this IEnumerable<T> source, Predicate<T> predicate) {
            var source_enumerator = source.GetPeekableEnumerator();
            var values = new List<T>();
            while (source_enumerator.HasNext) {
                var key = source_enumerator.TakeIf( predicate );
                values.Clear();
                values.AddRange( source_enumerator.TakeUntil( predicate ) );
                yield return (key, values.ToArray());
            }
        }

        // Split
        public static IEnumerable<T[]> SplitByFirst<T>(this IEnumerable<T> source, Predicate<T> predicate) {
            var source_enumerator = source.GetPeekableEnumerator();
            var slice = new List<T>();
            while (source_enumerator.HasNext) {
                slice.Clear();
                slice.AddRange( source_enumerator.TakeSliceByFirst( predicate ) );
                yield return slice.ToArray();
            }
        }
        public static IEnumerable<T[]> SplitByLast<T>(this IEnumerable<T> source, Predicate<T> predicate) {
            var source_enumerator = source.GetPeekableEnumerator();
            var slice = new List<T>();
            while (source_enumerator.HasNext) {
                slice.Clear();
                slice.AddRange( source_enumerator.TakeSliceByLast( predicate ) );
                yield return slice.ToArray();
            }
        }
        public static IEnumerable<T[]> SplitByLast<T>(this IEnumerable<T> source, Func<T, T, bool> predicate) {
            var source_enumerator = source.GetPeekableEnumerator();
            var slice = new List<T>();
            while (source_enumerator.HasNext) {
                slice.Clear();
                slice.AddRange( source_enumerator.TakeSliceByLast( predicate ) );
                yield return slice.ToArray();
            }
        }

        // Take
        public static bool TryTake<T>(this IEnumerator<T> enumerator, [MaybeNullWhen( false )] out T current) {
            return enumerator.Take().TryGetValue( out current );
        }
        public static Option<T> Take<T>(this IEnumerator<T> enumerator) {
            if (enumerator.MoveNext()) return enumerator.Current;
            return default;
        }

        // Concat
        public static IEnumerable<T> Concat<T>(this T first, T second) {
            return new[] { first, second };
        }
        // Append
        public static IEnumerable<T> Append<T>(this T first, IEnumerable<T> seconds) {
            return seconds.Prepend( first );
        }
        // ToEnumerable
        public static IEnumerable<T> ToEnumerable<T>(this T element) {
            return new[] { element };
        }
        // Where/Not
        public static IEnumerable<T> WhereNot<T>(this IEnumerable<T> source, Func<T, bool> predicate) {
            return source.Where( i => !predicate( i ) );
        }


    }
}
