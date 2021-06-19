﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace System.Collections.Generic {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;

    internal static class EnumerableExtensions {
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
        public static IEnumerable<(T? Key, IReadOnlyList<T> Children)> Unflatten<T>(this IEnumerable<T> source, Predicate<T> isKey) {
            var source_enumerator = source.GetPeekableEnumerator();
            var children = new List<T>();
            while (source_enumerator.PeekNext()) {
                source_enumerator.TakeIf( isKey, out var key );
                children.Clear();
                children.AddRange( source_enumerator.TakeUntil( isKey ) );
                yield return (key, children);
            }
        }
        public static IEnumerable<IReadOnlyList<T>> SplitByFirst<T>(this IEnumerable<T> source, Predicate<T> isFirst) {
            var source_enumerator = source.GetPeekableEnumerator();
            var slice = new List<T>();
            while (source_enumerator.PeekNext()) {
                slice.Clear();
                slice.AddRange( source_enumerator.TakeSliceByFirst( isFirst ) );
                yield return slice;
            }
        }
        public static IEnumerable<IReadOnlyList<T>> SplitByLast<T>(this IEnumerable<T> source, Predicate<T> isLast) {
            var source_enumerator = source.GetPeekableEnumerator();
            var slice = new List<T>();
            while (source_enumerator.PeekNext()) {
                slice.Clear();
                slice.AddRange( source_enumerator.TakeSliceByLast( isLast ) );
                yield return slice;
            }
        }
        public static IEnumerable<T> Append<T>(this T element, T element2) {
            return Enumerable.Empty<T>().Append( element ).Append( element2 );
        }
        public static IEnumerable<T> Append<T>(this T element, IEnumerable<T> elements) {
            return Enumerable.Empty<T>().Append( element ).Concat( elements );
        }
        public static IEnumerable<T> AsEnumerable<T>(this T element) {
            return Enumerable.Empty<T>().Append( element );
        }
        public static PeekableEnumerator<T> GetPeekableEnumerator<T>(this IEnumerable<T> enumerable) {
            return new PeekableEnumerator<T>( enumerable.GetEnumerator() );
        }
    }
    internal static class PeekableEnumeratorExtensions {
        public static IEnumerable<T> TakeWhile<T>(this PeekableEnumerator<T> enumerator, Predicate<T> predicate) {
            // true, true, [break], false
            while (enumerator.TakeIf( predicate, out var value )) {
                yield return value;
            }
        }
        public static IEnumerable<T> TakeUntil<T>(this PeekableEnumerator<T> enumerator, Predicate<T> predicate) {
            // false, false, [break], true
            while (enumerator.TakeIfNot( predicate, out var value )) {
                yield return value;
            }
        }
        public static IEnumerable<T> TakeSliceByFirst<T>(this PeekableEnumerator<T> enumerator, Predicate<T> isFirst) {
            // first, last, [break], first
            while (enumerator.MoveNext( out var curr )) {
                yield return curr;
                if (enumerator.PeekNext( out var next ) && isFirst( next )) yield break;
            }
        }
        public static IEnumerable<T> TakeSliceByLast<T>(this PeekableEnumerator<T> enumerator, Predicate<T> isLast) {
            // first, last, [break]
            while (enumerator.MoveNext( out var curr )) {
                yield return curr;
                if (isLast( curr )) yield break;
            }
        }
        public static bool TakeIf<T>(this PeekableEnumerator<T> enumerator, Predicate<T> predicate, [MaybeNullWhen( false )] out T value) {
            if (enumerator.PeekNext( out var next ) && predicate( next )) {
                return enumerator.MoveNext( out value );
            }
            value = default;
            return false;
        }
        public static bool TakeIfNot<T>(this PeekableEnumerator<T> enumerator, Predicate<T> predicate, [MaybeNullWhen( false )] out T value) {
            if (enumerator.PeekNext( out var next ) && !predicate( next )) {
                return enumerator.MoveNext( out value );
            }
            value = default;
            return false;
        }
    }
    internal class PeekableEnumerator<T> : IEnumerator<T> {
        private enum State_ { Uninitialized, Started, Finished }

        private IEnumerator<T> Source { get; }
        private State_ State { get; set; }
        T IEnumerator<T>.Current => Current.Value;
        object? IEnumerator.Current => Current.Value;

        public bool IsStarted => State == State_.Started;
        public bool IsFinished => State == State_.Finished;
        public Option<T> Current { get; private set; }
        public Option<T> Next { get; private set; }

        public PeekableEnumerator(IEnumerator<T> source) {
            Source = source ?? throw new ArgumentNullException( nameof( source ) );
            State = State_.Uninitialized;
            Current = default;
            Next = default;
        }
        public void Dispose() {
            Source.Dispose();
        }

        // MoveNext
        public bool MoveNext() {
            Current = MoveNext( Next, Source );
            Next = default;
            State = Current.HasValue ? State_.Started : State_.Finished;
            return Current.HasValue;
        }
        public bool MoveNext([MaybeNullWhen( false )] out T value) {
            if (MoveNext()) {
                value = Current.Value;
                return true;
            } else {
                value = default;
                return false;
            }
        }
        // PeekNext
        public bool PeekNext() {
            Next = MoveNext( Next, Source );
            return Next.HasValue;
        }
        public bool PeekNext([MaybeNullWhen( false )] out T value) {
            if (PeekNext()) {
                value = Next.Value;
                return true;
            } else {
                value = default;
                return false;
            }
        }
        // Reset
        public void Reset() {
            Source.Reset();
            State = State_.Uninitialized;
            Current = default;
            Next = default;
        }

        // Helpers
        private static Option<T> MoveNext(Option<T> next, IEnumerator<T> enumerator) {
            return next.HasValue ? next.Value : MoveNext( enumerator );
        }
        private static Option<T> MoveNext(IEnumerator<T> enumerator) {
            var hasValue = enumerator.MoveNext();
            return hasValue ? (Option<T>) enumerator.Current : default;
        }
    }
}
