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
        public static CompilationUnitSyntax CreateCompilationUnit(CompilationUnitSyntax unit) {
            return CompilationUnit(
                unit.Externs,
                unit.Usings,
                List<AttributeListSyntax>(),
                List<MemberDeclarationSyntax>() );
        }


        // Member
        public static NamespaceDeclarationSyntax CreateNamespaceDeclaration(NamespaceDeclarationSyntax @namespace) {
            return NamespaceDeclaration(
                @namespace.Name,
                @namespace.Externs,
                @namespace.Usings,
                List<MemberDeclarationSyntax>() );
        }
        public static ClassDeclarationSyntax CreateClassDeclaration(ClassDeclarationSyntax @class) {
            return ClassDeclaration(
                List<AttributeListSyntax>(),
                @class.Modifiers,
                @class.Identifier,
                @class.TypeParameterList,
                @class.BaseList,
                @class.ConstraintClauses,
                List<MemberDeclarationSyntax>() );
        }
        public static ClassDeclarationSyntax CreateClassDeclaration(string identifier, string @base) {
            return ClassDeclaration(
                List<AttributeListSyntax>(),
                TokenList( Token( SyntaxKind.PublicKeyword ) ),
                Identifier( identifier ),
                null,
                BaseList( SeparatedList<BaseTypeSyntax>( NodeOrTokenList( SimpleBaseType( ParseTypeName( @base ) ) ) ) ),
                List<TypeParameterConstraintClauseSyntax>(),
                List<MemberDeclarationSyntax>() );
        }
        public static PropertyDeclarationSyntax CreatePropertyDeclaration(string type, string identifier, ExpressionSyntax expression) {
            return PropertyDeclaration(
                List<AttributeListSyntax>(),
                TokenList( Token( SyntaxKind.PublicKeyword ) ),
                ParseTypeName( type ),
                null,
                Identifier( identifier ),
                AccessorList( List( AccessorDeclaration( SyntaxKind.GetAccessorDeclaration, List<AttributeListSyntax>(), TokenList(), Token( SyntaxKind.GetKeyword ), null, null, Token( SyntaxKind.SemicolonToken ) ) ) ),
                null,
                EqualsValueClause( expression ),
                Token( SyntaxKind.SemicolonToken ) );
        }


        // Expression
        public static ExpressionSyntax CreateObjectCreationExpression(string type) {
            return ObjectCreationExpression(
                ParseTypeName( type ),
                ArgumentList(),
                null );
        }
        public static ExpressionSyntax CreateObjectCreationExpression(string type, string argument) {
            return ObjectCreationExpression(
                ParseTypeName( type ),
                ArgumentList( SeparatedList<ArgumentSyntax>( NodeOrTokenList( Literal( argument ) ) ) ),
                null );
        }
        public static ExpressionSyntax CreateTypeOfExpression(string type) {
            return TypeOfExpression(
                ParseTypeName( type )
                );
        }


        // Helpers
        private static SyntaxList<TNode> List<TNode>() where TNode : SyntaxNode {
            return SyntaxFactory.List<TNode>();
        }
        private static SyntaxList<TNode> List<TNode>(TNode node) where TNode : SyntaxNode {
            return SyntaxFactory.List<TNode>( new[] { node }.AsEnumerable() );
        }
        private static SyntaxList<TNode> List<TNode>(params TNode[] nodes) where TNode : SyntaxNode {
            return SyntaxFactory.List<TNode>( nodes );
        }


    }
}
