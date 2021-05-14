// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Analyzer {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal static class SyntaxGenerator {


        // Project
        public static ClassDeclarationSyntax CreateClassDeclaration_Project(ClassDeclarationSyntax @class, string[] modules) {
            var properties = modules.Select( CreatePropertyDeclaration_Module ).ToArray();
            return SyntaxFactoryUtils.ClassDeclaration( @class ).AddMembers( properties );
        }

        // Module
        public static ClassDeclarationSyntax CreateClassDeclaration_Module(ClassDeclarationSyntax @class, (string Namespace, (string Group, string[] Types)[] Groups)[] namespaces) {
            var classes = namespaces.Select( i => CreateClassDeclaration_Namespace( i.Namespace, i.Groups ) ).ToArray();
            var properties = namespaces.Select( i => CreatePropertyDeclaration_Namespace( i.Namespace ) ).ToArray();
            return SyntaxFactoryUtils.ClassDeclaration( @class ).AddMembers( classes ).AddMembers( properties );
        }
        private static ClassDeclarationSyntax CreateClassDeclaration_Namespace(string @namespace, (string Group, string[] Types)[] groups) {
            var type = "Namespace_" + @namespace.EscapeTypeName();
            var classes = groups.Select( i => CreateClassDeclaration_Group( i.Group, i.Types ) ).ToArray();
            var properties = groups.Select( i => CreatePropertyDeclaration_Group( i.Group ) ).ToArray();
            return SyntaxFactoryUtils.ClassDeclaration( type, "NamespaceNode" ).AddMembers( classes ).AddMembers( properties );
        }
        private static ClassDeclarationSyntax CreateClassDeclaration_Group(string group, string[] types) {
            var type = "Group_" + group.EscapeTypeName();
            var properties = types.Select( CreatePropertyDeclaration_Type ).ToArray();
            return SyntaxFactoryUtils.ClassDeclaration( type, "GroupNode" ).AddMembers( properties );
        }

        // Properties
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Module(string module) {
            var type = module;
            var identifier = module.WithoutPrefix().EscapeIdentifier();
            return SyntaxFactoryUtils.PropertyDeclaration( type, identifier, SyntaxFactoryUtils.ObjectCreationExpression( type ) );
        }
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Namespace(string @namespace) {
            var type = "Namespace_" + @namespace.EscapeTypeName();
            var identifier = @namespace.EscapeIdentifier();
            return SyntaxFactoryUtils.PropertyDeclaration( type, identifier, SyntaxFactoryUtils.ObjectCreationExpression( type ) );
        }
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Group(string group) {
            var type = "Group_" + group.EscapeTypeName();
            var identifier = group.EscapeIdentifier();
            return SyntaxFactoryUtils.PropertyDeclaration( type, identifier, SyntaxFactoryUtils.ObjectCreationExpression( type ) );
        }
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Type(string type) {
            var identifier = type.EscapeIdentifier();
            return SyntaxFactoryUtils.PropertyDeclaration( "TypeNode", identifier, SyntaxFactoryUtils.TypeOfExpression( type ) );
        }


        // Helpers/String
        private static string WithoutPrefix(this string value) {
            var i = value.IndexOf( '_' );
            if (i == -1) return value;
            return value.Substring( i + 1 );
        }
        private static string EscapeTypeName(this string value) {
            value = value.Replace( '.', '_' ).Replace( '-', '_' ).Replace( ' ', '_' );
            return value;
        }
        private static string EscapeIdentifier(this string value) {
            value = value.Replace( '.', '_' ).Replace( '-', '_' ).Replace( ' ', '_' );
            if (SyntaxFacts.GetKeywordKind( value ) != SyntaxKind.None) value = "@" + value;
            return value;
        }


    }
}
