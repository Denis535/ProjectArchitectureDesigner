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
            var comment = SyntaxFactoryUtils.Comment( "// Project: {0}", project.Name );
            var name = CreatePropertyDeclaration_Name( project.Name );
            var properties = project.Modules.Select( CreatePropertyDeclaration_Module ).ToArray();
            return
                SyntaxFactoryUtils.ClassDeclaration( @class )
                .WithLeadingTrivia( SyntaxFactoryUtils.EndOfLine(), comment )
                .AddMembers( name )
                .AddMembers( properties );
        }
        // Module
        public static ClassDeclarationSyntax CreateClassDeclaration_Module(ClassDeclarationSyntax @class, ModuleInfo module) {
            var comment = SyntaxFactoryUtils.Comment( "// Module: {0}", module.Name );
            var name = CreatePropertyDeclaration_Name( module.Name );
            var properties = module.Namespaces.Select( CreatePropertyDeclaration_Namespace ).ToArray();
            var classes = module.Namespaces.Select( CreateClassDeclaration_Namespace ).ToArray();
            return
                SyntaxFactoryUtils.ClassDeclaration( @class )
                .WithLeadingTrivia( SyntaxFactoryUtils.EndOfLine(), comment )
                .AddMembers( name )
                .AddMembers( properties )
                .AddMembers( classes );
        }
        private static ClassDeclarationSyntax CreateClassDeclaration_Namespace(NamespaceEntry @namespace) {
            var comment = SyntaxFactoryUtils.Comment( "// Namespace: {0}", @namespace.Name );
            var name = CreatePropertyDeclaration_Name( @namespace.Name );
            var properties = @namespace.Groups.Select( CreatePropertyDeclaration_Group ).ToArray();
            var classes = @namespace.Groups.Select( CreateClassDeclaration_Group ).ToArray();
            return
                SyntaxFactoryUtils.ClassDeclaration( @namespace.Type, "NamespaceArchNode" )
                .WithLeadingTrivia( comment )
                .AddMembers( name )
                .AddMembers( properties )
                .AddMembers( classes );
        }
        private static ClassDeclarationSyntax CreateClassDeclaration_Group(GroupEntry group) {
            var comment = SyntaxFactoryUtils.Comment( "// Group: {0}", group.Name );
            var name = CreatePropertyDeclaration_Name( group.Name );
            var properties = group.Types.Select( CreatePropertyDeclaration_Type ).ToArray();
            return
                SyntaxFactoryUtils.ClassDeclaration( group.Type, "GroupArchNode" )
                .WithLeadingTrivia( comment )
                .AddMembers( name )
                .AddMembers( properties );
        }


        // Helpers/CreateSyntax
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Name(string name) {
            return SyntaxFactoryUtils.PropertyDeclaration_Overriding( "string", "Name", SyntaxFactoryUtils.StringLiteral( name ) );
        }
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Module(ModuleEntry module) {
            var type = module.Type;
            var identifier = module.Identifier;
            var comment = SyntaxFactoryUtils.Comment( "// Module: {0}", module.Name );
            return SyntaxFactoryUtils.PropertyDeclaration( type, identifier, SyntaxFactoryUtils.ObjectCreationExpression( type ) ).WithTrailingTrivia( comment );
        }
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Namespace(NamespaceEntry @namespace) {
            var type = @namespace.Type;
            var identifier = @namespace.Identifier;
            var comment = SyntaxFactoryUtils.Comment( "// Namespace: {0}", @namespace.Name );
            return SyntaxFactoryUtils.PropertyDeclaration( type, identifier, SyntaxFactoryUtils.ObjectCreationExpression( type ) ).WithTrailingTrivia( comment );
        }
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Group(GroupEntry group) {
            var type = group.Type;
            var identifier = group.Identifier;
            var comment = SyntaxFactoryUtils.Comment( "// Group: {0}", group.Name );
            return SyntaxFactoryUtils.PropertyDeclaration( type, identifier, SyntaxFactoryUtils.ObjectCreationExpression( type ) ).WithTrailingTrivia( comment );
        }
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Type(TypeEntry type) {
            var type_ = type.Type;
            var identifier = type.Identifier;
            var comment = SyntaxFactoryUtils.Comment( "// Type: {0}", type.Name );
            return SyntaxFactoryUtils.PropertyDeclaration( "TypeArchNode", identifier, SyntaxFactoryUtils.TypeOfExpression( type_ ) ).WithTrailingTrivia( comment );
        }


    }
}