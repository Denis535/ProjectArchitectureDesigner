// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public abstract class ProjectArchNode : ArchNode {

        public virtual Assembly[] Assemblies => Modules.Select( i => i.Assembly ).OfType<Assembly>().ToArray();
        // Children
        public abstract ModuleArchNode[] Modules { get; }
        public NamespaceArchNode[] Namespaces => Modules.SelectMany( i => i.Namespaces ).ToArray();
        public GroupArchNode[] Groups => Modules.SelectMany( i => i.Namespaces ).SelectMany( i => i.Groups ).ToArray();
        public TypeArchNode[] Types => Modules.SelectMany( i => i.Namespaces ).SelectMany( i => i.Groups ).SelectMany( i => i.Types ).ToArray();
        public ArchNode[] DescendantNodes => GetDescendantNodes( this ).ToArray();
        public ArchNode[] DescendantNodesAndSelf => GetDescendantNodes( this ).Prepend( this ).ToArray();


        public ProjectArchNode() {
            Initialize( this );
        }


        // Initialize
        protected abstract void Initialize(); // Used by source generator
        protected virtual void SetChildren(params Type[] children) { // Used by source generator
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


        // IsSupported
        protected virtual bool IsSupported(Type type) {
            return !type.IsObsolete() && !type.IsCompilerGenerated();
        }


        // Utils
        public override string ToString() {
            return "Project: " + Name;
        }


        // Helpers
        protected static void Initialize(ProjectArchNode project) {
            // Project
            foreach (var module in project.Modules) {
                module.Project = project;
                // Namespace
                foreach (var @namespace in module.Namespaces) {
                    @namespace.Module = module;
                    // Group
                    foreach (var group in @namespace.Groups) {
                        group.Namespace = @namespace;
                        // Type
                        foreach (var type in group.Types) {
                            type.Group = group;
                        }
                    }
                }
            }
        }


    }
}
