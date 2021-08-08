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
    using System.Text;

    public static class CSharpExtensions {

        public static TResult Map<TSource, TResult>(this TSource source, Func<TSource, TResult> selector) {
            return selector( source );
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
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
public sealed class DelegateAsyncDisposable : IAsyncDisposable {
    private readonly Func<ValueTask>? @delegate;
    public DelegateAsyncDisposable(Func<ValueTask>? @delegate) {
        this.@delegate = @delegate;
    }
    public ValueTask DisposeAsync() {
        return @delegate?.Invoke() ?? ValueTask.CompletedTask;
    }
}
#endif
}