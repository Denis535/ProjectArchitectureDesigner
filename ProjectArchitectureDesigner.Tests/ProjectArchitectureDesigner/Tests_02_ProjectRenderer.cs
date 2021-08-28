// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NUnit.Framework;
    using ProjectArchitectureDesigner.ApiReference;
    using ProjectArchitectureDesigner.Model;
    using ProjectArchitectureDesigner.Model.Renderers;

    public class Tests_02_ProjectRenderer {


        // Text/Color
        [Test]
        public void Test_10_ColorText() {
            var project = new Project_ProjectArchitectureDesigner().WithVisibleOnly();
            var renderer = new ColorTextProjectRenderer( NodeRenderer.Create<TextRenderer>() );
            var text = renderer.Render( project ).ToString();
            TestContext.WriteLine( text );
        }
        [Test]
        public void Test_11_ColorText_LeftAligned() {
            var project = new Project_ProjectArchitectureDesigner().WithVisibleOnly();
            var renderer = new ColorTextProjectRenderer( NodeRenderer.Create<LeftAlignedTextRenderer>() );
            var text = renderer.Render( project ).ToString();
            TestContext.WriteLine( text );
        }
        [Test]
        public void Test_12_ColorText_RightAligned() {
            var project = new Project_ProjectArchitectureDesigner().WithVisibleOnly();
            var renderer = new ColorTextProjectRenderer( NodeRenderer.Create<RightAlignedTextRenderer>() );
            var text = renderer.Render( project ).ToString();
            TestContext.WriteLine( text );
        }
        [Test]
        public void Test_13_ColorText_Hierarchy() {
            var project = new Project_ProjectArchitectureDesigner().WithVisibleOnly();
            var renderer = new ColorTextProjectRenderer( NodeRenderer.Create<HierarchyHighlighter, TextRenderer>() );
            var text = renderer.Render( project ).ToString();
            TestContext.WriteLine( text );
        }


        // Markdown
        [Test]
        public void Test_20_Markdown() {
            var project = new Project_ProjectArchitectureDesigner().WithVisibleOnly();
            var renderer = new MarkdownProjectRenderer( NodeRenderer.Create<TextRenderer, MarkdownHighlighter>() );
            var text = renderer.Render( project ).ToString();
            TestContext.WriteLine( text );
        }
        [Test]
        public void Test_21_MarkdownDocument() {
            var project = new Project_ProjectArchitectureDesigner().WithVisibleOnly();
            var renderer = new MarkdownDocumentProjectRenderer();
            var text = renderer.Render( project ).ToString();
            TestContext.WriteLine( text );
        }


    }
}
