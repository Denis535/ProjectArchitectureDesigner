// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace Microsoft.CodeAnalysis.CSharp.Syntax {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal static class SyntaxFactory2 {


        // Syntax
        public static CompilationUnitSyntax CompilationUnit(CompilationUnitSyntax unit) {
            return SyntaxFactory.CompilationUnit(
                unit.Externs.WithoutTrivia(),
                unit.Usings.WithoutTrivia(),
                List<AttributeListSyntax>(),
                List<MemberDeclarationSyntax>()
                );
        }
        // Syntax/Declarations
        public static NamespaceDeclarationSyntax NamespaceDeclaration(NamespaceDeclarationSyntax @namespace) {
            return SyntaxFactory.NamespaceDeclaration(
                @namespace.Name.WithoutTrivia(),
                @namespace.Externs.WithoutTrivia(),
                @namespace.Usings.WithoutTrivia(),
                List<MemberDeclarationSyntax>()
                );
        }
        public static ClassDeclarationSyntax ClassDeclaration(ClassDeclarationSyntax @class) {
            return SyntaxFactory.ClassDeclaration(
                List<AttributeListSyntax>(),
                @class.Modifiers.WithoutTrivia(),
                @class.Identifier.WithoutTrivia(),
                @class.TypeParameterList?.WithoutTrivia(),
                @class.BaseList?.WithoutTrivia(),
                @class.ConstraintClauses.WithoutTrivia(),
                List<MemberDeclarationSyntax>()
                );
        }


        // Syntax/Declarations
        public static ClassDeclarationSyntax ClassDeclaration(string syntax, params object[] args) {
            return (ClassDeclarationSyntax?) SyntaxFactory.ParseMemberDeclaration( syntax.Format2( args ) ) ?? throw new Exception( "Class declaration syntax is invalid" );
        }
        public static PropertyDeclarationSyntax PropertyDeclaration(string syntax, params object[] args) {
            return (PropertyDeclarationSyntax?) SyntaxFactory.ParseMemberDeclaration( syntax.Format2( args ) ) ?? throw new Exception( "Property declaration syntax is invalid" );
        }
        public static ConstructorDeclarationSyntax ConstructorDeclaration(string syntax, params object[] args) {
            return (ConstructorDeclarationSyntax?) SyntaxFactory.ParseMemberDeclaration( syntax.Format2( args ) ) ?? throw new Exception( "Constructor declaration syntax is invalid" );
        }


        // Trivia
        public static SyntaxTrivia Comment(string format, params object[] args) {
            return SyntaxFactory.Comment( string.Format( format, args ) );
        }
        public static SyntaxTrivia EndOfLine() {
            return SyntaxFactory.EndOfLine( "\r\n" );
        }


        // Helpers/Syntax
        private static SyntaxList<TNode> List<TNode>() where TNode : SyntaxNode {
            return SyntaxFactory.List<TNode>();
        }
        private static SyntaxList<TNode> List<TNode>(TNode node) where TNode : SyntaxNode {
            return SyntaxFactory.List( new[] { node } );
        }
        private static SyntaxList<TNode> List<TNode>(params TNode[] nodes) where TNode : SyntaxNode {
            return SyntaxFactory.List( nodes );
        }
        private static SyntaxList<TNode> WithoutTrivia<TNode>(this SyntaxList<TNode> nodes) where TNode : SyntaxNode {
            return SyntaxFactory.List( nodes.Select( i => i.WithoutTrivia() ) );
        }
        private static SyntaxTokenList WithoutTrivia(this SyntaxTokenList tokens) {
            return SyntaxFactory.TokenList( tokens.Select( i => i.WithoutTrivia() ) );
        }


    }
}
