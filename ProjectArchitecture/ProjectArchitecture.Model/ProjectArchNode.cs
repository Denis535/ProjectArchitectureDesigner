// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public abstract class ProjectArchNode : ArchNode {

        public override string Name => GetName( this );
        public virtual ModuleArchNode[] Modules => GetChildren<ModuleArchNode>( this ).ToArray();
        public ArchNode[] DescendantNodes => GetDescendantNodes( this ).ToArray();
        public ArchNode[] DescendantNodesAndSelf => GetDescendantNodes( this ).Prepend( this ).ToArray();


        // Initialization
        protected abstract void DefineChildren(); // Used by source generator
        protected void SetChildren(params Type[] children) { // Used by source generator
        }


        // Compare
        public void Compare(Assembly assembly, out IList<Type> intersected, out IList<Type> missing, out IList<Type> extra) {
            var actual = GetDescendantNodes( this ).OfType<TypeArchNode>().Select( i => i.Value );
            var expected = assembly.DefinedTypes.Where( IsSupported );
            EnumerableExtensions.Compare( actual, expected, out intersected, out missing, out extra );
        }
        public void Compare(Assembly[] assemblies, out IList<Type> intersected, out IList<Type> missing, out IList<Type> extra) {
            var actual = GetDescendantNodes( this ).OfType<TypeArchNode>().Select( i => i.Value );
            var expected = assemblies.SelectMany( i => i.DefinedTypes ).Where( IsSupported );
            EnumerableExtensions.Compare( actual, expected, out intersected, out missing, out extra );
        }
        public void Compare(IEnumerable<Type> types, out IList<Type> intersected, out IList<Type> missing, out IList<Type> extra) {
            var actual = GetDescendantNodes( this ).OfType<TypeArchNode>().Select( i => i.Value );
            var expected = types.Where( IsSupported );
            EnumerableExtensions.Compare( actual, expected, out intersected, out missing, out extra );
        }


        // GetSupportedTypes
        public Type[] GetSupportedTypes(Assembly assembly) {
            return assembly.DefinedTypes.Where( IsSupported ).ToArray();
        }
        public Type[] GetSupportedTypes(params Assembly[] assemblies) {
            return assemblies.SelectMany( i => i.DefinedTypes ).Where( IsSupported ).ToArray();
        }


        // Utils
        public override string ToString() {
            return "Project: " + Name;
        }


        // Infrastructure
        protected virtual bool IsSupported(Type type) {
            return !type.IsObsolete() && !type.IsCompilerGenerated();
        }


    }
}
