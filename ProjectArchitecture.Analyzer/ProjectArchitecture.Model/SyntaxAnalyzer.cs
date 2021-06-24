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
        public static ProjectInfo GetProjectInfo(ClassDeclarationSyntax @class) {
            var type = @class.Identifier.ValueText;
            var modules = @class
                .GetMethods( "DefineChildren" )
                .FirstOrDefault()?
                .GetBody()?
                .DescendantNodes()
                .GetModuleEntries()
                .ToArray();
            return new ProjectInfo( type, modules ?? Array.Empty<ModuleEntry>() );
        }


        // Module
        public static bool IsModule(ClassDeclarationSyntax @class) {
            return @class.IsChildOf( "ModuleArchNode" );
        }
        public static ModuleInfo GetModuleInfo(ClassDeclarationSyntax @class) {
            var type = @class.Identifier.ValueText;
            var @namespaces = @class
                .GetMethods( "DefineChildren" )
                .FirstOrDefault()?
                .GetBody()?
                .DescendantNodes()
                .GetNamespaceEntries()
                .ToArray();
            return new ModuleInfo( type, namespaces ?? Array.Empty<NamespaceEntry>() );
        }


        // Helpers/GetEntries
        private static IEnumerable<ModuleEntry> GetModuleEntries(this IEnumerable<SyntaxNode> nodes) {
            foreach (var module in nodes.Where( SyntaxUtils.IsModuleEntry )) {
                yield return new ModuleEntry( module.GetModuleEntry() );
            }
        }
        private static IEnumerable<NamespaceEntry> GetNamespaceEntries(this IEnumerable<SyntaxNode> nodes) {
            foreach (var (@namespace, types) in nodes.Where( SyntaxUtils.IsNamespaceOrTypeEntry ).Unflatten( SyntaxUtils.IsNamespaceEntry )) {
                var name = @namespace?.GetNamespaceEntry() ?? "Global";
                var groups = types.GetGroupEntries().ToArray();
                yield return new NamespaceEntry( name, groups );
            }
        }
        private static IEnumerable<GroupEntry> GetGroupEntries(this IEnumerable<SyntaxNode> nodes) {
            foreach (var group in nodes.SplitByFirst( SyntaxUtils.HasGroupEntry )) {
                var name = group.First().HasGroupEntry() ? group.First().GetGroupEntry() : "Default";
                var types = group.GetTypeEntries().ToArray();
                yield return new GroupEntry( name, types );
            }
        }
        private static IEnumerable<TypeEntry> GetTypeEntries(this IEnumerable<SyntaxNode> nodes) {
            foreach (var type in nodes) {
                yield return new TypeEntry( type.GetTypeEntry() );
            }
        }


    }
}
