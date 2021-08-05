// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace System.Text {
    using System;
    using System.Collections.Generic;

    public static class StringBuilderExtensions {


        // Append/Range
        public static StringBuilder AppendRange(this StringBuilder builder, IEnumerable<string> values) {
            foreach (var value in values) builder.Append( value );
            return builder;
        }
        public static StringBuilder AppendRange<T>(this StringBuilder builder, IEnumerable<T> values, Func<T, string> selector) {
            foreach (var value in values) builder.Append( selector( value ) );
            return builder;
        }
        public static StringBuilder AppendRange<T>(this StringBuilder builder, IEnumerable<T> values, Action<StringBuilder, T> renderer) {
            foreach (var value in values) renderer( builder, value );
            return builder;
        }


        // Append/Join
        public static StringBuilder AppendJoin(this StringBuilder builder, string separator, IEnumerable<string> values) {
            foreach (var (value, isLast) in values.TagLast()) {
                builder.Append( value );
                if (!isLast) builder.Append( separator );
            }
            return builder;
        }
        public static StringBuilder AppendJoin<T>(this StringBuilder builder, string separator, IEnumerable<T> values, Func<T, string> selector) {
            foreach (var (value, isLast) in values.TagLast()) {
                builder.Append( selector( value ) );
                if (!isLast) builder.Append( separator );
            }
            return builder;
        }
        public static StringBuilder AppendJoin<T>(this StringBuilder builder, string separator, IEnumerable<T> values, Action<StringBuilder, T> renderer) {
            foreach (var (value, isLast) in values.TagLast()) {
                renderer( builder, value );
                if (!isLast) builder.Append( separator );
            }
            return builder;
        }


        // Append
        public static StringBuilder AppendIf(this StringBuilder builder, bool condition, string value) {
            if (!condition) return builder;
            return builder.Append( value );
        }
        // Append/Line
        public static StringBuilder AppendLineIf(this StringBuilder builder, bool condition, string value) {
            if (!condition) return builder;
            return builder.AppendLine( value );
        }
        // Append/Line/Format
        public static StringBuilder AppendLineFormat(this StringBuilder builder, string format, params object?[] args) {
            return builder.AppendFormat( format, args ).AppendLine();
        }
        public static StringBuilder AppendLineFormatIf(this StringBuilder builder, bool condition, string format, params object?[] args) {
            if (!condition) return builder;
            return builder.AppendFormat( format, args ).AppendLine();
        }


    }
}