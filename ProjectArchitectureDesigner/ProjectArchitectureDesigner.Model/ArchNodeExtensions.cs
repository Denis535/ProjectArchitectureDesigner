// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public static class ArchNodeExtensions {


        // Testing
        public static IEnumerable<TypeArchNode> GetTypesWithInvalidModule(this ProjectArchNode project) {
            foreach (var type in project.GetTypes()) {
                if (type.Group.Namespace.Module.Name != type.Value.Assembly.GetName().Name) {
                    yield return type;
                }
            }
        }
        public static IEnumerable<TypeArchNode> GetTypesWithInvalidNamespace(this ProjectArchNode project) {
            foreach (var type in project.GetTypes()) {
                if (type.Group.Namespace.Name != type.Value.Namespace) {
                    yield return type;
                }
            }
        }
        public static void GetMissingAndExtraTypes(this ProjectArchNode project, out Type[] missing, out TypeArchNode[] extra) {
            var actual = project.GetTypes();
            var expected = project.GetAssemblies().SelectMany( i => i.DefinedTypes ).Where( project.IsSupported );
            var expected_ = new LinkedList<Type>( expected );
            extra = actual.Where( i => !expected_.Remove( i.Value ) ).ToArray();
            missing = expected_.ToArray();
        }


        // WithVisibleOnly
        public static ProjectArchNode WithVisibleOnly(this ProjectArchNode project) {
            var modules = project.Modules.GetVisibleOnly().ToArray();
            return new ProjectArchNode( project.Name, modules );
        }
        private static IEnumerable<ModuleArchNode> GetVisibleOnly(this ModuleArchNode[] modules) {
            foreach (var module in modules) {
                var namespaces = module.Namespaces.GetVisibleOnly().ToArray();
                if (namespaces.Any()) yield return new ModuleArchNode( module.Assembly, module.Name, namespaces );
            }
        }
        private static IEnumerable<NamespaceArchNode> GetVisibleOnly(this NamespaceArchNode[] namespaces) {
            foreach (var @namespace in namespaces) {
                var groups = @namespace.Groups.GetVisibleOnly().ToArray();
                if (groups.Any()) yield return new NamespaceArchNode( @namespace.Name, groups );
            }
        }
        private static IEnumerable<GroupArchNode> GetVisibleOnly(this GroupArchNode[] groups) {
            foreach (var group in groups) {
                var types = group.Types.GetVisibleOnly().ToArray();
                if (types.Any()) yield return new GroupArchNode( group.Name, types );
            }
        }
        private static IEnumerable<TypeArchNode> GetVisibleOnly(this TypeArchNode[] types) {
            foreach (var type in types) {
                if (type.Group.Namespace.Module.Project.IsVisible( type )) yield return new TypeArchNode( type.Value );
            }
        }


        // GetName
        public static string GetName(this ArchNode node) {
            if (node is ProjectArchNode project) return project.Name;
            if (node is ModuleArchNode module) return module.Name;
            if (node is NamespaceArchNode @namespace) return @namespace.Name;
            if (node is GroupArchNode group) return group.Name;
            if (node is TypeArchNode type) return type.Name;
            throw new ArgumentException( "Node is invalid: " + node );
        }
        // GetAssemblies
        public static Assembly[] GetAssemblies(this ProjectArchNode project) {
            return project.Modules.Select( i => i.Assembly ).OfType<Assembly>().ToArray();
        }
        // IsGlobal
        public static bool IsGlobal(this NamespaceArchNode @namespace) {
            return @namespace.Name is null or "" or "Global";
        }
        // IsDefault
        public static bool IsDefault(this GroupArchNode group) {
            return group.Name is null or "" or "Global";
        }


        // GetDescendant/Project
        public static IEnumerable<NamespaceArchNode> GetNamespaces(this ProjectArchNode project) => project.Modules.SelectMany( i => i.Namespaces );
        public static IEnumerable<GroupArchNode> GetGroups(this ProjectArchNode project) => project.Modules.SelectMany( i => i.Namespaces ).SelectMany( i => i.Groups );
        public static IEnumerable<TypeArchNode> GetTypes(this ProjectArchNode project) => project.Modules.SelectMany( i => i.Namespaces ).SelectMany( i => i.Groups ).SelectMany( i => i.Types );
        // GetDescendant/Namespace
        public static IEnumerable<GroupArchNode> GetGroups(this ModuleArchNode module) => module.Namespaces.SelectMany( i => i.Groups );
        public static IEnumerable<TypeArchNode> GetTypes(this ModuleArchNode module) => module.Namespaces.SelectMany( i => i.Groups ).SelectMany( i => i.Types );
        // GetDescendant/Types
        public static IEnumerable<TypeArchNode> GetTypes(this NamespaceArchNode @namespace) => @namespace.Groups.SelectMany( i => i.Types );


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
