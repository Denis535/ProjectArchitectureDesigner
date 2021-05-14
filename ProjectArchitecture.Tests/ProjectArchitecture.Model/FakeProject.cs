// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;

    [Module( typeof( FakeModule_Domain ) )]
    [Module( typeof( FakeModule_Infrastructure ) )]
    public partial class FakeProject : ProjectNode {
    }

    [Namespace( "FakeProject" )]
    [Type( typeof( object ) )]
    public partial class FakeModule_Domain : ModuleNode {
    }

    [Namespace( "Global" )]
    [Type( typeof( object ) )]
    [Namespace( "System" )]
    [Type( typeof( object ) )]
    // String
    [Type( typeof( string ) )]
    public partial class FakeModule_Infrastructure : ModuleNode {
    }

}
