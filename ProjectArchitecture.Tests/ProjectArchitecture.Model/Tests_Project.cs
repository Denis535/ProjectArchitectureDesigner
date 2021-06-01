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
            foreach (var module in Project.Modules) {
                foreach (var type in module.DescendantNodes.OfType<TypeArchNode>().Select( i => i.Value )) {
                    Assert.That( type.Assembly.GetName().Name, Is.EqualTo( module.Name ) );
                }
            }
        }
        [Test]
        public void Test_00_Project_Namespaces() {
            foreach (var @namespace in Project.DescendantNodes.OfType<NamespaceArchNode>()) {
                foreach (var type in @namespace.DescendantNodes.OfType<TypeArchNode>().Select( i => i.Value )) {
                    Assert.That( type.Namespace, Is.EqualTo( @namespace.Name ) );
                }
            }
        }
        [Test]
        public void Test_00_Project_Types() {
            Project.Compare( Assemblies, out var intersected, out var missing, out var extra );
            if (missing.Any()) {
                TestContext.WriteLine( GetDisplayString( Project.GetSupportedTypes( Assemblies ) ) );
                Assert.Warn( "Missing types: {0}", missing.Select( i => i.ToString() ).Join() );
            }
            if (extra.Any()) {
                TestContext.WriteLine( GetDisplayString( Project.GetSupportedTypes( Assemblies ) ) );
                Assert.Warn( "Extra types: {0}", extra.Select( i => i.ToString() ).Join() );
            }
        }


        // Rendering
        [Test]
        public void Test_01_Rendering_Text() {
            TestContext.WriteLine( Project.RenderToText() );
        }
        [Test]
        public void Test_01_Rendering_HierarchicalText() {
            TestContext.WriteLine( Project.RenderToHierarchicalText() );
        }
        [Test]
        public void Test_01_Rendering_Markdown() {
            TestContext.WriteLine( Project.RenderToMarkdown() );
        }


        // Helpers/GetDisplayString
        private static string GetDisplayString(Type[] types) {
            var builder = new StringBuilder();
            // Assemblies
            foreach (var assembly in types.GroupBy( i => i.Assembly.GetName().Name )) {
                builder.AppendLineFormat( "Module: \"{0}\",", assembly.Key );
                // Namespaces
                foreach (var @namespace in assembly.GroupBy( i => i.Namespace )) {
                    builder.AppendLineFormat( "\"{0}\",", @namespace.Key );
                    // Types
                    foreach (var type in @namespace) {
                        builder.AppendLineFormat( "typeof( {0} ),", type.Name );
                    }
                }
            }
            return builder.ToString();
        }


    }
}