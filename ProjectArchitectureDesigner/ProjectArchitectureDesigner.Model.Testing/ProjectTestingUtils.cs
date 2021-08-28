// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model.Testing {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class ProjectTestingUtils {


        // GetTypes/WithInvalidModule
        public static IEnumerable<TypeArchNode> GetTypesWithInvalidModule(this ProjectArchNode project) {
            foreach (var type in project.GetTypes()) {
                if (type.GetModule().Name != type.Value.Assembly.GetName().Name) {
                    yield return type;
                }
            }
        }
        // GetTypes/WithInvalidNamespace
        public static IEnumerable<TypeArchNode> GetTypesWithInvalidNamespace(this ProjectArchNode project) {
            foreach (var type in project.GetTypes()) {
                if (type.GetNamespace().Name != type.Value.Namespace) {
                    yield return type;
                }
            }
        }
        // GetTypes/MissingAndExtra
        public static void GetMissingAndExtraTypes(this ProjectArchNode project, out Type[] missing, out TypeArchNode[] extra) {
            var actual = project.GetTypes();
            var expected = project.GetAssemblies().SelectMany( i => i.DefinedTypes ).Where( project.IsSupported );
            var expected_ = new LinkedList<Type>( expected );
            extra = actual.Where( i => !expected_.Remove( i.Value ) ).ToArray();
            missing = expected_.ToArray();
        }


        // GetMessage
        public static string GetMessage_ProjectHasTypeWithInvalidModule(TypeArchNode type) {
            return "Project has type '{0}' with invalid module: {1}".Format( type.Name, type.GetModule().Name );
        }
        public static string GetMessage_ProjectHasTypeWithInvalidNamespace(TypeArchNode type) {
            return "Project has type '{0}' with invalid namespace: {1}".Format( type.Name, type.GetNamespace().Name );
        }
        public static string GetMessage_ProjectHasMissingTypes(IEnumerable<Type> types) {
            var builder = new StringBuilder();
            builder.AppendLine( "Project has missing types:" );
            builder.AppendTypes( types );
            return builder.ToString();
        }
        public static string GetMessage_ProjectHasExtraTypes(IEnumerable<TypeArchNode> types) {
            var builder = new StringBuilder();
            builder.AppendLine( "Project has extra types:" );
            builder.AppendTypes( types.Select( i => i.Value ) );
            return builder.ToString();
        }


        // Helpers
        private static void AppendTypes(this StringBuilder builder, IEnumerable<Type> types) {
            // Assembly
            foreach (var assembly in types.GroupBy( i => i.Assembly )) {
                builder.AppendLineFormat( "Assembly: {0}", assembly.Key.GetName().Name );
                // Namespace
                foreach (var @namespace in assembly.GroupBy( i => i.Namespace )) {
                    builder.AppendLineFormat( "\"{0}\",", @namespace.Key ?? "Global" );
                    // Type
                    foreach (var type in @namespace) {
                        builder.AppendLineFormat( "typeof( {0} ),", type.Name );
                    }
                }
            }
        }


    }
}
