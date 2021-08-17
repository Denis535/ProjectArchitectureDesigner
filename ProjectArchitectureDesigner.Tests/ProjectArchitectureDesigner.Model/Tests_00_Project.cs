// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using ProjectArchitectureDesigner.Model.Renderers;

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
            foreach (var @namespace in Project.GetNamespaces()) {
                Assert.NotNull( @namespace.Name );
                Assert.NotNull( @namespace.Module );
                Assert.NotNull( @namespace.Groups );
            }
            foreach (var group in Project.GetGroups()) {
                Assert.NotNull( group.Name );
                Assert.NotNull( group.Namespace );
                Assert.NotNull( group.Types );
            }
            foreach (var type in Project.GetTypes()) {
                Assert.NotNull( type.Value );
                Assert.NotNull( type.Name );
                Assert.NotNull( type.Group );
            }
        }
        [Test]
        public void Test_01_Types_AreValid() {
            foreach (var type in Project.GetTypesWithInvalidModule()) {
                Assert.Fail( "Type '{0}' has invalid module: {1}", type.Name, type.Group.Namespace.Module.Name );
            }
            foreach (var type in Project.GetTypesWithInvalidNamespace()) {
                Assert.Fail( "Type '{0}' has invalid namespace: {1}", type.Name, type.Group.Namespace.Name );
            }
        }
        [Test]
        public void Test_03_Types_AreComplete() {
            Project.GetMissingAndExtraTypes( out var missing, out var extra );
            if (missing.Any()) Assert.Warn( GetMessage_Missing( missing ) );
            if (extra.Any()) Assert.Warn( GetMessage_Extra( extra ) );
        }


        // Render
        [Test]
        public void Test_10_Render_Text() {
            var renderer = new TextNodeRenderer( null );
            var renderer2 = new TextProjectRenderer( renderer );
            var text = renderer2.Render( Project.WithVisibleOnly() ).WithCodeBlock().ToString();
            TestContext.WriteLine( text );
        }
        [Test]
        public void Test_11_Render_Text_LeftAligned() {
            var renderer = new LeftAlignedTextNodeRenderer();
            var renderer2 = new TextProjectRenderer( renderer );
            var text = renderer2.Render( Project.WithVisibleOnly() ).WithCodeBlock().ToString();
            TestContext.WriteLine( text );
        }
        [Test]
        public void Test_12_Render_Text_RightAligned() {
            var renderer = new RightAlignedTextNodeRenderer();
            var renderer2 = new TextProjectRenderer( renderer );
            var text = renderer2.Render( Project.WithVisibleOnly() ).WithCodeBlock().ToString();
            TestContext.WriteLine( text );
        }
        [Test]
        public void Test_13_Render_Text_Hierarchical() {
            var renderer = new HierarchyNodeHighlighter( new TextNodeRenderer( new MarkdownNodeHighlighter() ) );
            var renderer2 = new TextProjectRenderer( renderer );
            var text = renderer2.Render( Project.WithVisibleOnly() ).WithCodeBlock().ToString();
            TestContext.WriteLine( text );
        }
        [Test]
        public void Test_14_Render_MarkdownDocument() {
            var renderer2 = new MarkdownDocumentProjectRenderer();
            var text = renderer2.Render( Project.WithVisibleOnly() ).ToString();
            TestContext.WriteLine( text );
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