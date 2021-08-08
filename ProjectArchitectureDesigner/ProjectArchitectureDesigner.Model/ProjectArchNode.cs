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
        // Descendant
        public IEnumerable<NamespaceArchNode> Namespaces => Modules.SelectMany( i => i.Namespaces );
        public IEnumerable<GroupArchNode> Groups => Modules.SelectMany( i => i.Namespaces ).SelectMany( i => i.Groups );
        public IEnumerable<TypeArchNode> Types => Modules.SelectMany( i => i.Namespaces ).SelectMany( i => i.Groups ).SelectMany( i => i.Types );


        public ProjectArchNode() {
        }


        // Initialize
        protected virtual void Initialize() { // Used by source generator
        }
        protected virtual void SetChildren(params Type[] children) { // Used by source generator
        }


        // Testing
        public IEnumerable<TypeArchNode> GetTypesWithInvalidModule() {
            foreach (var type in Types) {
                if (type.Group.Namespace.Module.Name != type.Value.Assembly.GetName().Name) {
                    yield return type;
                }
            }
        }
        public IEnumerable<TypeArchNode> GetTypesWithInvalidNamespace() {
            foreach (var type in Types) {
                if (type.Group.Namespace.Name != type.Value.Namespace) {
                    yield return type;
                }
            }
        }
        public void GetMissingAndExtraTypes(out Type[] missing, out TypeArchNode[] extra) {
            var actual = Types;
            var expected = Assemblies.SelectMany( i => i.DefinedTypes ).Where( IsSupported );
            var expected_ = new LinkedList<Type>( expected );
            extra = actual.Where( i => !expected_.Remove( i.Value ) ).ToArray();
            missing = expected_.ToArray();
        }


        // IsSupported
        public virtual bool IsSupported(Type type) {
            return !type.IsObsolete() && !type.IsCompilerGenerated();
        }
        // IsVisible
        public virtual bool IsVisible(TypeArchNode type) {
            return type.Value.IsVisible;
        }


        // Utils
        public override string ToString() {
            return "Project: " + Name;
        }


    }
}
