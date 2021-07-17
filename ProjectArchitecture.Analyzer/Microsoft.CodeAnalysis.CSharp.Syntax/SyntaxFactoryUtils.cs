// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace Microsoft.CodeAnalysis.CSharp.Syntax {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal static class SyntaxFactoryUtils {


        // Syntax
        public static CompilationUnitSyntax CompilationUnit(CompilationUnitSyntax unit) {
            return SyntaxFactory.CompilationUnit(
                unit.Externs.WithoutTrivia(),
                unit.Usings.WithoutTrivia(),
                List<AttributeListSyntax>(),
                List<MemberDeclarationSyntax>()
                );
        }
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
            var syntax = $"public class {identifier} : {@base} {{\r\n}}";
            return (ClassDeclarationSyntax?) SyntaxFactory.ParseMemberDeclaration( syntax ) ?? throw new Exception( "Class declaration syntax is invalid" );
        }
        public static PropertyDeclarationSyntax PropertyDeclaration(string type, string identifier, string expression) {
            var syntax = $"public {type} {identifier} {{ get; }} = {expression};";
            return (PropertyDeclarationSyntax?) SyntaxFactory.ParseMemberDeclaration( syntax ) ?? throw new Exception( "Property declaration syntax is invalid" );
        }
        public static PropertyDeclarationSyntax PropertyDeclaration_Overriding(string type, string identifier, string expression) {
            var syntax = $"public override {type} {identifier} {{ get; }} = {expression};";
            return (PropertyDeclarationSyntax?) SyntaxFactory.ParseMemberDeclaration( syntax ) ?? throw new Exception( "Property declaration syntax is invalid" );
        }


        // Trivia
        public static SyntaxTrivia Comment(string format, params object[] args) {
            return SyntaxFactory.Comment( string.Format( format, args ) );
        }
        public static SyntaxTrivia EndOfLine() {
            return SyntaxFactory.EndOfLine( "\r\n" );
        }


        // Helpers
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
