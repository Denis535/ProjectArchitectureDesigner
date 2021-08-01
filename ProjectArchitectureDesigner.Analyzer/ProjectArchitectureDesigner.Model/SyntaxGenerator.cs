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
        public static ClassDeclarationSyntax CreateSyntax_ProjectClass(ClassDeclarationSyntax @class, ProjectInfo project) {
            return SyntaxFactory2.ClassDeclaration( @class )
                .WithLeadingTrivia( SyntaxFactory2.EndOfLine(), SyntaxFactory2.Comment( "// Project" ) )
                .AddMembers( CreateSyntax_NameProperty( project.Name ) )
                .AddMembers( CreateSyntax_ModuleArrayProperty( project.Modules ) )
                .AddMembers( project.Modules.Select( CreateSyntax_ModuleProperty ).ToArray() );
        }
        // Module
        public static ClassDeclarationSyntax CreateSyntax_ModuleClass(ClassDeclarationSyntax @class, ModuleInfo module) {
            return SyntaxFactory2.ClassDeclaration( @class )
                .WithLeadingTrivia( SyntaxFactory2.EndOfLine(), SyntaxFactory2.Comment( "// Module" ) )
                .AddMembers( CreateSyntax_NameProperty( module.Name ) )
                .AddMembers( CreateSyntax_NamespaceArrayProperty( module.Namespaces ) )
                .AddMembers( CreateSyntax_ModuleConstructor( module ) )
                .AddMembers( module.Namespaces.Select( CreateSyntax_NamespaceProperty ).ToArray() )
                .AddMembers( module.Namespaces.Select( CreateSyntax_NamespaceClass ).ToArray() );
        }
        private static ClassDeclarationSyntax CreateSyntax_NamespaceClass(NamespaceEntry @namespace) {
            return SyntaxFactory2.ClassDeclaration( "public class $type : NamespaceArchNode {}", @namespace.Type )
                .WithLeadingTrivia( SyntaxFactory2.Comment( "// Namespace" ) )
                .AddMembers( CreateSyntax_NameProperty( @namespace.Name ) )
                .AddMembers( CreateSyntax_GroupArrayProperty( @namespace.Groups ) )
                .AddMembers( CreateSyntax_NamespaceConstructor( @namespace ) )
                .AddMembers( @namespace.Groups.Select( CreateSyntax_GroupProperty ).ToArray() )
                .AddMembers( @namespace.Groups.Select( CreateSyntax_GroupClass ).ToArray() );
        }
        private static ClassDeclarationSyntax CreateSyntax_GroupClass(GroupEntry group) {
            return SyntaxFactory2.ClassDeclaration( "public class $type : GroupArchNode {}", group.Type )
                .WithLeadingTrivia( SyntaxFactory2.Comment( "// Group" ) )
                .AddMembers( CreateSyntax_NameProperty( group.Name ) )
                .AddMembers( CreateSyntax_TypeArrayProperty( group.Types ) )
                .AddMembers( CreateSyntax_GroupConstructor( group ) );
        }


        // Helpers/CreateSyntax/Property/Name
        private static PropertyDeclarationSyntax CreateSyntax_NameProperty(string name) {
            return SyntaxFactory2.PropertyDeclaration( "public override string Name => \"$name\";", name );
        }
        // Helpers/CreateSyntax/Property/Array
        private static PropertyDeclarationSyntax CreateSyntax_ModuleArrayProperty(ModuleEntry[] modules) {
            var items = modules.Select( i => i.Property );
            return SyntaxFactory2.PropertyDeclaration( "public override ModuleArchNode[] Modules => new ModuleArchNode[] { $items };", items );
        }
        private static PropertyDeclarationSyntax CreateSyntax_NamespaceArrayProperty(NamespaceEntry[] namespaces) {
            var items = namespaces.Select( i => i.Property );
            return SyntaxFactory2.PropertyDeclaration( "public override NamespaceArchNode[] Namespaces => new NamespaceArchNode[] { $items };", items );
        }
        private static PropertyDeclarationSyntax CreateSyntax_GroupArrayProperty(GroupEntry[] groups) {
            var items = groups.Select( i => i.Property );
            return SyntaxFactory2.PropertyDeclaration( "public override GroupArchNode[] Groups => new GroupArchNode[] { $items };", items );
        }
        private static PropertyDeclarationSyntax CreateSyntax_TypeArrayProperty(TypeEntry[] types) {
            var items = types.Select( i => $"new TypeArchNode( typeof( {i.Type} ) )" );
            return SyntaxFactory2.PropertyDeclaration( "public override TypeArchNode[] Types { get; } = new TypeArchNode[] { $items };", items );
        }
        // Helpers/CreateSyntax/Property
        private static PropertyDeclarationSyntax CreateSyntax_ModuleProperty(ModuleEntry module) {
            var type = module.Type;
            var property = module.Property;
            return SyntaxFactory2.PropertyDeclaration( "public $type $name { get; } = new $type();", type, property, type );
        }
        private static PropertyDeclarationSyntax CreateSyntax_NamespaceProperty(NamespaceEntry @namespace) {
            var type = @namespace.Type;
            var property = @namespace.Property;
            return SyntaxFactory2.PropertyDeclaration( "public $type $name { get; } = new $type();", type, property, type );
        }
        private static PropertyDeclarationSyntax CreateSyntax_GroupProperty(GroupEntry group) {
            var type = group.Type;
            var property = group.Property;
            return SyntaxFactory2.PropertyDeclaration( "public $type $name { get; } = new $type();", type, property, type );
        }
        // Helpers/CreateSyntax/Constructor
        private static ConstructorDeclarationSyntax CreateSyntax_ModuleConstructor(ModuleInfo module) {
            var type = module.Type;
            return SyntaxFactory2.ConstructorDeclaration( "public $type() {}", type );
        }
        private static ConstructorDeclarationSyntax CreateSyntax_NamespaceConstructor(NamespaceEntry @namespace) {
            var type = @namespace.Type;
            return SyntaxFactory2.ConstructorDeclaration( "public $type() {}", type );
        }
        private static ConstructorDeclarationSyntax CreateSyntax_GroupConstructor(GroupEntry group) {
            var type = group.Type;
            return SyntaxFactory2.ConstructorDeclaration( "public $type() {}", type );
        }


    }
}