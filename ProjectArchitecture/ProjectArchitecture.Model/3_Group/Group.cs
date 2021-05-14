// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public abstract class Group : Node {

        public override string Name => GetName( this );
        public TypeNode[] Types => GetChildren<TypeNode>( this ).ToArray();


        //public Group(string name) {
        //    var types = Array.Empty<TypeNode>();
        //    (Name, Types) = (name, types);
        //}
        //public Group(string name, params TypeNode[] types) {
        //    (Name, Types) = (name, types);
        //}


        // Utils
        public override string ToString() {
            return "Group: " + Name;
        }


    }
}
