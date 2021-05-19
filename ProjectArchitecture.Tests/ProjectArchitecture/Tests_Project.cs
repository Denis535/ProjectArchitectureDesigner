// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using NUnit.Framework;
    using ProjectArchitecture.Model;
    using ProjectArchitecture.Renderers;

    public class Tests_Project {

        private FakeProject Project { get; set; } = default!;


        [SetUp]
        public void Setup() {
            Trace.Listeners.Add( new TextWriterTraceListener( TestContext.Out ) );
            Project = new FakeProject();
        }


        [Test]
        public void Test_00() {
            Assert.AreEqual( Project.Domain.FakeProject.Default.@object.Type, typeof( object ) );
            Assert.AreEqual( Project.Infrastructure.Global.Default.@object.Type, typeof( object ) );
            Assert.AreEqual( Project.Infrastructure.System.Group_0.@object.Type, typeof( object ) );
            Assert.AreEqual( Project.Infrastructure.System.Group_1.@string.Type, typeof( string ) );
        }

        [Test]
        public void Test_01_Render() {
            TestContext.WriteLine( Project.Render() );
        }

        [Test]
        public void Test_01_Render_Markdown() {
            TestContext.WriteLine( Project.RenderMarkdown() );
        }


    }
}