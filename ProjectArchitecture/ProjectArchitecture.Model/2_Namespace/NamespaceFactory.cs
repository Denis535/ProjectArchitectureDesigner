//// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
//// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

//namespace ProjectArchitecture.Model {
//    using System;
//    using System.Collections;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Text;

//    public static class NamespaceFactory {


//        public static Namespace AsNamespace(this string name) {
//            return new Namespace( name );
//        }


//        public static Namespace[] ToNamespaces(this Node[] nodes) {
//            return nodes.ToHierarchy<Namespace, Node>().Select( i => GetNamespace( i.Key, i.Values ) ).ToArray();
//        }
//        private static Namespace GetNamespace(Namespace? @namespace, IList<Node> nodes) {
//            return new Namespace( @namespace?.Name ?? "Global", nodes.ToGroups() );
//        }
//        private static Group[] ToGroups(this IList<Node> nodes) {
//            return nodes.ToHierarchy<Group, TypeItem>().Select( i => GetGroup( i.Key, i.Values ) ).ToArray();
//        }
//        private static Group GetGroup(Group? group, IList<TypeItem> nodes) {
//            return new Group( group?.Name ?? "Default", nodes.ToArray() );
//        }


//        // Helpers
//        private static IEnumerable<(TKey? Key, IList<TValue> Values)> ToHierarchy<TKey, TValue>(this IEnumerable<object> enumerable) {
//            var enumerator = enumerable.GetEnumerator();
//            var hasNext = enumerator.MoveNext();
//            while (hasNext) {
//                var key = default( TKey );
//                var values = new List<TValue>();
//                if (enumerator.Current is TKey key_) {
//                    key = key_;
//                    hasNext = enumerator.MoveNext();
//                }
//                while (hasNext && enumerator.Current is not TKey) {
//                    values.Add( (TValue) enumerator.Current );
//                    hasNext = enumerator.MoveNext();
//                }
//                yield return (key, values.ToArray());
//            }
//        }


//    }
//}
