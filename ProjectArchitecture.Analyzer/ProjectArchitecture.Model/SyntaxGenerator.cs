// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal static class SyntaxGenerator {


        // Project
        public static ClassDeclarationSyntax CreateClassDeclaration_Project(ClassDeclarationSyntax @class, SyntaxAnalyzer.ProjectData project) {
            var comment = SyntaxFactoryUtils.Comment( "// Project: {0}", project.Name );
            var name = CreatePropertyDeclaration_Name( project.Name );
            var properties = project.Modules.Select( CreatePropertyDeclaration_Module ).ToArray();
            return
                SyntaxFactoryUtils.ClassDeclaration( @class )
                .WithLeadingTrivia( comment )
                .AddMembers( name )
                .AddMembers( properties );
        }
        // Module
        public static ClassDeclarationSyntax CreateClassDeclaration_Module(ClassDeclarationSyntax @class, SyntaxAnalyzer.ModuleData module) {
            var comment = SyntaxFactoryUtils.Comment( "// Module: {0}", module.Name );
            var name = CreatePropertyDeclaration_Name( module.Name );
            var properties = module.Namespaces.Select( CreatePropertyDeclaration_Namespace ).ToArray();
            var classes = module.Namespaces.Select( CreateClassDeclaration_Namespace ).ToArray();
            return
                SyntaxFactoryUtils.ClassDeclaration( @class )
                .WithLeadingTrivia( comment )
                .AddMembers( name )
                .AddMembers( properties )
                .AddMembers( classes );
        }
        private static ClassDeclarationSyntax CreateClassDeclaration_Namespace(SyntaxAnalyzer.Namespace @namespace) {
            var comment = SyntaxFactoryUtils.Comment( "// Namespace: {0}", @namespace.Name );
            var name = CreatePropertyDeclaration_Name( @namespace.Name );
            var properties = @namespace.Groups.Select( CreatePropertyDeclaration_Group ).ToArray();
            var classes = @namespace.Groups.Select( CreateClassDeclaration_Group ).ToArray();
            return
                SyntaxFactoryUtils.ClassDeclaration( @namespace.GetTypeName(), "NamespaceNode" )
                .WithLeadingTrivia( comment )
                .AddMembers( name )
                .AddMembers( properties )
                .AddMembers( classes );
        }
        private static ClassDeclarationSyntax CreateClassDeclaration_Group(SyntaxAnalyzer.Group group) {
            var comment = SyntaxFactoryUtils.Comment( "// Group: {0}", group.Name );
            var name = CreatePropertyDeclaration_Name( group.Name );
            var properties = group.Types.Select( CreatePropertyDeclaration_Type ).ToArray();
            return
                SyntaxFactoryUtils.ClassDeclaration( group.GetTypeName(), "GroupNode" )
                .WithLeadingTrivia( comment )
                .AddMembers( name )
                .AddMembers( properties );
        }


        // Helpers/Syntax
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Name(string name) {
            return SyntaxFactoryUtils.PropertyDeclaration_Overriding( "string", "Name", SyntaxFactoryUtils.StringLiteral( name ) );
        }
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Module(SyntaxAnalyzer.Module module) {
            var type = module.GetTypeName();
            var identifier = module.GetIdentifier();
            return SyntaxFactoryUtils.PropertyDeclaration( type, identifier, SyntaxFactoryUtils.ObjectCreationExpression( type ) );
        }
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Namespace(SyntaxAnalyzer.Namespace @namespace) {
            var type = @namespace.GetTypeName();
            var identifier = @namespace.GetIdentifier();
            return SyntaxFactoryUtils.PropertyDeclaration( type, identifier, SyntaxFactoryUtils.ObjectCreationExpression( type ) );
        }
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Group(SyntaxAnalyzer.Group group) {
            var type = group.GetTypeName();
            var identifier = group.GetIdentifier();
            return SyntaxFactoryUtils.PropertyDeclaration( type, identifier, SyntaxFactoryUtils.ObjectCreationExpression( type ) );
        }
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Type(SyntaxAnalyzer.Type type) {
            var identifier = type.GetIdentifier();
            return SyntaxFactoryUtils.PropertyDeclaration( "TypeNode", identifier, SyntaxFactoryUtils.TypeOfExpression( type.Name ) );
        }


        // Helpers/String/Type
        private static string GetTypeName(this SyntaxAnalyzer.Module module) => module.Name;
        private static string GetTypeName(this SyntaxAnalyzer.Namespace @namespace) => "Namespace_" + @namespace.Name.Escape();
        private static string GetTypeName(this SyntaxAnalyzer.Group group) => "Group_" + group.Name.Escape();
        // Helpers/String/Identifier
        private static string GetIdentifier(this SyntaxAnalyzer.Module module) => module.Name;
        private static string GetIdentifier(this SyntaxAnalyzer.Namespace @namespace) => @namespace.Name.Escape().Escape2();
        private static string GetIdentifier(this SyntaxAnalyzer.Group group) => group.Name.Escape().Escape2();
        private static string GetIdentifier(this SyntaxAnalyzer.Type type) => type.Name.Escape().Escape2();
        // Helpers/String
        private static string Escape(this string value) {
            return string.Concat( value.Select( Escape ) );
        }
        private static string Escape2(this string value) {
            if (SyntaxFacts.GetKeywordKind( value ) != SyntaxKind.None) return "@" + value;
            return value;
        }
        private static char Escape(char @char) {
            return char.IsLetterOrDigit( @char ) ? @char : '_';
        }


    }
}
