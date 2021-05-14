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
        protected static string GetName(Node node) {
            return node.GetType().Name.Replace( '_', '.' );
        }
        protected static IEnumerable<T> GetChildren<T>(Node node) {
            return
                node
                .GetType()
                .GetProperties( BindingFlags.Public | BindingFlags.Instance )
                .Where( i => i.PropertyType.IsSubclassOf( typeof( T ) ) )
                .Select( i => i.GetValue( node ) )
                .Cast<T>();
        }


    }
}
