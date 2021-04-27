// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using ProjectArchitecture.Renderer;

    public class Tests_Project {

        private FakeProject Project { get; set; } = default!;


        [SetUp]
        public void Setup() {
            Project = new FakeProject();
        }


        [Test]
        public void Test_00_Render() {
            TestContext.WriteLine( Project.Render() );
        }

        [Test]
        public void Test_00_Render_Markdown() {
            TestContext.WriteLine( Project.RenderMarkdown() );
        }


    }
}