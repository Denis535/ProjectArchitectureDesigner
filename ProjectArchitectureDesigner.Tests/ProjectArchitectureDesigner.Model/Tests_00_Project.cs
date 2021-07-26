// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using ProjectArchitectureDesigner.Renderers;

    public class Tests_00_Project {

        private Project_ProjectArchitectureDesigner Project { get; set; } = default!;


        [SetUp]
        public void Setup() {
            Trace.Listeners.Add( new TextWriterTraceListener( TestContext.Out ) );
            Project = new Project_ProjectArchitectureDesigner();
        }


        // Project
        [Test]
        public void Test_00_Project_IsInitialized() {
            {
                Assert.NotNull( Project.Name );
                Assert.NotNull( Project.Modules );
            }
            foreach (var module in Project.Modules) {
                Assert.NotNull( module.Name );
                Assert.NotNull( module.Project );
                Assert.NotNull( module.Namespaces );
            }
            foreach (var @namespace in Project.Namespaces) {
                Assert.NotNull( @namespace.Name );
                Assert.NotNull( @namespace.Module );
                Assert.NotNull( @namespace.Groups );
            }
            foreach (var group in Project.Groups) {
                Assert.NotNull( group.Name );
                Assert.NotNull( group.Namespace );
                Assert.NotNull( group.Types );
            }
            foreach (var type in Project.Types) {
                Assert.NotNull( type.Value );
                Assert.NotNull( type.Name );
                Assert.NotNull( type.Group );
            }
        }
        [Test]
        public void Test_01_Project_Modules_AreValid() {
            foreach (var type in Project.Types) {
                Assert.That( type.Module.Name, Is.EqualTo( type.Value.Assembly.GetName().Name ) );
            }
        }
        [Test]
        public void Test_02_Project_Namespaces_AreValid() {
            foreach (var type in Project.Types) {
                Assert.That( type.Namespace.Name, Is.EqualTo( type.Value.Namespace ) );
            }
        }
        [Test]
        public void Test_03_Project_Types_AreComplete() {
            Project.Compare( Project.Assemblies, out _, out var missing, out var extra );
            if (missing.Any()) Assert.Warn( GetMessage_Missing( missing ) );
            if (extra.Any()) Assert.Warn( GetMessage_Extra( extra ) );
        }


        // Render
        [Test]
        public void Test_10_RenderToText() {
            TestContext.WriteLine( Project.RenderToText( INodeRenderer.TextRenderer, IsVisible ) );
        }
        [Test]
        public void Test_11_RenderToText_Aligned() {
            TestContext.WriteLine( Project.RenderToText( INodeRenderer.AlignedTextRenderer, IsVisible ) );
        }
        [Test]
        public void Test_12_RenderToHierarchicalText() {
            TestContext.WriteLine( Project.RenderToHierarchicalText( INodeRenderer.TextRenderer, IsVisible ) );
        }
        [Test]
        public void Test_13_RenderToMarkdown() {
            TestContext.WriteLine( Project.RenderToMarkdown( INodeRenderer.TextRenderer, IsVisible ) );
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
        private static string GetMessage_Extra(IEnumerable<Type> types) {
            var builder = new StringBuilder();
            builder.AppendLine( "Extra:" );
            foreach (var item in GetAssembliesNamespacesTypes( types )) {
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
        // Helpers/Type
        private static bool IsVisible(TypeArchNode type) {
            return
                type.Value.IsVisible &&
                !type.Value.Assembly.GetName().Name!.EndsWith( ".Internal" ) &&
                !type.Value.Assembly.GetName().Name!.EndsWith( ".Analyzer" );
        }


    }
}