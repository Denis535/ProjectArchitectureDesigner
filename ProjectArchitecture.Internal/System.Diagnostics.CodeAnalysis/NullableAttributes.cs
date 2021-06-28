#if NETSTANDARD2_0
namespace System.Diagnostics.CodeAnalysis {

    // AllowNull
    [AttributeUsage( AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false )]
    internal sealed class AllowNullAttribute : Attribute {
    }

    // DisallowNull
    [AttributeUsage( AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property, Inherited = false )]
    internal sealed class DisallowNullAttribute : Attribute {
    }

    // MaybeNull
    [AttributeUsage( AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false )]
    internal sealed class MaybeNullAttribute : Attribute {
    }
    // MaybeNull/When
    [AttributeUsage( AttributeTargets.Parameter, Inherited = false )]
    internal sealed class MaybeNullWhenAttribute : Attribute {
        public MaybeNullWhenAttribute(bool returnValue) => ReturnValue = returnValue;
        public bool ReturnValue { get; }
    }

    // NotNull
    [AttributeUsage( AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = false )]
    internal sealed class NotNullAttribute : Attribute {
    }
    // NotNull/When
    [AttributeUsage( AttributeTargets.Parameter, Inherited = false )]
    internal sealed class NotNullWhenAttribute : Attribute {
        public NotNullWhenAttribute(bool returnValue) => ReturnValue = returnValue;
        public bool ReturnValue { get; }
    }
    // NotNull/IfNotNull
    [AttributeUsage( AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false )]
    internal sealed class NotNullIfNotNullAttribute : Attribute {
        public NotNullIfNotNullAttribute(string parameterName) => ParameterName = parameterName;
        public string ParameterName { get; }
    }

    // MemberNotNull
    [AttributeUsage( AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true )]
    internal sealed class MemberNotNullAttribute : Attribute {
        public MemberNotNullAttribute(string member) => Members = new[] { member };
        public MemberNotNullAttribute(params string[] members) => Members = members;
        public string[] Members { get; }
    }
    // MemberNotNull/When
    [AttributeUsage( AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = true )]
    internal sealed class MemberNotNullWhenAttribute : Attribute {
        public MemberNotNullWhenAttribute(bool returnValue, string member) {
            ReturnValue = returnValue;
            Members = new[] { member };
        }
        public MemberNotNullWhenAttribute(bool returnValue, params string[] members) {
            ReturnValue = returnValue;
            Members = members;
        }
        public bool ReturnValue { get; }
        public string[] Members { get; }
    }

    // DoesNotReturn
    [AttributeUsage( AttributeTargets.Method, Inherited = false )]
    internal sealed class DoesNotReturnAttribute : Attribute {
    }
    // DoesNotReturn/If
    [AttributeUsage( AttributeTargets.Parameter, Inherited = false )]
    internal sealed class DoesNotReturnIfAttribute : Attribute {
        public DoesNotReturnIfAttribute(bool parameterValue) => ParameterValue = parameterValue;
        public bool ParameterValue { get; }
    }

}
#endif