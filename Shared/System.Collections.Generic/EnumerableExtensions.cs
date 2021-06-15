// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
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
            var (hasKey, key) = (false, default( T ));
            var children = new List<T>();
            foreach (var item in source) {
                if (isKey( item )) {
                    if (hasKey || children.Any()) yield return (key, children);
                    (hasKey, key) = (true, item); // new key of new node
                    children.Clear();
                } else {
                    children.Add( item );
                }
            }
            if (hasKey || children.Any()) yield return (key, children);
        }
        public static IEnumerable<IEnumerable<T>> SplitByLast<T>(this IEnumerable<T> source, Predicate<T> isLast) {
            var source_enumerator = source.GetPeekableEnumerator();
            while (source_enumerator.PeekNext()) {
                yield return source_enumerator.TakeSliceByLast( isLast );
            }
        }
        private static IEnumerable<T> TakeSliceByLast<T>(this IEnumerator<T> enumerator, Predicate<T> isLast) {
            while (enumerator.MoveNext()) {
                yield return enumerator.Current;
                if (isLast( enumerator.Current )) yield break;
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
