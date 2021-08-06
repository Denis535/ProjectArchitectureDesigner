// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace Microsoft.CodeAnalysis.CSharp.Syntax {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    internal class SyntaxBuilder {

        private StringBuilder Builder { get; } = new StringBuilder();
        private int Level { get; set; } = 0;


        // UsingDirective
        public void UsingDirective(IEnumerable<UsingDirectiveSyntax> objects) {
            foreach (var @object in objects) {
                AppendSyntaxLine( @object.ToString() );
            }
        }

        // ExternAliasDirective
        public void ExternAliasDirective(IEnumerable<ExternAliasDirectiveSyntax> objects) {
            foreach (var @object in objects) {
                AppendSyntaxLine( @object.ToString() );
            }
        }

        // Namespace
        public void Namespace(string text, params object?[] args) {
            AppendSyntaxLine( text, args );
        }

        // Class
        public void Class(string text, params object?[] args) {
            AppendSyntaxLine( text, args );
        }

        // Property
        public void Property(string text, params object?[] args) {
            AppendSyntaxLine( text, args );
        }
        public void Property<T>(T[] objects, string text, Func<T, (object, object)> arguments) {
            foreach (var @object in objects) {
                var args = arguments( @object );
                AppendSyntaxLine( text, args.Item1, args.Item2 );
            }
        }

        // Constructor
        public void Constructor(string text, params object?[] args) {
            AppendSyntaxLine( text, args );
        }

        // Statement
        public void Statement(string text, params object?[] args) {
            AppendSyntaxLine( text, args );
        }
        public void Statement<T>(T[] objects, string text, Func<T, object> argument) {
            foreach (var @object in objects) {
                var arg = argument( @object );
                AppendSyntaxLine( text, arg );
            }
        }
        public void Statement<T>(T[] objects, string text, Func<T, (object, object)> arguments) {
            foreach (var @object in objects) {
                var args = arguments( @object );
                AppendSyntaxLine( text, args.Item1, args.Item2 );
            }
        }

        // Comment
        public void Comment(string text, params object?[] args) {
            AppendSyntaxLine( text, args );
        }

        // EndOfLine
        public void EndOfLine() {
            Builder.AppendLine();
        }

        // Indent
        private void AppendSyntaxLine(string text, params object?[] args) {
            if (text.StartsWith( "}" )) Level--;
            Builder.Append( ' ', Level * 4 );
            AppendTemplate( Builder, text, args );
            Builder.AppendLine();
            if (text.EndsWith( "{" )) Level++;
        }


        // Utils
        public override string ToString() {
            return Builder.ToString().TrimEnd( '\r', '\n' );
        }


        // Helpers
        private static StringBuilder AppendTemplate(StringBuilder builder, string text, params object?[] args) {
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
