// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using NUnit.Framework;
    using ProjectArchitectureDesigner.ApiReference;
    using ProjectArchitectureDesigner.Model.Renderers;

    public class Tests_00_ProjectArchNode {

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
                Assert.NotNull( Project.Name, "Project.Name is null" );
                Assert.NotNull( Project.Modules, "Project.Modules is null" );
            }
            foreach (var module in Project.Modules) {
                Assert.NotNull( module.Name, "Module.Name is null" );
                Assert.NotNull( module.Project, "Module.Project is null" );
                Assert.NotNull( module.Namespaces, "Module.Namespaces is null" );
            }
            foreach (var @namespace in Project.GetNamespaces()) {
                Assert.NotNull( @namespace.Name, "Namespace.Name is null" );
                Assert.NotNull( @namespace.Module, "Namespace.Module is null" );
                Assert.NotNull( @namespace.Groups, "Namespace.Groups is null" );
            }
            foreach (var group in Project.GetGroups()) {
                Assert.NotNull( group.Name, "Group.Name is null" );
                Assert.NotNull( group.Namespace, "Group.Namespace is null" );
                Assert.NotNull( group.Types, "Group.Types is null" );
            }
            foreach (var type in Project.GetTypes()) {
                Assert.NotNull( type.Value, "Type.Value is null" );
                Assert.NotNull( type.Name, "Type.Name is null" );
                Assert.NotNull( type.Group, "Type.Group is null" );
            }
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


    }
}