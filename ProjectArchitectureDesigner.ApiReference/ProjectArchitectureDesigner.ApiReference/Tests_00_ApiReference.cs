// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.ApiReference {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using ProjectArchitectureDesigner.Model;
    using ProjectArchitectureDesigner.Model.Testing;

    public class Tests_00_ApiReference {

        private Project_ProjectArchitectureDesigner Project { get; set; } = default!;


        [SetUp]
        public void Setup() {
            Trace.Listeners.Add( new TextWriterTraceListener( TestContext.Out ) );
            Project = new Project_ProjectArchitectureDesigner();
        }


        [Test]
        public void Test_00_Project_ShouldBe_Complete() {
            var types = Project.GetMissingTypes();
            if (types.Any()) Assert.Warn( ProjectTestingUtils.GetMessage_ProjectHasMissingTypes( Project, types ) );
        }
        [Test]
        public void Test_01_Project_ShouldHaveNo_ExtraTypes() {
            var types = Project.GetExtraTypes();
            if (types.Any()) Assert.Warn( ProjectTestingUtils.GetMessage_ProjectHasExtraTypes( Project, types ) );
        }
        [Test]
        public void Test_02_Modules_ShouldHaveNo_ExtraTypes() {
            foreach (var module in Project.Modules) {
                var types = module.GetExtraTypes();
                if (types.Any()) Assert.Warn( ProjectTestingUtils.GetMessage_ModuleHasExtraTypes( module, types ) );
            }
        }
        [Test]
        public void Test_03_Namespaces_ShouldHaveNo_ExtraTypes() {
            foreach (var @namespace in Project.GetNamespaces()) {
                var types = @namespace.GetExtraTypes();
                if (types.Any()) Assert.Warn( ProjectTestingUtils.GetMessage_NamespaceHasExtraTypes( @namespace, types ) );
            }
        }


    }
}