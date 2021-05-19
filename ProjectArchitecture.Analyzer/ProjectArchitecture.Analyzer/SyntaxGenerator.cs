// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Analyzer {
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
            var properties = modules.Select( i => CreatePropertyDeclaration_Module( i ) ).ToArray();
            var comment = SyntaxFactoryUtils.Comment( "// {0}: {1}", @class.Identifier, modules.Select( i => i.Value ).Join() );
            return SyntaxFactoryUtils.ClassDeclaration( @class ).AddMembers( properties ).WithLeadingTrivia( comment );
        }
        // Classes/Module
        public static ClassDeclarationSyntax CreateClassDeclaration_Module(ClassDeclarationSyntax @class, SyntaxAnalyzer.Namespace[] namespaces) {
            var classes = namespaces.Select( CreateClassDeclaration_Namespace ).ToArray();
            var properties = namespaces.Select( i => CreatePropertyDeclaration_Namespace( i ) ).ToArray();
            var comment = SyntaxFactoryUtils.Comment( "// {0}: {1}", @class.Identifier, namespaces.Select( i => i.Value ).Join() );
            return SyntaxFactoryUtils.ClassDeclaration( @class ).AddMembers( classes ).AddMembers( properties ).WithLeadingTrivia( comment );
        }
        private static ClassDeclarationSyntax CreateClassDeclaration_Namespace(SyntaxAnalyzer.Namespace @namespace) {
            var type = @namespace.GetTypeName();
            var classes = @namespace.Groups.Select( i => CreateClassDeclaration_Group( i ) ).ToArray();
            var properties = @namespace.Groups.Select( i => CreatePropertyDeclaration_Group( i ) ).ToArray();
            var comment = SyntaxFactoryUtils.Comment( "// {0}: {1}", @namespace.Value, @namespace.Groups.Select( i => i.Value ).Join() );
            return SyntaxFactoryUtils.ClassDeclaration( type, "NamespaceNode" ).AddMembers( classes ).AddMembers( properties ).WithLeadingTrivia( comment );
        }
        private static ClassDeclarationSyntax CreateClassDeclaration_Group(SyntaxAnalyzer.Group group) {
            var type = group.GetTypeName();
            var properties = group.Types.Select( i => CreatePropertyDeclaration_Type( i ) ).ToArray();
            var comment = SyntaxFactoryUtils.Comment( "// {0}: {1}", group.Value, group.Types.Select( i => i.Value ).Join() );
            return SyntaxFactoryUtils.ClassDeclaration( type, "GroupNode" ).AddMembers( properties ).WithLeadingTrivia( comment );
        }


        // Properties/Project
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Module(SyntaxAnalyzer.Module module) {
            var type = module;
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
        private static string GetTypeName(this SyntaxAnalyzer.Namespace @namespace) => "Namespace_" + GetTypeName( @namespace.Value );
        private static string GetTypeName(this SyntaxAnalyzer.Group group) => "Group_" + GetTypeName( group.Value );
        private static string GetTypeName(string value) {
            return value.Replace( '.', '_' ).Replace( '-', '_' ).Replace( ' ', '_' );
        }
        // Helpers/Identifier
        private static string GetIdentifier(this SyntaxAnalyzer.Module module) {
            var value = module.Value;
            var i = value.IndexOf( "Module_" );
            if (i != -1) value = value.Substring( i + 7 );
            return value;
        }
        private static string GetIdentifier(this SyntaxAnalyzer.Namespace @namespace) => GetIdentifier( @namespace.Value );
        private static string GetIdentifier(this SyntaxAnalyzer.Group group) => GetIdentifier( group.Value );
        private static string GetIdentifier(this SyntaxAnalyzer.Type type) => GetIdentifier( type.Value );
        private static string GetIdentifier(string value) {
            value = value.Replace( '.', '_' ).Replace( '-', '_' ).Replace( ' ', '_' );
            if (SyntaxFacts.GetKeywordKind( value ) != SyntaxKind.None) value = "@" + value;
            return value;
        }


    }
}
