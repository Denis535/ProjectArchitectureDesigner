// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace System {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal static class Option {
        public static int Compare<T>(Option<T> opt1, Option<T> opt2) {
            if (opt1.HasValue && !opt2.HasValue) return 1;
            if (!opt1.HasValue && opt2.HasValue) return -1;
            return Comparer<T>.Default.Compare( opt1.Value, opt2.Value );
        }
        public static bool Equals<T>(Option<T> opt1, Option<T> opt2) {
            if (opt1.HasValue && !opt2.HasValue) return false;
            if (!opt1.HasValue && opt2.HasValue) return false;
            return EqualityComparer<T>.Default.Equals( opt1.Value, opt2.Value );
        }
        public static Type? GetUnderlyingType(Type optionType) {
            if (optionType is null) throw new ArgumentNullException( nameof( optionType ) );
            if (GetUnboundType( optionType ) == typeof( Option<> )) return optionType.GetGenericArguments().First();
            return null;
        }
        private static Type GetUnboundType(Type type) {
            if (type.IsGenericType) {
                return type.IsGenericTypeDefinition ? type : type.GetGenericTypeDefinition();
            } else {
                return type;
            }
        }
    }
    [Serializable]
    internal readonly struct Option<T> {

        private readonly bool hasValue;
        private readonly T value;
        public bool HasValue => hasValue;
        public T Value => hasValue ? value : throw new InvalidOperationException( "Option object must have a value" );
        public T? ValueOrDefault => hasValue ? value : default;

        public Option(T value) {
            this.hasValue = true;
            this.value = value;
        }

        // GetValueOrDefault
        public T? GetValueOrDefault(T? @default) {
            return hasValue ? value : @default;
        }

        // Utils
        public override string ToString() {
            return value?.ToString() ?? "";
        }
        public override bool Equals(object? other) {
            if (value is null) return other is null;
            if (other is null) return false;
            return value.Equals( other );
        }
        public override int GetHashCode() {
            return value?.GetHashCode() ?? 0;
        }

        // Operators
        public static implicit operator Option<T>(T value) {
            return new Option<T>( value );
        }
        public static explicit operator T(Option<T> value) {
            return value.Value;
        }

    }
}
