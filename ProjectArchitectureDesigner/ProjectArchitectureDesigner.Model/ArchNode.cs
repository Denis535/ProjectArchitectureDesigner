// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public abstract class ArchNode {

        public abstract string Name { get; }
        // Children
        public IEnumerable<ArchNode> Children => GetChildren( this );
        public IEnumerable<ArchNode> ChildrenAndSelf => GetChildren( this ).Prepend( this );
        // Descendant
        public IEnumerable<ArchNode> Descendant => GetDescendant( this );
        public IEnumerable<ArchNode> DescendantAndSelf => GetDescendant( this ).Prepend( this );


        // Utils
        public override string ToString() {
            return "Node: " + Name;
        }


        // Helpers/GetName
        protected static string GetName(ProjectArchNode project) {
            return project.GetType().Name.Replace( '_', '.' );
        }
        protected static string GetName(ModuleArchNode module) {
            return module.GetType().Name.Replace( '_', '.' );
        }
        protected static string GetName(NamespaceArchNode @namespace) {
            return @namespace.GetType().Name.Replace( '_', '.' );
        }
        protected static string GetName(GroupArchNode group) {
            return group.GetType().Name.Replace( '_', ' ' );
        }
        // Helpers/GetChildren
        protected static IEnumerable<T> GetChildren<T>(ArchNode node) where T : ArchNode {
            return
                node
                .GetType()
                .GetProperties( BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly )
                .Where( IsOfType<T> )
                .Select( i => i.GetValue( node ) )
                .Cast<T>();
        }
        private static bool IsOfType<T>(PropertyInfo property) {
            return
                property.PropertyType.Equals( typeof( T ) ) ||
                property.PropertyType.IsSubclassOf( typeof( T ) );
        }
        // Helpers/GetChildren
        private static IEnumerable<ArchNode> GetChildren(ArchNode node) {
            if (node is ProjectArchNode project) return project.Modules;
            if (node is ModuleArchNode module) return module.Namespaces;
            if (node is NamespaceArchNode @namespace) return @namespace.Groups;
            if (node is GroupArchNode group) return group.Types;
            if (node is TypeArchNode) return Enumerable.Empty<ArchNode>();
            throw new ArgumentException( "Node is invalid: " + node );
        }
        // Helpers/GetDescendant
        private static IEnumerable<ArchNode> GetDescendant(ArchNode node) {
            if (node is ProjectArchNode project) return project.Modules.SelectMany( GetDescendantAndSelf );
            if (node is ModuleArchNode module) return module.Namespaces.SelectMany( GetDescendantAndSelf );
            if (node is NamespaceArchNode @namespace) return @namespace.Groups.SelectMany( GetDescendantAndSelf );
            if (node is GroupArchNode group) return group.Types;
            if (node is TypeArchNode) return Enumerable.Empty<ArchNode>();
            throw new ArgumentException( "Node is invalid: " + node );
        }
        private static IEnumerable<ArchNode> GetDescendantAndSelf(ArchNode node) {
            return GetDescendant( node ).Prepend( node );
        }


    }
}
