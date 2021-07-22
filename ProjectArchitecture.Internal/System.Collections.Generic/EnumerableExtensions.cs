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

        // With
        public static IEnumerable<(T Current, Option<T> Prev)> WithPrevious<T>(this IEnumerable<T> source) {
            using var source_enumerator = source.GetPeekableEnumerator();
            var prev = default( Option<T> );
            while (source_enumerator.MoveNext( out var current )) {
                yield return (current, prev);
                prev = current;
            }
        }
        public static IEnumerable<(T Current, Option<T> Next)> WithNext<T>(this IEnumerable<T> source) {
            using var source_enumerator = source.GetPeekableEnumerator();
            while (source_enumerator.MoveNext( out var current )) {
                if (source_enumerator.PeekNext( out var next )) {
                    yield return (current, next);
                } else {
                    yield return (current, default);
                }
            }
        }
        public static IEnumerable<T> WithSeparator<T>(this IEnumerable<T> source, T separator) {
            using var source_enumerator = source.GetPeekableEnumerator();
            while (source_enumerator.MoveNext( out var current )) {
                yield return current;
                if (source_enumerator.HasNext()) yield return separator;
            }
        }

        // Tag
        public static IEnumerable<(T Value, bool IsFirst)> TagFirst<T>(this IEnumerable<T> source) {
            using var source_enumerator = source.GetEnumerator();
            if (source_enumerator.MoveNext( out var first )) {
                yield return (first, true);
            }
            while (source_enumerator.MoveNext( out var value )) {
                yield return (value, false);
            }
        }
        public static IEnumerable<(T Value, bool IsLast)> TagLast<T>(this IEnumerable<T> source) {
            using var source_enumerator = source.GetPeekableEnumerator();
            while (source_enumerator.MoveNext( out var value )) {
                var isLast = !source_enumerator.HasNext();
                yield return (value, isLast);
            }
        }

        // Unflatten
        public static IEnumerable<(T? Key, IReadOnlyList<T> Children)> Unflatten<T>(this IEnumerable<T> source, Predicate<T> isKey) {
            var source_enumerator = source.GetPeekableEnumerator();
            var children = new List<T>();
            while (source_enumerator.HasNext()) {
                source_enumerator.TakeIf( isKey, out var key );
                children.Clear();
                children.AddRange( source_enumerator.TakeUntil( isKey ) );
                yield return (key, children);
            }
        }

        // Split
        public static IEnumerable<IReadOnlyList<T>> SplitByFirst<T>(this IEnumerable<T> source, Predicate<T> isFirst) {
            var source_enumerator = source.GetPeekableEnumerator();
            var slice = new List<T>();
            while (source_enumerator.HasNext()) {
                slice.Clear();
                slice.AddRange( source_enumerator.TakeSliceByFirst( isFirst ) );
                yield return slice;
            }
        }
        public static IEnumerable<IReadOnlyList<T>> SplitByLast<T>(this IEnumerable<T> source, Predicate<T> isLast) {
            var source_enumerator = source.GetPeekableEnumerator();
            var slice = new List<T>();
            while (source_enumerator.HasNext()) {
                slice.Clear();
                slice.AddRange( source_enumerator.TakeSliceByLast( isLast ) );
                yield return slice;
            }
        }

        // WhereNot
        public static IEnumerable<T> WhereNot<T>(this IEnumerable<T> source, Func<T, bool> predicate) {
            return source.Where( i => !predicate( i ) );
        }

        // Append
        public static IEnumerable<T> Append<T>(this T source, T element) {
            return Enumerable.Empty<T>().Append( source ).Append( element );
        }
        public static IEnumerable<T> Append<T>(this T source, IEnumerable<T> elements) {
            return Enumerable.Empty<T>().Append( source ).Concat( elements );
        }
        public static IEnumerable<T> AsEnumerable<T>(this T source) {
            return Enumerable.Empty<T>().Append( source );
        }

        // GetPeekableEnumerator
        public static PeekableEnumerator<T> GetPeekableEnumerator<T>(this IEnumerable<T> enumerable) {
            return new PeekableEnumerator<T>( enumerable.GetEnumerator() );
        }

        // MoveNext
        //public static Option<T> MoveNext<T>(this IEnumerator<T> enumerator) {
        //    if (enumerator.MoveNext()) {
        //        return enumerator.Current;
        //    } else {
        //        return default;
        //    }
        //}
        public static bool MoveNext<T>(this IEnumerator<T> enumerator, [MaybeNullWhen( false )] out T value) {
            if (enumerator.MoveNext()) {
                value = enumerator.Current;
                return true;
            } else {
                value = default;
                return false;
            }
        }


    }
}
