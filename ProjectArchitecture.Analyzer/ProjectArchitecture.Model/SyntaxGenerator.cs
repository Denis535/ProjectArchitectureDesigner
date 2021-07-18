// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
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
            var name = CreatePropertyDeclaration_Name( project.Name );
            var modules = CreatePropertyDeclaration_Modules( project.Modules );
            var modules_properties = project.Modules.Select( CreatePropertyDeclaration_Module ).ToArray();
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
            var name = CreatePropertyDeclaration_Name( module.Name );
            var namespaces = CreatePropertyDeclaration_Namespaces( module.Namespaces );
            var namespaces_properties = module.Namespaces.Select( CreatePropertyDeclaration_Namespace ).ToArray();
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
            var name = CreatePropertyDeclaration_Name( @namespace.Name );
            var groups = CreatePropertyDeclaration_Groups( @namespace.Groups );
            var groups_properties = @namespace.Groups.Select( CreatePropertyDeclaration_Group ).ToArray();
            var groups_classes = @namespace.Groups.Select( CreateClassDeclaration_Group ).ToArray();
            return
                SyntaxFactory2.ClassDeclaration( @namespace.Type, "NamespaceArchNode" )
                .WithLeadingTrivia( comment )
                .AddMembers( name )
                .AddMembers( groups )
                .AddMembers( groups_properties )
                .AddMembers( groups_classes );
        }
        private static ClassDeclarationSyntax CreateClassDeclaration_Group(GroupEntry group) {
            var name = CreatePropertyDeclaration_Name( group.Name );
            var types = CreatePropertyDeclaration_Types( group.Types );
            return
                SyntaxFactory2.ClassDeclaration( group.Type, "GroupArchNode" )
                .AddMembers( name )
                .AddMembers( types );
        }


        // Helpers/CreatePropertyDeclaration/Overriding
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Name(string name) {
            return SyntaxFactory2.PropertyDeclaration_Deferred_Overriding( "string", "Name", $"\"{name}\"" );
        }
        // Helpers/CreatePropertyDeclaration/Overriding
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Modules(ModuleEntry[] modules) {
            var expression = SyntaxFactory2.NewArrayExpression( "ModuleArchNode", modules.Select( i => i.Identifier ) );
            return SyntaxFactory2.PropertyDeclaration_Deferred_Overriding( "ModuleArchNode[]", "Modules", expression );
        }
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Namespaces(NamespaceEntry[] namespaces) {
            var expression = SyntaxFactory2.NewArrayExpression( "NamespaceArchNode", namespaces.Select( i => i.Identifier ) );
            return SyntaxFactory2.PropertyDeclaration_Deferred_Overriding( "NamespaceArchNode[]", "Namespaces", expression );
        }
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Groups(GroupEntry[] groups) {
            var expression = SyntaxFactory2.NewArrayExpression( "GroupArchNode", groups.Select( i => i.Identifier ) );
            return SyntaxFactory2.PropertyDeclaration_Deferred_Overriding( "GroupArchNode[]", "Groups", expression );
        }
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Types(TypeEntry[] types) {
            var expression = SyntaxFactory2.NewArrayExpression( "TypeArchNode", types.Select( i => SyntaxFactory2.TypeOfExpression( i.Type ) ) );
            return SyntaxFactory2.PropertyDeclaration_Immediate_Overriding( "TypeArchNode[]", "Types", expression );
        }
        // Helpers/CreatePropertyDeclaration
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Module(ModuleEntry module) {
            var type = module.Type;
            var identifier = module.Identifier;
            return SyntaxFactory2.PropertyDeclaration_Immediate( type, identifier, $"new {type}()" );
        }
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Namespace(NamespaceEntry @namespace) {
            var type = @namespace.Type;
            var identifier = @namespace.Identifier;
            return SyntaxFactory2.PropertyDeclaration_Immediate( type, identifier, $"new {type}()" );
        }
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Group(GroupEntry group) {
            var type = group.Type;
            var identifier = group.Identifier;
            return SyntaxFactory2.PropertyDeclaration_Immediate( type, identifier, $"new {type}()" );
        }


    }
}