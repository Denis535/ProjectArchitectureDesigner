// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public abstract class Namespace : Node {

        public override string Name => GetName( this );
        public Group[] Groups => GetChildren<Group>( this ).ToArray();
        public TypeNode[] Types => GetChildren<TypeNode>( this ).ToArray();


        //public Namespace() {
        //    var groups = Array.Empty<Group>();
        //    (Name, Groups) = ("Global", groups);
        //}
        //public Namespace(string name) {
        //    var groups = Array.Empty<Group>();
        //    (Name, Groups) = (name, groups);
        //}
        //public Namespace(string name, params Group[] groups) {
        //    (Name, Groups) = (name, groups);
        //}
        //public Namespace(string name, params TypeNode[] types) {
        //    var groups = new[] { new Group( "Default", types ) };
        //    (Name, Groups) = (name, groups);
        //}


        // Utils
        public override string ToString() {
            return "Namespace: " + Name;
        }


    }
}
