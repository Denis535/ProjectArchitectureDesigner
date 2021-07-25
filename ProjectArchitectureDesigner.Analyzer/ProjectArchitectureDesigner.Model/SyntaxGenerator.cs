// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal static class SyntaxGenerator {


        // Project
        public static ClassDeclarationSyntax CreateClassDeclaration_Project(ClassDeclarationSyntax @class, ProjectInfo project) {
            var comment = SyntaxFactory2.Comment( "// Project: {0}", project.Name );
            var name = PropertyDeclaration_Name( project.Name );
            var modules = PropertyDeclaration_Modules( project.Modules );
            var modules_properties = project.Modules.Select( PropertyDeclaration_Module ).ToArray();
            return
                SyntaxFactory2.ClassDeclaration( @class )
                .WithLeadingTrivia( SyntaxFactory2.EndOfLine(), comment )
                .AddMembers( name )
                .AddMembers( modules )
                .AddMembers( modules_properties );
        }
        // Module
        public static ClassDeclarationSyntax CreateClassDeclaration_Module(ClassDeclarationSyntax @class, ModuleInfo module) {
            var comment = SyntaxFactory2.Comment( "// Module: {0}", module.Name );
            var name = PropertyDeclaration_Name( module.Name );
            var namespaces = PropertyDeclaration_Namespaces( module.Namespaces );
            var namespaces_properties = module.Namespaces.Select( PropertyDeclaration_Namespace ).ToArray();
            var namespaces_classes = module.Namespaces.Select( CreateClassDeclaration_Namespace ).ToArray();
            return
                SyntaxFactory2.ClassDeclaration( @class )
                .WithLeadingTrivia( SyntaxFactory2.EndOfLine(), comment )
                .AddMembers( name )
                .AddMembers( namespaces )
                .AddMembers( namespaces_properties )
                .AddMembers( namespaces_classes );
        }
        private static ClassDeclarationSyntax CreateClassDeclaration_Namespace(NamespaceEntry @namespace) {
            var comment = SyntaxFactory2.Comment( "// Namespace: {0}", @namespace.Name );
            var name = PropertyDeclaration_Name( @namespace.Name );
            var groups = PropertyDeclaration_Groups( @namespace.Groups );
            var groups_properties = @namespace.Groups.Select( PropertyDeclaration_Group ).ToArray();
            var groups_classes = @namespace.Groups.Select( CreateClassDeclaration_Group ).ToArray();
            return
                SyntaxFactory2.ClassDeclaration( "public class $name : NamespaceArchNode {}", @namespace.Type )
                .WithLeadingTrivia( comment )
                .AddMembers( name )
                .AddMembers( groups )
                .AddMembers( groups_properties )
                .AddMembers( groups_classes );
        }
        private static ClassDeclarationSyntax CreateClassDeclaration_Group(GroupEntry group) {
            var name = PropertyDeclaration_Name( group.Name );
            var types = PropertyDeclaration_Types( group.Types );
            return
                SyntaxFactory2.ClassDeclaration( "public class $name : GroupArchNode {}", group.Type )
                .AddMembers( name )
                .AddMembers( types );
        }


        // Helpers/PropertyDeclaration
        private static PropertyDeclarationSyntax PropertyDeclaration_Name(string name) {
            return SyntaxFactory2.PropertyDeclaration( "public override string Name => \"$name\";", name );
        }
        // Helpers/PropertyDeclaration
        private static PropertyDeclarationSyntax PropertyDeclaration_Modules(ModuleEntry[] modules) {
            var items = modules.Select( i => i.Property );
            return SyntaxFactory2.PropertyDeclaration( "public override ModuleArchNode[] Modules => new ModuleArchNode[] { $items };", items );
        }
        private static PropertyDeclarationSyntax PropertyDeclaration_Namespaces(NamespaceEntry[] namespaces) {
            var items = namespaces.Select( i => i.Property );
            return SyntaxFactory2.PropertyDeclaration( "public override NamespaceArchNode[] Namespaces => new NamespaceArchNode[] { $items };", items );
        }
        private static PropertyDeclarationSyntax PropertyDeclaration_Groups(GroupEntry[] groups) {
            var items = groups.Select( i => i.Property );
            return SyntaxFactory2.PropertyDeclaration( "public override GroupArchNode[] Groups => new GroupArchNode[] { $items };", items );
        }
        private static PropertyDeclarationSyntax PropertyDeclaration_Types(TypeEntry[] types) {
            var items = types.Select( i => $"typeof( {i.Type} )" );
            return SyntaxFactory2.PropertyDeclaration( "public override TypeArchNode[] Types { get; } = new TypeArchNode[] { $items };", items );
        }
        // Helpers/PropertyDeclaration
        private static PropertyDeclarationSyntax PropertyDeclaration_Module(ModuleEntry module) {
            var type = module.Type;
            var property = module.Property;
            return SyntaxFactory2.PropertyDeclaration( "public $type $name { get; } = new $type();", type, property, type );
        }
        private static PropertyDeclarationSyntax PropertyDeclaration_Namespace(NamespaceEntry @namespace) {
            var type = @namespace.Type;
            var property = @namespace.Property;
            return SyntaxFactory2.PropertyDeclaration( "public $type $name { get; } = new $type();", type, property, type );
        }
        private static PropertyDeclarationSyntax PropertyDeclaration_Group(GroupEntry group) {
            var type = group.Type;
            var property = group.Property;
            return SyntaxFactory2.PropertyDeclaration( "public $type $name { get; } = new $type();", type, property, type );
        }


    }
}