// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace Microsoft.CodeAnalysis.CSharp.Syntax {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    internal class SyntaxBuilder {

        private StringBuilder Builder { get; } = new StringBuilder();


        // Class
        public SyntaxBuilder Class(string text, params object?[] args) {
            AppendSyntax( Builder, text, args ).AppendLine();
            return this;
        }

        // Property
        public SyntaxBuilder Property(string text, params object?[] args) {
            AppendSyntax( Builder, text, args ).AppendLine();
            return this;
        }
        public SyntaxBuilder Property<T>(T[] objects, string text, Func<T, (object, object)> arguments) {
            foreach (var @object in objects) {
                var args = arguments( @object );
                AppendSyntax( Builder, text, args.Item1, args.Item2 ).AppendLine();
            }
            return this;
        }

        // Constructor
        public SyntaxBuilder Constructor(string text, params object?[] args) {
            AppendSyntax( Builder, text, args ).AppendLine();
            return this;
        }

        // Statement
        public SyntaxBuilder Statement(string text, params object?[] args) {
            AppendSyntax( Builder, text, args ).AppendLine();
            return this;
        }
        public SyntaxBuilder Statement<T>(T[] objects, string text, Func<T, object> argument) {
            foreach (var @object in objects) {
                var arg = argument( @object );
                AppendSyntax( Builder, text, arg ).AppendLine();
            }
            return this;
        }
        public SyntaxBuilder Statement<T>(T[] objects, string text, Func<T, (object, object)> arguments) {
            foreach (var @object in objects) {
                var args = arguments( @object );
                AppendSyntax( Builder, text, args.Item1, args.Item2 ).AppendLine();
            }
            return this;
        }

        // Comment
        public StringBuilder Comment(string text, params object?[] args) {
            return AppendSyntax( Builder, text, args ).AppendLine();
        }

        // EndOfLine
        public StringBuilder EndOfLine() {
            return Builder.AppendLine();
        }


        // Utils
        public override string ToString() {
            return Builder.ToString();
        }


        // Helpers
        private static StringBuilder AppendSyntax(StringBuilder builder, string text, params object?[] args) {
            for (int i = 0, j = 0; i < text.Length;) {
                if (IsPlaceholder( text, i )) {
                    var placeholder = GetPlaceholder( text, i );
                    var argument = GetArgument( args, j );
                    AppendArgument( builder, argument );
                    i += placeholder.Length;
                    j++;
                } else {
                    builder.Append( text[ i ] );
                    i++;
                }
            }
            return builder;
        }
        private static bool IsPlaceholder(string text, int i) {
            return text[ i ] == '$';
        }
        private static ReadOnlySpan<char> GetPlaceholder(string text, int i) {
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
        private static void AppendArgument(StringBuilder builder, object? argument) {
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
