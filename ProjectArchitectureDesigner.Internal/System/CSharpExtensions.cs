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
    using System.Text;

    public static class CSharpExtensions {


        [EditorBrowsable( EditorBrowsableState.Never )]
        public static TResult Map<TSource, TResult>(this TSource source, Func<TSource, TResult> selector) {
            return selector( source );
        }


    }
}