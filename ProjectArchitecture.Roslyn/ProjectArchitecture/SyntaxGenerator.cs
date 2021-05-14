// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal static class SyntaxGenerator {


        // Class/Project
        public static ClassDeclarationSyntax CreateClassDeclaration_Project(ClassDeclarationSyntax @class, string[] modules) {
            var properties = modules.Select( CreatePropertyDeclaration_Module ).ToArray();
            return SyntaxFactoryUtils.CreateClassDeclaration( @class ).AddMembers( properties );
        }

        // Class/Module
        public static ClassDeclarationSyntax CreateClassDeclaration_Module(ClassDeclarationSyntax @class, (string Namespace, (string Group, string[] Types)[] Groups)[] namespaces) {
            var classes = namespaces.Select( i => CreateClassDeclaration_Namespace( i.Namespace, i.Groups ) ).ToArray();
            var properties = namespaces.Select( i => CreatePropertyDeclaration_Namespace( i.Namespace ) ).ToArray();
            return SyntaxFactoryUtils.CreateClassDeclaration( @class ).AddMembers( classes ).AddMembers( properties );
        }
        private static ClassDeclarationSyntax CreateClassDeclaration_Namespace(string @namespace, (string Group, string[] Types)[] groups) {
            var classes = groups.Select( i => CreateClassDeclaration_Group( i.Group, i.Types ) ).ToArray();
            var properties = groups.Select( i => CreatePropertyDeclaration_Group( i.Group ) ).ToArray();
            return SyntaxFactoryUtils.CreateClassDeclaration( @namespace, "NamespaceNode" ).AddMembers( classes ).AddMembers( properties );
        }
        private static ClassDeclarationSyntax CreateClassDeclaration_Group(string group, string[] types) {
            var properties = types.Select( CreatePropertyDeclaration_Type ).ToArray();
            return SyntaxFactoryUtils.CreateClassDeclaration( group, "GroupNode" ).AddMembers( properties );
        }

        // Property
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Module(string module) {
            var identifier = module.Map( WithoutPrefix ).Map( EscapeIdentifier );
            return SyntaxFactoryUtils.CreatePropertyDeclaration( module, identifier, SyntaxFactoryUtils.CreateObjectCreationExpression( module ) );
        }
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Namespace(string @namespace) {
            var identifier = @namespace.Map( WithoutPrefix ).Map( EscapeIdentifier );
            return SyntaxFactoryUtils.CreatePropertyDeclaration( @namespace, identifier, SyntaxFactoryUtils.CreateObjectCreationExpression( @namespace ) );
        }
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Group(string group) {
            var identifier = group.Map( WithoutPrefix ).Map( EscapeIdentifier );
            return SyntaxFactoryUtils.CreatePropertyDeclaration( group, identifier, SyntaxFactoryUtils.CreateObjectCreationExpression( group ) );
        }
        private static PropertyDeclarationSyntax CreatePropertyDeclaration_Type(string type) {
            var identifier = type.Map( EscapeIdentifier );
            return SyntaxFactoryUtils.CreatePropertyDeclaration( "TypeNode", identifier, SyntaxFactoryUtils.CreateTypeOfExpression( type ) );
        }


        // Helpers/String
        private static string WithoutPrefix(string value) {
            var i = value.IndexOf( '_' );
            if (i == -1) return value;
            return value.Substring( i + 1 );
        }
        private static string EscapeIdentifier(string value) {
            if (SyntaxFacts.GetKeywordKind( value ) == SyntaxKind.None) return value;
            return "@" + value;
        }


    }
}
