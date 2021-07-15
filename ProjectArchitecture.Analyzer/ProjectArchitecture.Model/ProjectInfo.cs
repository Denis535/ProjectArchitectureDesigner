// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    // Info/ProjectInfo
    internal record ProjectInfo(string Type, ModuleEntry[] Modules) {
        public string Name => Type.ToName( "Project_" );
        public string Type { get; } = Type;
        public ModuleEntry[] Modules { get; } = Modules;
    }
    // Info/ModuleInfo
    internal record ModuleInfo(string Type, NamespaceEntry[] Namespaces) {
        public string Name => Type.ToName( "Module_" );
        public string Type { get; } = Type;
        public NamespaceEntry[] Namespaces { get; } = Namespaces;
    }

    // Entries/ModuleEntry
    internal record ModuleEntry(string Type) {
        public string Name { get; } = Type.ToName( "Module_" );
        public string Type { get; } = Type;
        public string Identifier => Name.ToIdentifier();
    }
    // Entries/NamespaceEntry
    internal record NamespaceEntry(string Name, GroupEntry[] Groups) {
        public string Name { get; } = Name;
        public string Type => Name.ToType( "Namespace_" );
        public string Identifier => Name.ToIdentifier();
        public GroupEntry[] Groups { get; } = Groups;
    }
    // Entries/GroupEntry
    internal record GroupEntry(string Name, TypeEntry[] Types) {
        public string Name { get; } = Name;
        public string Type => Name.ToType( "Group_" );
        public string Identifier => Name.ToIdentifier();
        public TypeEntry[] Types { get; } = Types;
    }
    // Entries/TypeEntry
    internal record TypeEntry(string Type) {
        public string Name { get; } = Type;
        public string Type { get; } = Type;
        public string Identifier => Name.ToIdentifier();
    }
}
