// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;

    // FakeProject
    public partial class FakeProject : ProjectNode {
        public FakeProject() => SetModules(
            typeof( FakeModule_Domain ),
            typeof( FakeModule_Infrastructure )
        );
    }

    // Modules/Domain
    public partial class FakeModule_Domain : ModuleNode {
        public FakeModule_Domain() => SetNamespacesAndGroupsAndTypes(
            "FakeProject",
            typeof( object )
        );
    }

    // Modules/Infrastructure
    public partial class FakeModule_Infrastructure : ModuleNode {
        public FakeModule_Infrastructure() => SetNamespacesAndGroupsAndTypes(
            "Global",
            typeof( object ),
            "System",
            /// Group 0
            typeof( object ),
            /// Group 1
            typeof( string )
        );
    }

}
