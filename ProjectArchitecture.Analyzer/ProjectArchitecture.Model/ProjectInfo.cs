// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    // ProjectInfo
    internal record ProjectInfo(string Type, ModuleEntry[] Modules) {
        public string Name => Type.WithoutPrefix( "Project_" ).Replace( '_', '.' );
        public string Type { get; } = Type;
        public ModuleEntry[] Modules { get; } = Modules;
    }
    // ModuleInfo
    internal record ModuleInfo(string Type, NamespaceEntry[] Namespaces) {
        public string Name => Type.WithoutPrefix( "Module_" ).Replace( '_', '.' );
        public string Type { get; } = Type;
        public NamespaceEntry[] Namespaces { get; } = Namespaces;
    }
    // Entries/ModuleEntry
    public record ModuleEntry(string Type) {
        public string Name { get; } = Type;
        public string Type { get; } = Type;
        public string Identifier => Type.WithoutPrefix( "Module_" ).EscapeIdentifier(); // todo
    }
    // Entries/NamespaceEntry
    public record NamespaceEntry(string Name, GroupEntry[] Groups) {
        public string Name { get; } = Name;
        public string Type => "Namespace_" + Name.EscapeTypeName();
        public string Identifier => Name.EscapeIdentifier();
        public GroupEntry[] Groups { get; } = Groups;
    }
    // Entries/GroupEntry
    public record GroupEntry(string Name, TypeEntry[] Types) {
        public string Name { get; } = Name;
        public string Type => "Group_" + Name.EscapeTypeName();
        public string Identifier => Name.EscapeIdentifier();
        public TypeEntry[] Types { get; } = Types;
    }
    // Entries/TypeEntry
    public record TypeEntry(string Type) {
        public string Name { get; } = Type;
        public string Type { get; } = Type;
        public string Identifier => Type.EscapeIdentifier(); // todo
    }
}
