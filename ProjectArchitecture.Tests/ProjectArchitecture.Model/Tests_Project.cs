// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using NUnit.Framework;
    using ProjectArchitecture.Renderers;

    public class Tests_Project {

        private static Assembly[] Assemblies { get; } = new[] {
            typeof( ArchNode ).Assembly,
            typeof( SourceGenerator ).Assembly
        };
        private Project_ProjectArchitecture Project { get; set; } = default!;


        [SetUp]
        public void Setup() {
            Trace.Listeners.Add( new TextWriterTraceListener( TestContext.Out ) );
            Project = new Project_ProjectArchitecture();
        }


        // Project
        [Test]
        public void Test_00_Project_Modules() {
            foreach (var type in Project.DescendantNodes.OfType<TypeArchNode>()) {
                Assert.That( type.Value.Assembly.GetName().Name, Is.EqualTo( type.Module.Name ) );
            }
        }
        [Test]
        public void Test_00_Project_Namespaces() {
            foreach (var type in Project.DescendantNodes.OfType<TypeArchNode>()) {
                Assert.That( type.Value.Namespace, Is.EqualTo( type.Namespace.Name ) );
            }
        }
        [Test]
        public void Test_00_Project_Types() {
            Project.Compare( Assemblies, out var intersected, out var missing, out var extra );
            if (missing.Any()) {
                Assert.Warn( "Missing types: {0}", missing.Select( i => i.ToString() ).Join() );
                TestContext.WriteLine( GetMessage_SupportedTypes( Project.GetSupportedTypes( Assemblies ) ) );
            }
            if (extra.Any()) {
                Assert.Warn( "Extra types: {0}", extra.Select( i => i.ToString() ).Join() );
                TestContext.WriteLine( GetMessage_SupportedTypes( Project.GetSupportedTypes( Assemblies ) ) );
            }
        }


        // Rendering
        [Test]
        public void Test_01_Rendering_AlignedText() {
            TestContext.WriteLine( Project.RenderToAlignedText() );
        }
        [Test]
        public void Test_01_Rendering_HierarchicalText() {
            TestContext.WriteLine( Project.RenderToHierarchicalText() );
        }
        [Test]
        public void Test_01_Rendering_Markdown() {
            TestContext.WriteLine( Project.RenderToMarkdown() );
        }


        // Helpers/GetMessage
        private static string GetMessage_SupportedTypes(IEnumerable<Type> types) {
            var builder = new StringBuilder();
            builder.AppendLine( "Supported types:" );
            foreach (var assembly in types.GroupBy( i => i.Assembly.GetName().Name )) {
                builder.AppendLineFormat( "Assembly: {0}", assembly.Key );
                foreach (var @namespace in assembly.GroupBy( i => i.Namespace )) {
                    builder.AppendLineFormat( "\"{0}\",", @namespace.Key );
                    foreach (var type in @namespace) {
                        builder.AppendLineFormat( "typeof( {0} ),", type.Name );
                    }
                }
            }
            return builder.ToString();
        }


    }
}