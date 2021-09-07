// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NUnit.Framework;
    using ProjectArchitectureDesigner.ApiReference;
    using ProjectArchitectureDesigner.Model;

    public class Tests_00_Project {


        [Test]
        public void Test_00_Creation() {
            var project = new ProjectArchNode(
                new[] { typeof( object ).Assembly }, "Project", new ModuleArchNode(
                    typeof( object ).Assembly, "Module", new NamespaceArchNode(
                        "Namespace", new GroupArchNode(
                            "Group", new TypeArchNode( typeof( object ) )
                        )
                    )
                )
            );
            {
                Assert.NotNull( project.Assemblies, "Project.Assemblies is null" );
                Assert.NotNull( project.Name, "Project.Name is null" );
                Assert.NotNull( project.Modules, "Project.Modules is null" );
            }
            foreach (var module in project.Modules) {
                Assert.NotNull( module.Assembly, "Module.Assembly is null" );
                Assert.NotNull( module.Project, "Module.Project is null" );
                Assert.NotNull( module.Name, "Module.Name is null" );
                Assert.NotNull( module.Namespaces, "Module.Namespaces is null" );
            }
            foreach (var @namespace in project.GetNamespaces()) {
                Assert.NotNull( @namespace.Name, "Namespace.Name is null" );
                Assert.NotNull( @namespace.Module, "Namespace.Module is null" );
                Assert.NotNull( @namespace.Groups, "Namespace.Groups is null" );
            }
            foreach (var group in project.GetGroups()) {
                Assert.NotNull( group.Name, "Group.Name is null" );
                Assert.NotNull( group.Namespace, "Group.Namespace is null" );
                Assert.NotNull( group.Types, "Group.Types is null" );
            }
            foreach (var type in project.GetTypes()) {
                Assert.NotNull( type.Value, "Type.Value is null" );
                Assert.NotNull( type.Name, "Type.Name is null" );
                Assert.NotNull( type.Group, "Type.Group is null" );
            }
        }
        [Test]
        public void Test_01_Creation_Generated() {
            var project = new Project_ProjectArchitectureDesigner();
            {
                Assert.NotNull( project.GetAssemblies(), "Project.Assemblies is null" );
                Assert.NotNull( project.Name, "Project.Name is null" );
                Assert.NotNull( project.Modules, "Project.Modules is null" );
            }
            foreach (var module in project.Modules) {
                Assert.NotNull( module.Assembly, "Module.Assembly is null" );
                Assert.NotNull( module.Project, "Module.Project is null" );
                Assert.NotNull( module.Name, "Module.Name is null" );
                Assert.NotNull( module.Namespaces, "Module.Namespaces is null" );
            }
            foreach (var @namespace in project.GetNamespaces()) {
                Assert.NotNull( @namespace.Module, "Namespace.Module is null" );
                Assert.NotNull( @namespace.Name, "Namespace.Name is null" );
                Assert.NotNull( @namespace.Groups, "Namespace.Groups is null" );
            }
            foreach (var group in project.GetGroups()) {
                Assert.NotNull( group.Namespace, "Group.Namespace is null" );
                Assert.NotNull( group.Name, "Group.Name is null" );
                Assert.NotNull( group.Types, "Group.Types is null" );
            }
            foreach (var type in project.GetTypes()) {
                Assert.NotNull( type.Group, "Type.Group is null" );
                Assert.NotNull( type.Value, "Type.Value is null" );
                Assert.NotNull( type.Name, "Type.Name is null" );
            }
        }


    }
}