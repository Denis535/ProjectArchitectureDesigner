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
        public static ClassDeclarationSyntax CreateClassDeclaration_Project(ClassDeclarationSyntax @class, SyntaxAnalyzer.Module[] modules) {
            var comment = SyntaxFactoryUtils.Comment( GetComment( @class.Identifier.ValueText, modules ) );
            //var name = SyntaxFactoryUtils.PropertyDeclaration_Overriding( "string", "Name", SyntaxFactoryUtils.StringLiteral( @class.Identifier.ValueText ) );
            var properties = modules.Select( CreatePropertyDeclaration_Module ).ToArray();
            return
                SyntaxFactoryUtils.ClassDeclaration( @class )
                .WithLeadingTrivia( comment )
                //.AddMembers( name )
                .AddMembers( properties );
        }
        // Classes/Module
        public static ClassDeclarationSyntax CreateClassDeclaration_Module(ClassDeclarationSyntax @class, SyntaxAnalyzer.Namespace[] namespaces) {
            var comment = SyntaxFactoryUtils.Comment( GetComment( @class.Identifier.ValueText, namespaces ) );
            //var name = SyntaxFactoryUtils.PropertyDeclaration_Overriding( "string", "Name", SyntaxFactoryUtils.StringLiteral( @class.Identifier.ValueText ) );
            var properties = namespaces.Select( CreatePropertyDeclaration_Namespace ).ToArray();
            var classes = namespaces.Select( CreateClassDeclaration_Namespace ).ToArray();
            return
                SyntaxFactoryUtils.ClassDeclaration( @class )
                .WithLeadingTrivia( comment )
                //.AddMembers( name )
                .AddMembers( properties )
                .AddMembers( classes );
        }
        private static ClassDeclarationSyntax CreateClassDeclaration_Namespace(SyntaxAnalyzer.Namespace @namespace) {
            var comment = SyntaxFactoryUtils.Comment( GetComment( @namespace ) );
            var name = SyntaxFactoryUtils.PropertyDeclaration_Overriding( "string", "Name", SyntaxFactoryUtils.StringLiteral( @namespace ) );
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
            var comment = SyntaxFactoryUtils.Comment( GetComment( group ) );
            var name = SyntaxFactoryUtils.PropertyDeclaration_Overriding( "string", "Name", SyntaxFactoryUtils.StringLiteral( group ) );
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
            return SyntaxFactoryUtils.PropertyDeclaration( "TypeNode", identifier, SyntaxFactoryUtils.TypeOfExpression( type ) );
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
        //private static string GetIdentifier(this SyntaxAnalyzer.Module module) {
        //    var value = module.Value;
        //    var i = value.IndexOf( "Module_" );
        //    if (i != -1) value = value.Substring( i + 7 );
        //    return value;
        //}
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
        private static string GetComment(string project, SyntaxAnalyzer.Module[] modules) {
            return string.Format( "// Project: {0}, Modules: {1}", project, modules.Join( i => i.Value ) );
        }
        private static string GetComment(string module, SyntaxAnalyzer.Namespace[] namespaces) {
            return string.Format( "// Module: {0}, Namespaces: {1}", module, namespaces.Join( i => i.Value ) );
        }
        private static string GetComment(SyntaxAnalyzer.Namespace @namespace) {
            return string.Format( "// Namespace: {0}, Groups: {1}", @namespace.Value, @namespace.Groups.Join( i => i.Value ) );
        }
        private static string GetComment(SyntaxAnalyzer.Group group) {
            return string.Format( "// Group: {0}, Types: {1}", group.Value, group.Types.Join( i => i.Value ) );
        }


    }
}
