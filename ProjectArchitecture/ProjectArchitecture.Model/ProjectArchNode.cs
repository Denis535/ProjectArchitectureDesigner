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
        public ModuleArchNode[] Modules => GetChildren<ModuleArchNode>( this ).ToArray();
        public ArchNode[] DescendantNodes => GetDescendantNodes( this ).ToArray();
        public ArchNode[] DescendantNodesAndSelf => GetDescendantNodes( this ).Prepend( this ).ToArray();


        // Initialization
        protected abstract void DefineChildren(); // Used by source generator
        protected void SetChildren(params Type[] children) { // Used by source generator
        }


        // Compare
        public void Compare(Assembly assembly, out IList<Type> intersected, out IList<Type> missing, out IList<Type> extra) {
            var actual = GetDescendantNodes<TypeArchNode>( this ).Select( i => i.Value );
            var expected = assembly.DefinedTypes.Where( IsSupported );
            Utils.Compare( actual, expected, out intersected, out missing, out extra );
        }
        public void Compare(Assembly[] assemblies, out IList<Type> intersected, out IList<Type> missing, out IList<Type> extra) {
            var actual = GetDescendantNodes<TypeArchNode>( this ).Select( i => i.Value );
            var expected = assemblies.SelectMany( i => i.DefinedTypes ).Where( IsSupported );
            Utils.Compare( actual, expected, out intersected, out missing, out extra );
        }
        public void Compare(IEnumerable<Type> types, out IList<Type> intersected, out IList<Type> missing, out IList<Type> extra) {
            var actual = GetDescendantNodes<TypeArchNode>( this ).Select( i => i.Value );
            var expected = types.Where( IsSupported );
            Utils.Compare( actual, expected, out intersected, out missing, out extra );
        }


        // GetSupportedTypes
        public IEnumerable<Type> GetSupportedTypes(Assembly assembly) {
            return assembly.DefinedTypes.Where( IsSupported );
        }
        public IEnumerable<Type> GetSupportedTypes(params Assembly[] assemblies) {
            return assemblies.SelectMany( i => i.DefinedTypes ).Where( IsSupported );
        }


        // Utils
        public override string ToString() {
            return "Project: " + Name;
        }


        // Infrastructure
        protected virtual bool IsSupported(Type type) {
            return !type.IsObsolete() && !type.IsCompilerGenerated();
        }


        // Helpers
        private static IEnumerable<T> GetDescendantNodes<T>(ProjectArchNode project) where T : ArchNode {
            return GetDescendantNodes( project ).OfType<T>();
        }
        private static IEnumerable<ArchNode> GetDescendantNodes(ProjectArchNode project) {
            foreach (var module in project.Modules) {
                yield return module;

                foreach (var @namespace in module.Namespaces) {
                    yield return @namespace;

                    foreach (var group in @namespace.Groups) {
                        yield return group;

                        foreach (var type in group.Types) {
                            yield return type;
                        }
                    }
                }
            }
        }


    }
}
