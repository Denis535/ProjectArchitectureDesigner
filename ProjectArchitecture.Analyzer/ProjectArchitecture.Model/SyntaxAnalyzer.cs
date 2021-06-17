// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal static class SyntaxAnalyzer {


        // Project
        public static bool IsProject(ClassDeclarationSyntax @class) {
            return @class.IsChildOf( "ProjectArchNode" );
        }
        public static ProjectInfo GetProjectInfo(ClassDeclarationSyntax @class, SemanticModel model) {
            var type = @class.Identifier.ValueText;
            var modules = @class
                .GetMethods( "DefineChildren" )
                .FirstOrDefault()?
                .GetBody()?
                .DescendantNodes()
                .Select( GetModuleEntry )
                .OfType<ModuleEntry>()
                .ToArray();
            return new ProjectInfo( type, modules ?? Array.Empty<ModuleEntry>() );
        }


        // Module
        public static bool IsModule(ClassDeclarationSyntax @class) {
            return @class.IsChildOf( "ModuleArchNode" );
        }
        public static ModuleInfo GetModuleInfo(ClassDeclarationSyntax @class, SemanticModel model) {
            var type = @class.Identifier.ValueText;
            var @namespaces = @class
                .GetMethods( "DefineChildren" )
                .FirstOrDefault()?
                .GetBody()?
                .DescendantNodes()
                .SelectMany( GetNamespaceOrGroupOrTypeEntry )
                .GetNamespaceHierarchy()
                .ToArray();
            return new ModuleInfo( type, namespaces ?? Array.Empty<NamespaceEntry>() );
        }


        // Helpers/GetEntry
        private static ModuleEntry? GetModuleEntry(SyntaxNode syntax) {
            if (syntax.IsModuleEntry()) {
                return new ModuleEntry( syntax.GetModuleEntry() );
            }
            return null;
        }
        private static IEnumerable<object> GetNamespaceOrGroupOrTypeEntry(SyntaxNode syntax) {
            if (syntax.IsNamespaceEntry()) {
                yield return new NamespaceEntry( syntax.GetNamespaceEntry(), null! );
            }
            if (syntax.IsTypeEntry()) {
                if (syntax.HasGroupEntry()) yield return new GroupEntry( syntax.GetGroupEntry(), null! );
                yield return new TypeEntry( syntax.GetTypeEntry() );
            }
        }
        // Helpers/GetNamespaceHierarchy
        private static IEnumerable<NamespaceEntry> GetNamespaceHierarchy(this IEnumerable<object> namespaces) {
            return namespaces.Unflatten( i => i is NamespaceEntry ).Select( i => GetNamespace( (NamespaceEntry?) i.Key, i.Children.ToArray() ) );
        }
        private static NamespaceEntry GetNamespace(NamespaceEntry? @namespace, object[] groups) {
            var namespace_ = @namespace ?? new NamespaceEntry( "Global", null! );
            var groups_ = groups.GetGroupHierarchy().ToArray();
            return new NamespaceEntry( namespace_.Name, groups_ );
        }
        private static IEnumerable<GroupEntry> GetGroupHierarchy(this object[] groups) {
            return groups.Unflatten( i => i is GroupEntry ).Select( i => GetGroup( (GroupEntry?) i.Key, i.Children.ToArray() ) );
        }
        private static GroupEntry GetGroup(GroupEntry? group, object[] types) {
            var group_ = group ?? new GroupEntry( "Default", null! );
            var types_ = types.Cast<TypeEntry>().ToArray();
            return new GroupEntry( group_.Name, types_ );
        }


    }
}
