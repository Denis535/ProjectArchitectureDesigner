// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model.Testing {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class ProjectTestingUtils {


        // GetTypes
        public static Type[] GetMissingTypes(this ProjectArchNode project) {
            var actual = project.GetTypes();
            var expected_ = new LinkedList<Type>( project.GetAssemblies().SelectMany( i => i.DefinedTypes ).Where( project.IsSupported ) );
            foreach (var actual_ in actual) {
                expected_.Remove( actual_.Value );
            }
            return expected_.ToArray();
        }
        public static TypeArchNode[] GetExtraTypes(this ProjectArchNode project) {
            var actual_ = new LinkedList<TypeArchNode>( project.GetTypes() );
            var expected = project.GetAssemblies().SelectMany( i => i.DefinedTypes ).Where( project.IsSupported );
            foreach (var expected_ in expected) {
                actual_.Remove( i => i.Value == expected_ );
            }
            return actual_.ToArray();
        }
        public static IEnumerable<TypeArchNode> GetTypesWithInvalidModule(this ProjectArchNode project) {
            foreach (var module in project.Modules) {
                foreach (var type in module.GetTypes()) {
                    if (type.Value.Assembly.GetName().Name != module.Name) {
                        yield return type;
                    }
                }
            }
        }
        public static IEnumerable<TypeArchNode> GetTypesWithInvalidNamespace(this ProjectArchNode project) {
            foreach (var @namespace in project.GetNamespaces()) {
                foreach (var type in @namespace.GetTypes()) {
                    if (type.Value.Namespace != @namespace.Name) {
                        yield return type;
                    }
                }
            }
        }


        // GetMessage
        public static string GetMessage_ProjectHasMissingTypes(IEnumerable<Type> types) {
            var builder = new StringBuilder();
            builder.AppendLine( "Project has missing types:" );
            builder.AppendTypes( types );
            return builder.ToString();
        }
        public static string GetMessage_ProjectHasExtraTypes(IEnumerable<TypeArchNode> types) {
            var builder = new StringBuilder();
            builder.AppendLine( "Project has extra types:" );
            builder.AppendTypes( types );
            return builder.ToString();
        }
        public static string GetMessage_ProjectHasTypeWithInvalidModule(IEnumerable<TypeArchNode> types) {
            var builder = new StringBuilder();
            builder.AppendLine( "Project has types with invalid module:" );
            builder.AppendTypes( types );
            return builder.ToString();
        }
        public static string GetMessage_ProjectHasTypeWithInvalidNamespace(IEnumerable<TypeArchNode> types) {
            var builder = new StringBuilder();
            builder.AppendLine( "Project has types with invalid namespace:" );
            builder.AppendTypes( types );
            return builder.ToString();
        }


        // Helpers/StringBuilder
        private static void AppendTypes(this StringBuilder builder, IEnumerable<TypeArchNode> types) {
            // Module
            foreach (var module in types.GroupBy( i => i.GetModule() )) {
                builder.AppendLineFormat( "Module: {0}", module.Key.Name );
                // Namespace
                foreach (var @namespace in module.GroupBy( i => i.GetNamespace() )) {
                    builder.AppendLineFormat( "  \"{0}\",", @namespace.Key.Name );
                    // Type
                    foreach (var type in @namespace) {
                        builder.AppendLineFormat( "    typeof( {0} ),", type.Name );
                    }
                }
            }
        }
        private static void AppendTypes(this StringBuilder builder, IEnumerable<Type> types) {
            // Assembly
            foreach (var assembly in types.GroupBy( i => i.Assembly )) {
                builder.AppendLineFormat( "Assembly: {0}", assembly.Key.GetName().Name );
                // Namespace
                foreach (var @namespace in assembly.GroupBy( i => i.Namespace )) {
                    builder.AppendLineFormat( "  \"{0}\",", @namespace.Key );
                    // Type
                    foreach (var type in @namespace) {
                        builder.AppendLineFormat( "    typeof( {0} ),", type.Name );
                    }
                }
            }
        }
        // Helpers/List
        private static bool Remove<T>(this LinkedList<T> list, Predicate<T> predicate) {
            var node = list.First;
            while (node != null) {
                if (predicate( node.Value )) {
                    list.Remove( node );
                    return true;
                }
                node = node.Next;
            }
            return false;
        }


    }
}
