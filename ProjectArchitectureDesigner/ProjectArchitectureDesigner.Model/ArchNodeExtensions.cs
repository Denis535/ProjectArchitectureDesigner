// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public static class ArchNodeExtensions {


        // WithVisibleOnly
        public static ProjectArchNode WithVisibleOnly(this ProjectArchNode project) {
            var modules = project.Modules.GetVisibleOnly().ToArray();
            return new ProjectArchNode( project.Assemblies, project.Name, modules );
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
                if (type.GetProject().IsVisible( type )) yield return new TypeArchNode( type.Value );
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
            if (project.Assemblies != null) return project.Assemblies;
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
        // IsLast
        public static bool IsLast(this ModuleArchNode module) {
            return module == module.Project.Modules.Last();
        }
        public static bool IsLast(this NamespaceArchNode @namespace) {
            return @namespace == @namespace.Module.Namespaces.Last();
        }
        public static bool IsLast(this GroupArchNode group) {
            return group == group.Namespace.Groups.Last();
        }
        public static bool IsLast(this TypeArchNode type) {
            return type == type.Group.Types.Last();
        }


        // GetProject
        public static ProjectArchNode GetProject(this ModuleArchNode module) {
            return module.Project;
        }
        public static ProjectArchNode GetProject(this NamespaceArchNode @namespace) {
            return @namespace.Module.Project;
        }
        public static ProjectArchNode GetProject(this GroupArchNode group) {
            return group.Namespace.Module.Project;
        }
        public static ProjectArchNode GetProject(this TypeArchNode type) {
            return type.Group.Namespace.Module.Project;
        }
        // GetModule
        public static ModuleArchNode GetModule(this NamespaceArchNode @namespace) {
            return @namespace.Module;
        }
        public static ModuleArchNode GetModule(this GroupArchNode group) {
            return group.Namespace.Module;
        }
        public static ModuleArchNode GetModule(this TypeArchNode type) {
            return type.Group.Namespace.Module;
        }
        // GetNamespace
        public static NamespaceArchNode GetNamespace(this GroupArchNode group) {
            return group.Namespace;
        }
        public static NamespaceArchNode GetNamespace(this TypeArchNode type) {
            return type.Group.Namespace;
        }


        // GetNamespaces
        public static IEnumerable<NamespaceArchNode> GetNamespaces(this ProjectArchNode project) {
            return project.Modules.SelectMany( i => i.Namespaces );
        }
        // GetGroups
        public static IEnumerable<GroupArchNode> GetGroups(this ProjectArchNode project) {
            return project.Modules.SelectMany( i => i.Namespaces ).SelectMany( i => i.Groups );
        }
        public static IEnumerable<GroupArchNode> GetGroups(this ModuleArchNode module) {
            return module.Namespaces.SelectMany( i => i.Groups );
        }
        // GetTypes
        public static IEnumerable<TypeArchNode> GetTypes(this ProjectArchNode project) {
            return project.Modules.SelectMany( i => i.Namespaces ).SelectMany( i => i.Groups ).SelectMany( i => i.Types );
        }
        public static IEnumerable<TypeArchNode> GetTypes(this ModuleArchNode module) {
            return module.Namespaces.SelectMany( i => i.Groups ).SelectMany( i => i.Types );
        }
        public static IEnumerable<TypeArchNode> GetTypes(this NamespaceArchNode @namespace) {
            return @namespace.Groups.SelectMany( i => i.Types );
        }


        // GetAncestors
        public static IEnumerable<ArchNode> GetAncestors(this ArchNode node) {
            var parent = node.GetParent();
            while (parent != null) {
                yield return parent;
                parent = parent.GetParent();
            }
        }
        public static IEnumerable<ArchNode> GetAncestorsAndSelf(this ArchNode node) {
            return node.GetAncestors().Prepend( node );
        }
        // GetDescendant
        public static IEnumerable<ArchNode> GetDescendant(this ArchNode node) {
            foreach (var child in node.GetChildren()) {
                yield return child;
                foreach (var i in child.GetDescendant()) yield return i;
            }
        }
        public static IEnumerable<ArchNode> GetDescendantAndSelf(this ArchNode node) {
            return node.GetDescendant().Prepend( node );
        }


        // GetParent
        public static ArchNode? GetParent(this ArchNode node) {
            if (node is ProjectArchNode) return null;
            if (node is ModuleArchNode module) return module.Project;
            if (node is NamespaceArchNode @namespace) return @namespace.Module;
            if (node is GroupArchNode group) return group.Namespace;
            if (node is TypeArchNode type) return type.Group;
            throw new ArgumentException( "Node is invalid: " + node );
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


    }
}
