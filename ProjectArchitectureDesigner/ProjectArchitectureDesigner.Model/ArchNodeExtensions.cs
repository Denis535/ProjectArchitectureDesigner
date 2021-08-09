// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class ArchNodeExtensions {


        // Testing
        public static IEnumerable<TypeArchNode> GetTypesWithInvalidModule(this ProjectArchNode project) {
            foreach (var type in project.GetDescendantTypes()) {
                if (type.Group.Namespace.Module.Name != type.Value.Assembly.GetName().Name) {
                    yield return type;
                }
            }
        }
        public static IEnumerable<TypeArchNode> GetTypesWithInvalidNamespace(this ProjectArchNode project) {
            foreach (var type in project.GetDescendantTypes()) {
                if (type.Group.Namespace.Name != type.Value.Namespace) {
                    yield return type;
                }
            }
        }
        public static void GetMissingAndExtraTypes(this ProjectArchNode project, out Type[] missing, out TypeArchNode[] extra) {
            var actual = project.GetDescendantTypes();
            var expected = project.Assemblies.SelectMany( i => i.DefinedTypes ).Where( project.IsSupported );
            var expected_ = new LinkedList<Type>( expected );
            extra = actual.Where( i => !expected_.Remove( i.Value ) ).ToArray();
            missing = expected_.ToArray();
        }


        // GetDescendant/Project
        public static IEnumerable<NamespaceArchNode> GetDescendantNamespaces(this ProjectArchNode project) => project.Modules.SelectMany( i => i.Namespaces );
        public static IEnumerable<GroupArchNode> GetDescendantGroups(this ProjectArchNode project) => project.Modules.SelectMany( i => i.Namespaces ).SelectMany( i => i.Groups );
        public static IEnumerable<TypeArchNode> GetDescendantTypes(this ProjectArchNode project) => project.Modules.SelectMany( i => i.Namespaces ).SelectMany( i => i.Groups ).SelectMany( i => i.Types );
        // GetDescendant/Namespace
        public static IEnumerable<GroupArchNode> GetDescendantGroups(this ModuleArchNode module) => module.Namespaces.SelectMany( i => i.Groups );
        public static IEnumerable<TypeArchNode> GetDescendantTypes(this ModuleArchNode module) => module.Namespaces.SelectMany( i => i.Groups ).SelectMany( i => i.Types );
        // GetDescendant/Types
        public static IEnumerable<TypeArchNode> GetDescendantTypes(this NamespaceArchNode @namespace) => @namespace.Groups.SelectMany( i => i.Types );


        // GetDescendant
        public static IEnumerable<ArchNode> GetDescendant(this ArchNode node) {
            return GetChildren( node ).SelectMany( GetDescendantAndSelf );
        }
        public static IEnumerable<ArchNode> GetDescendantAndSelf(this ArchNode node) {
            return node.GetDescendant().Prepend( node );
        }


        // GetChildren
        public static IEnumerable<ArchNode> GetChildren(this ArchNode node) {
            if (node is ProjectArchNode project) return project.Modules;
            if (node is ModuleArchNode module) return module.Namespaces;
            if (node is NamespaceArchNode @namespace) return @namespace.Groups;
            if (node is GroupArchNode group) return group.Types;
            if (node is TypeArchNode) return Enumerable.Empty<ArchNode>();
            throw new ArgumentException( "Node is invalid: " + node );
        }
        public static IEnumerable<ArchNode> GetChildrenAndSelf(this ArchNode node) {
            return GetChildren( node ).Prepend( node );
        }


    }
}
