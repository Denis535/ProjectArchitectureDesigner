// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace System {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    public static class CSharpExtensions {

        public static TResult Map<TSource, TResult>(this TSource source, Func<TSource, TResult> selector) {
            return selector( source );
        }

        public static Func<T, bool> AsFunc<T>(this Predicate<T> predicate) {
            return new Func<T, bool>( predicate );
        }

    }
    public sealed class DelegateDisposable : IDisposable {
        private readonly Action? @delegate;
        public DelegateDisposable(Action? @delegate) {
            this.@delegate = @delegate;
        }
        public void Dispose() {
            @delegate?.Invoke();
        }
    }
#if NETSTANDARD2_1_OR_GREATER
public sealed class AsyncDelegateDisposable : IAsyncDisposable {
    private readonly Func<ValueTask>? @delegate;
    public AsyncDelegateDisposable(Func<ValueTask>? @delegate) {
        this.@delegate = @delegate;
    }
    public ValueTask DisposeAsync() {
        return @delegate?.Invoke() ?? ValueTask.CompletedTask;
    }
}
#endif
}