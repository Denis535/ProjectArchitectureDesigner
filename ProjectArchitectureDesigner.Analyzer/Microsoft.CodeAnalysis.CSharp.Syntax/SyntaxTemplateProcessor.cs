// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace Microsoft.CodeAnalysis.CSharp.Syntax {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    internal static class SyntaxTemplateProcessor {


        // Comment
        public static StringBuilder Comment(this StringBuilder builder, string text, params object?[] args) {
            return builder.AppendSyntax( text, args ).AppendLine();
        }

        // Class
        public static StringBuilder Class(this StringBuilder builder, string text, params object?[] args) {
            return builder.AppendSyntax( text, args ).AppendLine();
        }

        // Property
        public static StringBuilder Property(this StringBuilder builder, string text, params object?[] args) {
            return builder.AppendSyntax( text, args ).AppendLine();
        }
        public static StringBuilder Property<T>(this StringBuilder builder, T[] objects, string text, Func<T, object> arg0, Func<T, object> arg1) {
            foreach (var @object in objects) {
                builder.Property( text, arg0( @object ), arg1( @object ) );
            }
            return builder;
        }

        // Constructor
        public static StringBuilder Constructor(this StringBuilder builder, string text, params object?[] args) {
            return builder.AppendSyntax( text, args ).AppendLine();
        }

        // Statement
        public static StringBuilder Statement(this StringBuilder builder, string text, params object?[] args) {
            return builder.AppendSyntax( text, args ).AppendLine();
        }
        public static StringBuilder Statement<T>(this StringBuilder builder, T[] objects, string text, Func<T, object> arg0) {
            foreach (var @object in objects) {
                builder.Statement( text, arg0( @object ) );
            }
            return builder;
        }
        public static StringBuilder Statement<T>(this StringBuilder builder, T[] objects, string text, Func<T, object> arg0, Func<T, object> arg1) {
            foreach (var @object in objects) {
                builder.Statement( text, arg0( @object ), arg1( @object ) );
            }
            return builder;
        }


        // Helpers
        private static StringBuilder AppendSyntax(this StringBuilder builder, string text, object?[] args) {
            for (int i = 0, j = 0; i < text.Length;) {
                if (text.IsPlaceholder( i )) {
                    var placeholder = GetPlaceholder( text, i );
                    var argument = GetArgument( args, j );
                    builder.AppendArgument( argument );
                    i += placeholder.Length;
                    j++;
                } else {
                    builder.Append( text[ i ] );
                    i++;
                }
            }
            return builder;
        }
        private static bool IsPlaceholder(this string text, int i) {
            return text[ i ] == '$';
        }
        private static ReadOnlySpan<char> GetPlaceholder(this string text, int i) {
            if (text[ i ] == '$') {
                var i2 = i + 1;
                while (i2 < text.Length && char.IsLetterOrDigit( text[ i2 ] )) i2++;
                return text.AsSpan( i, i2 - i );
            }
            return text.AsSpan( i, 0 );
        }
        private static object? GetArgument(object?[] args, int i) {
            return args[ i ];
        }
        private static void AppendArgument(this StringBuilder builder, object? argument) {
            if (argument is string @string) {
                builder.Append( @string );
                return;
            }
            if (argument is IEnumerable enumerable) {
                foreach (var item in enumerable) {
                    builder.Append( item );
                    builder.Append( ' ' );
                }
                builder.Length -= 1;
                return;
            }
            builder.Append( argument );
        }


    }
}
