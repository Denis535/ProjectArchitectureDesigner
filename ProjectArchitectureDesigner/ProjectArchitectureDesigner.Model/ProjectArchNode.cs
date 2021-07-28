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
            SetUpHierarchy( this );
        }


        // Initialize
        protected virtual void Initialize() { // Used by source generator
        }
        protected virtual void SetChildren(params Type[] children) { // Used by source generator
        }


        // GetTypesWithInvalidModule
        public IEnumerable<TypeArchNode> GetTypesWithInvalidModule() {
            foreach (var type in Types) {
                if (type.Module.Name != type.Value.Assembly.GetName().Name) {
                    yield return type;
                }
            }
        }
        // GetTypesWithInvalidNamespace
        public IEnumerable<TypeArchNode> GetTypesWithInvalidNamespace() {
            foreach (var type in Types) {
                if (type.Namespace.Name != type.Value.Namespace) {
                    yield return type;
                }
            }
        }
        // GetMissingAndExtraTypes
        public void GetMissingAndExtraTypes(out List<Type> missing, out List<TypeArchNode> extra) {
            var actual = Types;
            var expected = Assemblies.SelectMany( i => i.DefinedTypes ).Where( IsSupported );
            var expected_ = new LinkedList<Type>( expected );
            extra = actual.Where( i => !expected_.Remove( i.Value ) ).ToList();
            missing = expected_.ToList();
        }


        // IsSupported
        public virtual bool IsSupported(Type type) {
            return !type.IsObsolete() && !type.IsCompilerGenerated();
        }
        // IsVisible
        public virtual bool IsVisible(Type type) {
            return type.IsVisible;
        }


        // Utils
        public override string ToString() {
            return "Project: " + Name;
        }


        // Helpers
        protected static void SetUpHierarchy(ProjectArchNode project) {
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
