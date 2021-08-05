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
        public static ClassDeclarationSyntax ClassDeclaration(string text) {
            return (ClassDeclarationSyntax?) SyntaxFactory.ParseMemberDeclaration( text ) ?? throw new Exception( "Class declaration syntax is invalid" );
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
