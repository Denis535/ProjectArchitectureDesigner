// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ProjectArchitecture.Model;

    public class FakeProject : Project {
        public override string Name => "FakeProject";
        public override Module[] Modules => new Module[] {
            new FakeModule_Domain(),
            new FakeModule_Infrastructure(),
        };
    }

    public class FakeModule_Domain : Module {
        public override string Name => "FakeProject.Domain";
        public override Namespace[] Namespaces => new Node[] {
            "FakeProject".AsNamespace(),
            typeof(object),
            typeof(object),
            "Group 0",
            typeof(object),
            typeof(object),
            "Group 1",
            typeof(object),
            typeof(object),
        }.ToNamespacesHierarchy();
    }

    public class FakeModule_Infrastructure : Module {
        public override string Name => "FakeProject.Infrastructure";
        public override Namespace[] Namespaces => new Node[] {
            typeof(object),
            typeof(object),
            "System".AsNamespace(),
            typeof(object),
            typeof(object),
            "System.Linq".AsNamespace(),
            typeof(object),
            typeof(object),
        }.ToNamespacesHierarchy();
    }

}
