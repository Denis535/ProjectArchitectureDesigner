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
            typeof( SourceGenerator ).Assembly,
            typeof( Option ).Assembly,
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
                Assert.Warn( GetMessage_Missing( missing ) );
            }
            if (extra.Any()) {
                Assert.Warn( GetMessage_Extra( extra ) );
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
        private static string GetMessage_Missing(IEnumerable<Type> types) {
            var builder = new StringBuilder();
            builder.AppendLine( "Missing:" );
            foreach (var item in GetHierarchy( types )) {
                builder.AppendLine( GetItemString( item ) );
            }
            return builder.ToString();
        }
        private static string GetMessage_Extra(IEnumerable<Type> types) {
            var builder = new StringBuilder();
            builder.AppendLine( "Extra:" );
            foreach (var item in GetHierarchy( types )) {
                builder.AppendLine( GetItemString( item ) );
            }
            return builder.ToString();
        }
        private static IEnumerable<object> GetHierarchy(IEnumerable<Type> types) {
            foreach (var assembly in types.GroupBy( i => i.Assembly )) {
                yield return assembly.Key;
                foreach (var @namespace in assembly.GroupBy( i => i.Namespace )) {
                    yield return @namespace.Key ?? "";
                    foreach (var type in @namespace) {
                        yield return type;
                    }
                }
            }
        }
        private static string GetItemString(object item) {
            if (item is Assembly assembly) {
                return string.Format( "Assembly: {0}", assembly.GetName().Name );
            }
            if (item is string @namespace) {
                return string.Format( "\"{0}\",", @namespace );
            }
            if (item is Type type) {
                return string.Format( "typeof( {0} ),", type.Name );
            }
            throw new ArgumentException( "Item is invalid: " + item?.ToString() ?? "null" );
        }


    }
}