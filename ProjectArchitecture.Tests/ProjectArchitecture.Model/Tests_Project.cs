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
            foreach (var @namespace in Project.Modules.SelectMany( i => i.Namespaces )) {
                Assert.NotNull( @namespace.Name );
                Assert.NotNull( @namespace.Module );
                Assert.NotNull( @namespace.Groups );
            }
            foreach (var group in Project.Modules.SelectMany( i => i.Namespaces ).SelectMany( i => i.Groups )) {
                Assert.NotNull( group.Name );
                Assert.NotNull( group.Namespace );
                Assert.NotNull( group.Types );
            }
            foreach (var type in Project.Modules.SelectMany( i => i.Namespaces ).SelectMany( i => i.Groups ).SelectMany( i => i.Types )) {
                Assert.NotNull( type.Value );
                Assert.NotNull( type.Name );
                Assert.NotNull( type.Group );
            }
        }
        [Test]
        public void Test_01_Modules_AreValid() {
            foreach (var type in Project.DescendantNodes.OfType<TypeArchNode>()) {
                Assert.That( type.Module.Name, Is.EqualTo( type.Value.Assembly.GetName().Name ) );
            }
        }
        [Test]
        public void Test_02_Namespaces_AreValid() {
            foreach (var type in Project.DescendantNodes.OfType<TypeArchNode>()) {
                Assert.That( type.Namespace.Name, Is.EqualTo( type.Value.Namespace ) );
            }
        }
        [Test]
        public void Test_03_Types_AreComplete() {
            Project.Compare( Assemblies, out _, out var missing, out var extra );
            if (missing.Any()) {
                Assert.Warn( GetMessage_Missing( missing ) );
            }
            if (extra.Any()) {
                Assert.Warn( GetMessage_Extra( extra ) );
            }
        }


        // Rendering
        [Test]
        public void Test_10_Rendering_AlignedText() {
            TestContext.WriteLine( Project.RenderToAlignedText() );
        }
        [Test]
        public void Test_11_Rendering_HierarchicalText() {
            TestContext.WriteLine( Project.RenderToHierarchicalText() );
        }
        [Test]
        public void Test_21_Rendering_Markdown() {
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