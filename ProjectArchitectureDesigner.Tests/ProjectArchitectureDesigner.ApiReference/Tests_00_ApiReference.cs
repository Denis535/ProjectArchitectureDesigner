// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.ApiReference {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using ProjectArchitectureDesigner.Model;

    public class Tests_00_ApiReference {

        private Project_ProjectArchitectureDesigner Project { get; set; } = default!;


        [SetUp]
        public void Setup() {
            Trace.Listeners.Add( new TextWriterTraceListener( TestContext.Out ) );
            Project = new Project_ProjectArchitectureDesigner();
        }


        // Project
        [Test]
        public void Test_00_Project_IsValid() {
            foreach (var type in Project.GetTypesWithInvalidModule()) {
                Assert.Fail( "Type '{0}' has invalid module: {1}", type.Name, type.Group.Namespace.Module.Name );
            }
            foreach (var type in Project.GetTypesWithInvalidNamespace()) {
                Assert.Fail( "Type '{0}' has invalid namespace: {1}", type.Name, type.Group.Namespace.Name );
            }
        }
        [Test]
        public void Test_01_Project_IsComplete() {
            Project.GetMissingAndExtraTypes( out var missing, out var extra );
            if (missing.Any()) Assert.Warn( GetMessage_Missing( missing ) );
            if (extra.Any()) Assert.Warn( GetMessage_Extra( extra ) );
        }


        // Helpers/GetMessage
        private static string GetMessage_Missing(IEnumerable<Type> types) {
            var builder = new StringBuilder();
            builder.AppendLine( "Missing:" );
            foreach (var item in GetAssembliesNamespacesTypes( types )) {
                builder.AppendLine( item );
            }
            return builder.ToString();
        }
        private static string GetMessage_Extra(IEnumerable<TypeArchNode> types) {
            var builder = new StringBuilder();
            builder.AppendLine( "Extra:" );
            foreach (var item in GetAssembliesNamespacesTypes( types.Select( i => i.Value ) )) {
                builder.AppendLine( item );
            }
            return builder.ToString();
        }
        private static IEnumerable<string> GetAssembliesNamespacesTypes(IEnumerable<Type> types) {
            // Assembly
            foreach (var assembly in types.GroupBy( i => i.Assembly )) {
                yield return string.Format( "Assembly: {0}", assembly.Key.GetName().Name );
                // Namespace
                foreach (var @namespace in assembly.GroupBy( i => i.Namespace )) {
                    yield return string.Format( "\"{0}\",", @namespace.Key ?? "Global" );
                    // Type
                    foreach (var type in @namespace) {
                        yield return string.Format( "typeof( {0} ),", type.Name );
                    }
                }
            }
        }


    }
}