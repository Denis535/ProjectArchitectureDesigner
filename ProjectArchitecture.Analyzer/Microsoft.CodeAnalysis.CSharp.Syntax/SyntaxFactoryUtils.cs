// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace Microsoft.CodeAnalysis.CSharp.Syntax {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    internal static class SyntaxFactoryUtils {


        // CompilationUnit
        public static CompilationUnitSyntax CompilationUnit(CompilationUnitSyntax unit) {
            return SyntaxFactory.CompilationUnit(
                unit.Externs.WithoutTrivia(),
                unit.Usings.WithoutTrivia(),
                List<AttributeListSyntax>(),
                List<MemberDeclarationSyntax>()
                );
        }


        // Members
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
        public static ClassDeclarationSyntax ClassDeclaration(string identifier, string @base) {
            return SyntaxFactory.ClassDeclaration(
                List<AttributeListSyntax>(),
                TokenList( Token( SyntaxKind.PublicKeyword ), Token( SyntaxKind.SealedKeyword ) ),
                Identifier( identifier ),
                null,
                BaseList( SeparatedList<BaseTypeSyntax>( NodeOrTokenList( SimpleBaseType( ParseTypeName( @base ) ) ) ) ),
                List<TypeParameterConstraintClauseSyntax>(),
                List<MemberDeclarationSyntax>()
                );
        }
        public static PropertyDeclarationSyntax PropertyDeclaration(string type, string identifier, ExpressionSyntax expression) {
            return SyntaxFactory.PropertyDeclaration(
                List<AttributeListSyntax>(),
                TokenList( Token( SyntaxKind.PublicKeyword ) ),
                ParseTypeName( type ),
                null,
                Identifier( identifier ),
                AccessorList( List( AccessorDeclaration( SyntaxKind.GetAccessorDeclaration, List<AttributeListSyntax>(), TokenList(), Token( SyntaxKind.GetKeyword ), null, null, Token( SyntaxKind.SemicolonToken ) ) ) ),
                null,
                EqualsValueClause( expression ),
                Token( SyntaxKind.SemicolonToken )
                );
        }
        public static PropertyDeclarationSyntax PropertyDeclaration_Overriding(string type, string identifier, ExpressionSyntax expression) {
            return SyntaxFactory.PropertyDeclaration(
                List<AttributeListSyntax>(),
                TokenList( Token( SyntaxKind.PublicKeyword ), Token( SyntaxKind.OverrideKeyword ) ),
                ParseTypeName( type ),
                null,
                Identifier( identifier ),
                AccessorList( List( AccessorDeclaration( SyntaxKind.GetAccessorDeclaration, List<AttributeListSyntax>(), TokenList(), Token( SyntaxKind.GetKeyword ), null, null, Token( SyntaxKind.SemicolonToken ) ) ) ),
                null,
                EqualsValueClause( expression ),
                Token( SyntaxKind.SemicolonToken )
                );
        }


        // Expressions
        public static ExpressionSyntax StringLiteral(string value) {
            return LiteralExpression(
                SyntaxKind.StringLiteralExpression,
                Token( default, SyntaxKind.StringLiteralToken, '\"' + value + '\"', value, default )
                );
        }
        public static ExpressionSyntax ObjectCreationExpression(string type) {
            return SyntaxFactory.ObjectCreationExpression(
                ParseTypeName( type ),
                ArgumentList(),
                null
                );
        }
        //public static ExpressionSyntax ObjectCreationExpression(string type, string argument) {
        //    return SyntaxFactory.ObjectCreationExpression(
        //        ParseTypeName( type ),
        //        ArgumentList( SeparatedList<ArgumentSyntax>( NodeOrTokenList( Literal( argument ) ) ) ),
        //        null
        //        );
        //}
        public static ExpressionSyntax TypeOfExpression(string type) {
            return SyntaxFactory.TypeOfExpression(
                ParseTypeName( type )
                );
        }


        // Comment
        public static SyntaxTrivia Comment(string format, params object[] args) {
            return SyntaxFactory.Comment( string.Format( format, args ) );
        }


        // Helpers
        private static SyntaxList<TNode> List<TNode>() where TNode : SyntaxNode {
            return SyntaxFactory.List<TNode>();
        }
        private static SyntaxList<TNode> List<TNode>(TNode node) where TNode : SyntaxNode {
            return SyntaxFactory.List( new[] { node }.AsEnumerable() );
        }
        private static SyntaxList<TNode> List<TNode>(params TNode[] nodes) where TNode : SyntaxNode {
            return SyntaxFactory.List( nodes );
        }
        public static SyntaxList<TNode> WithoutTrivia<TNode>(this SyntaxList<TNode> nodes) where TNode : SyntaxNode {
            return SyntaxFactory.List( nodes.Select( i => i.WithoutTrivia() ) );
        }
        public static SyntaxTokenList WithoutTrivia(this SyntaxTokenList tokens) {
            return SyntaxFactory.TokenList( tokens.Select( i => i.WithoutTrivia() ) );
        }


    }
}
