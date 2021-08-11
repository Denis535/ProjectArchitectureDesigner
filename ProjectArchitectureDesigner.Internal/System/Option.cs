// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace System {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;

    public static class Option {
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
        public static Type? GetUnderlyingType(Type type) {
            if (type is null) throw new ArgumentNullException( nameof( type ) );
            if (GetUnboundType( type ) == typeof( Option<> )) return type.GetGenericArguments().First();
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
    // Note: don't override true, false operators!
    // Note: when HasValue == true then value may be null (it's allowed)
    // Note: when HasValue == false then value always is null
    // Note: we need something like [MemberMaybeNullWhen( false, nameof( ValueOrDefault ) )]
    [Serializable]
    public readonly struct Option<T> : IEquatable<Option<T>> {

        private readonly bool hasValue;
        private readonly T value;
        public bool HasValue => hasValue;
        public T Value => hasValue ? value : throw new InvalidOperationException( "Option object must have a value" );
        public T? ValueOrDefault => hasValue ? value : default;

        public Option(T value) {
            this.hasValue = true;
            this.value = value;
        }

        // TryGetValue
        public bool TryGetValue([MaybeNullWhen( false )] out T value) {
            value = hasValue ? this.value : default;
            return hasValue;
        }

        // Utils
        public override string ToString() {
            if (hasValue) return value?.ToString() ?? "";
            return "";
        }
        public override bool Equals(object? other) {
            if (other is Option<T> other_) return Equals( other_ );
            return false;
        }
        public bool Equals(Option<T> other) {
            if (!hasValue) return !other.hasValue;
            if (!other.hasValue) return !hasValue;
            return AreEqual( value, other.value );
        }
        public override int GetHashCode() {
            if (hasValue) return value?.GetHashCode() ?? 0;
            return 0;
        }

        // Conversions
        public static implicit operator Option<T>(T value) {
            return new Option<T>( value );
        }
        public static explicit operator T(Option<T> value) {
            return value.Value;
        }

        // Operators
        public static bool operator ==(Option<T> left, Option<T> right) {
            return left.Equals( right );
        }
        public static bool operator !=(Option<T> left, Option<T> right) {
            return !left.Equals( right );
        }


        // Helpers
        private static bool AreEqual(T v1, T v2) {
            if (v1 is null) return v2 is null;
            if (v2 is null) return v1 is null;
            return v1.Equals( v2 );
        }


    }
}
