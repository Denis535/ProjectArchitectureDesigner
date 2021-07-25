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


        // Utils
        public override string ToString() {
            return "Node: " + Name;
        }


        // Helpers/GetName
        protected static string GetName(ProjectArchNode project) {
            return WithoutPrefix( project.GetType().Name, "Project_" ).Replace( '_', '.' );
        }
        protected static string GetName(ModuleArchNode module) {
            return WithoutPrefix( module.GetType().Name, "Module_" ).Replace( '_', '.' );
        }
        protected static string GetName(NamespaceArchNode @namespace) {
            return WithoutPrefix( @namespace.GetType().Name, "Namespace_" ).Replace( '_', '.' );
        }
        protected static string GetName(GroupArchNode group) {
            return WithoutPrefix( group.GetType().Name, "Group_" ).Replace( '_', ' ' );
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
        // Helpers/GetDescendantNodes
        private protected static IEnumerable<ArchNode> GetDescendantNodes(ProjectArchNode project) {
            return project.Modules.SelectMany( i => GetDescendantNodes( i ).Prepend( i ) );
        }
        private protected static IEnumerable<ArchNode> GetDescendantNodes(ModuleArchNode module) {
            return module.Namespaces.SelectMany( i => GetDescendantNodes( i ).Prepend( i ) );
        }
        private protected static IEnumerable<ArchNode> GetDescendantNodes(NamespaceArchNode @namespace) {
            return @namespace.Groups.SelectMany( i => GetDescendantNodes( i ).Prepend( i ) );
        }
        private protected static IEnumerable<ArchNode> GetDescendantNodes(GroupArchNode group) {
            return group.Types;
        }
        // Helpers/String
        private static string WithoutPrefix(string value, string prefix) {
            if (value.StartsWith( prefix )) return value.Substring( prefix.Length );
            return value;
        }


    }
}
