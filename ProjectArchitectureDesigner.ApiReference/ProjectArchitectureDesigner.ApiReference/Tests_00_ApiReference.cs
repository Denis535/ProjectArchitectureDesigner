// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.ApiReference {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using ProjectArchitectureDesigner.Model.Testing;

    public class Tests_00_ApiReference {

        private Project_ProjectArchitectureDesigner Project { get; set; } = default!;


        [SetUp]
        public void Setup() {
            Trace.Listeners.Add( new TextWriterTraceListener( TestContext.Out ) );
            Project = new Project_ProjectArchitectureDesigner();
        }


        [Test]
        public void Test_00_Project_IsComplete_Missing() {
            var types = Project.GetMissingTypes();
            if (types.Any()) Assert.Warn( ProjectTestingUtils.GetMessage_ProjectHasMissingTypes( types ) );
        }
        [Test]
        public void Test_01_Project_IsComplete_Extra() {
            var types = Project.GetExtraTypes();
            if (types.Any()) Assert.Warn( ProjectTestingUtils.GetMessage_ProjectHasExtraTypes( types ) );
        }
        [Test]
        public void Test_02_Project_IsValid_Module() {
            var types = Project.GetTypesWithInvalidModule();
            if (types.Any()) Assert.Fail( ProjectTestingUtils.GetMessage_ProjectHasTypeWithInvalidModule( types ) );
        }
        [Test]
        public void Test_03_Project_IsValid_Namespace() {
            var types = Project.GetTypesWithInvalidNamespace();
            if (types.Any()) Assert.Fail( ProjectTestingUtils.GetMessage_ProjectHasTypeWithInvalidNamespace( types ) );
        }


    }
}