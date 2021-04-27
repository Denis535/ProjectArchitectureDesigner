// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public abstract class Project : Node {

        public abstract Module[] Modules { get; }


        // Compare/Assembly
        public void Compare(Assembly assembly, out IList<Type> intersected, out IList<Type> missing, out IList<Type> extra) {
            var actual = Flatten<TypeItem>().Select( i => i.Type );
            var expected = assembly.DefinedTypes.Where( IsSupported );
            Utils.Compare( actual, expected, out intersected, out missing, out extra );
        }
        public void Compare(Assembly[] assemblies, out IList<Type> intersected, out IList<Type> missing, out IList<Type> extra) {
            var actual = Flatten<TypeItem>().Select( i => i.Type );
            var expected = assemblies.SelectMany( i => i.DefinedTypes ).Where( IsSupported );
            Utils.Compare( actual, expected, out intersected, out missing, out extra );
        }
        // Compare/Type
        public void Compare(IEnumerable<Type> types, out IList<Type> intersected, out IList<Type> missing, out IList<Type> extra) {
            var actual = Flatten<TypeItem>().Select( i => i.Type );
            var expected = types.Where( IsSupported );
            Utils.Compare( actual, expected, out intersected, out missing, out extra );
        }


        // Flatten
        public IEnumerable<Node> Flatten() {
            yield return this;

            foreach (var module in Modules) {
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
        public IEnumerable<T> Flatten<T>() where T : Node {
            return Flatten().OfType<T>();
        }


        // Utils
        public override string ToString() {
            return "Project: " + Name;
        }


        // Infrastructure
        protected virtual bool IsSupported(Type type) {
            return !type.IsObsolete() && !type.IsCompilerGenerated() && !type.IsNestedPrivate;
        }


    }
}
