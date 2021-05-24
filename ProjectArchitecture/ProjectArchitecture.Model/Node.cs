// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public abstract class Node {

        public abstract string Name { get; }


        // Utils
        public override string ToString() {
            return "Node: " + Name;
        }


        // Helpers
        private protected static string GetName(ProjectNode node) {
            return WithoutPrefix( node.GetType().Name, "Project_" ).Replace( '_', '.' );
        }
        private protected static string GetName(ModuleNode node) {
            return WithoutPrefix( node.GetType().Name, "Module_" ).Replace( '_', '.' );
        }
        private protected static string GetName(NamespaceNode node) {
            return WithoutPrefix( node.GetType().Name, "Namespace_" ).Replace( '_', '.' );
        }
        private protected static string GetName(GroupNode node) {
            return WithoutPrefix( node.GetType().Name, "Group_" ).Replace( '_', ' ' );
        }
        private protected static IEnumerable<T> GetChildren<T>(Node node) {
            return
                node
                .GetType()
                .GetProperties( BindingFlags.Public | BindingFlags.Instance )
                .Where( IsOfType<T> )
                .Select( i => i.GetValue( node ) )
                .Cast<T>();
        }
        // Helpers/Misc
        private static bool IsOfType<T>(PropertyInfo property) {
            return
                property.PropertyType.Equals( typeof( T ) ) ||
                property.PropertyType.IsSubclassOf( typeof( T ) );
        }
        private static string WithoutPrefix(string value, string prefix) {
            var i = value.IndexOf( prefix );
            if (i != -1) value = value[ (i + prefix.Length).. ];
            return value;
        }


    }
}
