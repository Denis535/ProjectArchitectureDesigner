// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    // ProjectInfo
    internal record ProjectInfo(string Type, ProjectInfo.Module_[] Modules) {
        public string Name => Type.WithoutPrefix( "Project_" ).Replace( '_', '.' );
        public string Type { get; } = Type;
        public Module_[] Modules { get; } = Modules;

        public record Module_(string Type) {
            public string Type { get; } = Type;
            public string Identifier => Type.WithoutPrefix( "Module_" ).EscapeIdentifier();
        }
    }
    // ModuleInfo
    internal record ModuleInfo(string Type, ModuleInfo.Namespace_[] Namespaces) {
        public string Name => Type.WithoutPrefix( "Module_" ).Replace( '_', '.' );
        public string Type { get; } = Type;
        public Namespace_[] Namespaces { get; } = Namespaces;

        public record Namespace_(string Name, Group_[] Groups) {
            public string Name { get; } = Name;
            public string Type => "Namespace_" + Name.EscapeTypeName();
            public string Identifier => Name.EscapeIdentifier();
            public Group_[] Groups { get; } = Groups;
        }
        public record Group_(string Name, Type_[] Types) {
            public string Name { get; } = Name;
            public string Type => "Group_" + Name.EscapeTypeName();
            public string Identifier => Name.EscapeIdentifier();
            public Type_[] Types { get; } = Types;
        }
        public record Type_(string Type) {
            public string Type { get; } = Type;
            public string Identifier => Type.EscapeIdentifier();
        }
    }
}
