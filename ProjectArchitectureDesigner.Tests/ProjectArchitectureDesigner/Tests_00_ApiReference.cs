// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using ProjectArchitectureDesigner.ApiReference;
    using ProjectArchitectureDesigner.Model.Testing;

    public class Tests_00_ApiReference {

        private Project_ProjectArchitectureDesigner Project { get; set; } = default!;


        [SetUp]
        public void Setup() {
            Trace.Listeners.Add( new TextWriterTraceListener( TestContext.Out ) );
            Project = new Project_ProjectArchitectureDesigner();
        }


        // Project
        [Test]
        public void Test_00_Project_IsValid() {
            foreach (var type in Project.GetTypesWithInvalidModule()) {
                Assert.Fail( ProjectTestingUtils.GetMessage_ProjectHasTypeWithInvalidModule( type ) );
            }
            foreach (var type in Project.GetTypesWithInvalidNamespace()) {
                Assert.Fail( ProjectTestingUtils.GetMessage_ProjectHasTypeWithInvalidNamespace( type ) );
            }
        }
        [Test]
        public void Test_01_Project_IsComplete() {
            Project.GetMissingAndExtraTypes( out var missing, out var extra );
            if (missing.Any()) Assert.Warn( ProjectTestingUtils.GetMessage_ProjectHasMissingTypes( missing ) );
            if (extra.Any()) Assert.Warn( ProjectTestingUtils.GetMessage_ProjectHasExtraTypes( extra ) );
        }


    }
}