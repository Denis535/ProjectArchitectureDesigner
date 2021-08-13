// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public abstract class ArchNode {


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


    }
}
