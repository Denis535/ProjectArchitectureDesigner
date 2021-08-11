// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace System.Collections.Generic {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    public static class PeekableEnumeratorExtensions {
        // GetPeekableEnumerator
        public static PeekableEnumerator<T> GetPeekableEnumerator<T>(this IEnumerable<T> enumerable) {
            return new PeekableEnumerator<T>( enumerable.GetEnumerator() );
        }
        // Take/While
        public static IEnumerable<T> TakeWhile<T>(this PeekableEnumerator<T> enumerator, Predicate<T> predicate) {
            // true, true, [break], false
            while (enumerator.TryTakeIf( predicate, out var current )) {
                yield return current;
            }
        }
        // Take/Until
        public static IEnumerable<T> TakeUntil<T>(this PeekableEnumerator<T> enumerator, Predicate<T> predicate) {
            // false, false, [break], true
            while (enumerator.TryTakeIfNot( predicate, out var current )) {
                yield return current;
            }
        }
        // Take/Slice
        public static IEnumerable<T> TakeSliceByFirst<T>(this PeekableEnumerator<T> enumerator, Predicate<T> predicate) {
            // first, last, [break], first
            while (enumerator.TryTake( out var current )) {
                yield return current;
                if (enumerator.TryPeek( out var next ) && predicate( next )) yield break;
            }
        }
        public static IEnumerable<T> TakeSliceByLast<T>(this PeekableEnumerator<T> enumerator, Predicate<T> predicate) {
            // first, last, [break], first
            while (enumerator.TryTake( out var current )) {
                yield return current;
                if (predicate( current )) yield break;
            }
        }
        public static IEnumerable<T> TakeSliceByLast<T>(this PeekableEnumerator<T> enumerator, Func<T, T, bool> predicate) {
            // aaa [break] bbb
            while (enumerator.TryTake( out var current )) {
                yield return current;
                if (enumerator.TryPeek( out var next ) && predicate( current, next )) yield break;
            }
        }
        // Take/If
        public static bool TryTakeIf<T>(this PeekableEnumerator<T> enumerator, Predicate<T> predicate, [MaybeNullWhen( false )] out T current) {
            return enumerator.TakeIf( predicate ).TryGetValue( out current );
        }
        public static Option<T> TakeIf<T>(this PeekableEnumerator<T> enumerator, Predicate<T> predicate) {
            if (enumerator.TryPeek( out var next ) && predicate( next )) return enumerator.Take();
            return default;
        }
        // Take/If/Not
        public static bool TryTakeIfNot<T>(this PeekableEnumerator<T> enumerator, Predicate<T> predicate, [MaybeNullWhen( false )] out T current) {
            return enumerator.TakeIfNot( predicate ).TryGetValue( out current );
        }
        public static Option<T> TakeIfNot<T>(this PeekableEnumerator<T> enumerator, Predicate<T> predicate) {
            if (enumerator.TryPeek( out var next ) && !predicate( next )) return enumerator.Take();
            return default;
        }
    }
    public class PeekableEnumerator<T> : IEnumerator<T> {
        private enum State_ { Uninitialized, Started, Finished }

        private IEnumerator<T> Source { get; }
        private State_ State { get; set; }
        private Option<T> Current { get; set; }
        private Option<T> Next { get; set; }

        public bool IsStarted => State == State_.Started;
        public bool IsFinished => State == State_.Finished;
        public bool HasNext => PeekInternal().HasValue;


        public PeekableEnumerator(IEnumerator<T> source) {
            Source = source ?? throw new ArgumentNullException( nameof( source ) );
            State = State_.Uninitialized;
            Current = default;
            Next = default;
        }
        public void Dispose() {
            Source.Dispose();
        }


        // IEnumerator
        T IEnumerator<T>.Current => Current.Value;
        object? IEnumerator.Current => Current.Value;
        bool IEnumerator.MoveNext() => TakeInternal().HasValue;


        // Take
        public bool TryTake([MaybeNullWhen( false )] out T current) {
            return TakeInternal().TryGetValue( out current );
        }
        public Option<T> Take() {
            return TakeInternal();
        }
        private Option<T> TakeInternal() {
            State = State_.Started;
            Current = GetNext( Source, Next );
            Next = default;
            State = Current.HasValue ? State_.Started : State_.Finished;
            return Current;
        }
        // Peek
        public bool TryPeek([MaybeNullWhen( false )] out T next) {
            return PeekInternal().TryGetValue( out next );
        }
        public Option<T> Peek() {
            return PeekInternal();
        }
        private Option<T> PeekInternal() {
            Next = GetNext( Source, Next );
            return Next;
        }
        // Reset
        public void Reset() {
            Source.Reset();
            State = State_.Uninitialized;
            Current = default;
            Next = default;
        }


        // Helpers
        private static Option<T> GetNext(IEnumerator<T> enumerator, Option<T> next) {
            if (next.HasValue) return next;
            if (enumerator.MoveNext()) return enumerator.Current;
            return default;
        }


    }
}
