﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace System.Text {
    using System;
    using System.Collections.Generic;

    public static class StringBuilderExtensions {


        // Append/Range
        public static StringBuilder AppendRangeIf(this StringBuilder builder, bool condition, IEnumerable<string> values) {
            if (!condition) return builder;
            foreach (var value in values) builder.Append( value );
            return builder;
        }
        public static StringBuilder AppendRange(this StringBuilder builder, IEnumerable<string> values) {
            foreach (var value in values) builder.Append( value );
            return builder;
        }
        // Append/Join
        public static StringBuilder AppendJoinIf(this StringBuilder builder, bool condition, string separator, IEnumerable<string> values) {
            if (!condition) return builder;
            foreach (var (value, isLast) in values.TagLast()) builder.Append( value ).AppendIf( !isLast, separator );
            return builder;
        }
        public static StringBuilder AppendJoin(this StringBuilder builder, string separator, IEnumerable<string> values) {
            foreach (var (value, isLast) in values.TagLast()) builder.Append( value ).AppendIf( !isLast, separator );
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
        public static StringBuilder AppendLineFormatIf(this StringBuilder builder, bool condition, string format, params object?[] args) {
            if (!condition) return builder;
            return builder.AppendLine( string.Format( format, args ) );
        }
        public static StringBuilder AppendLineFormat(this StringBuilder builder, string format, params object?[] args) {
            return builder.AppendLine( string.Format( format, args ) );
        }


    }
}