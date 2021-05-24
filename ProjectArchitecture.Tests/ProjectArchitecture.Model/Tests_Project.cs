// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using ProjectArchitecture.Renderers;

    public class Tests_Project {

        private Project_ProjectArchitecture Project { get; set; } = default!;


        [SetUp]
        public void Setup() {
            Trace.Listeners.Add( new TextWriterTraceListener( TestContext.Out ) );
            Project = new Project_ProjectArchitecture();
        }


        // Project
        [Test]
        public void Test_00_Project() {
            var assemblies = new[] {
                typeof( Node ).Assembly,
                typeof( SourceGenerator ).Assembly
            };
            Project.Compare( assemblies, out var intersected, out var missing, out var extra );
            if (missing.Any()) Assert.Warn( "Missing types: {0}", missing.Select( i => i.ToString() ).Join() );
            if (extra.Any()) Assert.Warn( "Extra types: {0}", extra.Select( i => i.ToString() ).Join() );
        }


        // Rendering
        [Test]
        public void Test_01_Rendering() {
            TestContext.WriteLine( Project.Render() );
        }
        [Test]
        public void Test_01_Rendering_Markdown() {
            TestContext.WriteLine( Project.RenderMarkdown() );
        }


    }
}