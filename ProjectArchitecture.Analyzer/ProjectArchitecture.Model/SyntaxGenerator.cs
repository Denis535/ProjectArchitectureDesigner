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


        // Classes/Project
        public static ClassDeclarationSyntax CreateClassDeclaration_Project(ClassDeclarationSyntax @class, SyntaxAnalyzer.ProjectData project) {
            var comment = SyntaxFactoryUtils.Comment( "// Project: {0}", project.GetName() );
            var name = SyntaxFactoryUtils.PropertyDeclaration_Overriding( "string", "Name", SyntaxFactoryUtils.StringLiteral( project.GetName() ) );
            var properties = project.Modules.Select( CreatePropertyDeclaration_Module ).ToArray();
            return
                SyntaxFactoryUtils.ClassDeclaration( @class )
                .WithLeadingTrivia( comment )
                .AddMembers( name )
                .AddMembers( properties );
        }
        // Classes/Module
        public static ClassDeclarationSyntax CreateClassDeclaration_Module(ClassDeclarationSyntax @class, SyntaxAnalyzer.ModuleData module) {
            var comment = SyntaxFactoryUtils.Comment( "// Module: {0}", module.GetName() );
            var name = SyntaxFactoryUtils.PropertyDeclaration_Overriding( "string", "Name", SyntaxFactoryUtils.StringLiteral( module.GetName() ) );
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
            var comment = SyntaxFactoryUtils.Comment( "// Namespace: {0}", @namespace.Value );
            var name = SyntaxFactoryUtils.PropertyDeclaration_Overriding( "string", "Name", SyntaxFactoryUtils.StringLiteral( @namespace.Value ) );
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
            var comment = SyntaxFactoryUtils.Comment( "// Group: {0}", group.Value );
            var name = SyntaxFactoryUtils.PropertyDeclaration_Overriding( "string", "Name", SyntaxFactoryUtils.StringLiteral( group.Value ) );
            var properties = group.Types.Select( CreatePropertyDeclaration_Type ).ToArray();
            return
                SyntaxFactoryUtils.ClassDeclaration( group.GetTypeName(), "GroupNode" )
                .WithLeadingTrivia( comment )
                .AddMembers( name )
                .AddMembers( properties );
        }


        // Properties/Project
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Module(SyntaxAnalyzer.Module module) {
            var type = module.GetTypeName();
            var identifier = module.GetIdentifier();
            return SyntaxFactoryUtils.PropertyDeclaration( type, identifier, SyntaxFactoryUtils.ObjectCreationExpression( type ) );
        }
        // Properties/Module
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
            return SyntaxFactoryUtils.PropertyDeclaration( "TypeNode", identifier, SyntaxFactoryUtils.TypeOfExpression( type.Value ) );
        }


        // Helpers/Name
        private static string GetName(this SyntaxAnalyzer.ProjectData project) {
            return WithoutPrefix( project.Value, "Project_" ).Replace( '_', '.' );
        }
        private static string GetName(this SyntaxAnalyzer.ModuleData module) {
            return WithoutPrefix( module.Value, "Module_" ).Replace( '_', '.' );
        }
        // Helpers/Type
        private static string GetTypeName(this SyntaxAnalyzer.Module module) => module.Value;
        private static string GetTypeName(this SyntaxAnalyzer.Namespace @namespace) => "Namespace_" + GetTypeName( @namespace.Value );
        private static string GetTypeName(this SyntaxAnalyzer.Group group) => "Group_" + GetTypeName( group.Value );
        private static string GetTypeName(string value) {
            return value.Select( Escape ).Map( string.Concat );
        }
        // Helpers/Identifier
        private static string GetIdentifier(this SyntaxAnalyzer.Module module) => module.Value;
        private static string GetIdentifier(this SyntaxAnalyzer.Namespace @namespace) => GetIdentifier( @namespace.Value );
        private static string GetIdentifier(this SyntaxAnalyzer.Group group) => GetIdentifier( group.Value );
        private static string GetIdentifier(this SyntaxAnalyzer.Type type) => GetIdentifier( type.Value );
        private static string GetIdentifier(string value) {
            value = value.Select( Escape ).Map( string.Concat );
            if (SyntaxFacts.GetKeywordKind( value ) != SyntaxKind.None) value = "@" + value;
            return value;
        }
        private static char Escape(char @char) {
            return char.IsLetterOrDigit( @char ) ? @char : '_';
        }
        // Helpers/String
        private static string WithoutPrefix(string value, string prefix) {
            var i = value.IndexOf( prefix );
            if (i != -1) value = value.Substring( i + prefix.Length );
            return value;
        }


    }
}
