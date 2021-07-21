// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    // ClassInfo
    internal record ProjectInfo(string Type, ModuleEntry[] Modules) {
        public string Name => Type.WithoutPrefix( "Project_" ).ToBeautifulName();
        public string Type { get; } = Type;
        public ModuleEntry[] Modules { get; } = Modules;
    }
    internal record ModuleInfo(string Type, NamespaceEntry[] Namespaces) {
        public string Name => Type.WithoutPrefix( "Module_" ).ToBeautifulName();
        public string Type { get; } = Type;
        public NamespaceEntry[] Namespaces { get; } = Namespaces;
    }
    // ClassInfo/Entries
    internal record ModuleEntry(string Type) {
        public string Name { get; } = Type.WithoutPrefix( "Module_" ).ToBeautifulName();
        public string Type { get; } = Type;
        public string Property => Name.EscapeMemberIdentifier();
    }
    internal record NamespaceEntry(string Name, GroupEntry[] Groups) {
        public string Name { get; } = Name;
        public string Type => "Namespace_" + Name.EscapeTypeIdentifier();
        public string Property => Name.EscapeMemberIdentifier();
        public GroupEntry[] Groups { get; } = Groups;
    }
    internal record GroupEntry(string Name, TypeEntry[] Types) {
        public string Name { get; } = Name;
        public string Type => "Group_" + Name.EscapeTypeIdentifier();
        public string Property => Name.EscapeMemberIdentifier();
        public TypeEntry[] Types { get; } = Types;
    }
    internal record TypeEntry(string Type) {
        public string Type { get; } = Type;
    }
}
