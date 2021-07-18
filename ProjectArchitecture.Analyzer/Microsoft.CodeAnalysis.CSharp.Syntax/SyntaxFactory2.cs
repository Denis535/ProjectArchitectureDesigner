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
        public static ClassDeclarationSyntax ClassDeclaration(string identifier, string @base) {
            var builder = new StringBuilder( 100 );
            builder.Append( "public " );
            builder.Append( "class " );
            builder.Append( identifier );
            builder.Append( ':' );
            builder.Append( @base );
            builder.Append( '{' );
            builder.Append( '}' );
            return (ClassDeclarationSyntax?) SyntaxFactory.ParseMemberDeclaration( builder.ToString() ) ?? throw new Exception( "Class declaration syntax is invalid" );
        }
        public static PropertyDeclarationSyntax PropertyDeclaration_Immediate(string type, string identifier, string expression) {
            var builder = new StringBuilder( 1000 );
            builder.Append( "public " );
            builder.Append( type );
            builder.Append( ' ' );
            builder.Append( identifier );
            builder.Append( '{' );
            builder.Append( "get;" );
            builder.Append( '}' );
            builder.Append( '=' );
            builder.Append( expression );
            builder.Append( ';' );
            return (PropertyDeclarationSyntax?) SyntaxFactory.ParseMemberDeclaration( builder.ToString() ) ?? throw new Exception( "Property declaration syntax is invalid" );
        }
        public static PropertyDeclarationSyntax PropertyDeclaration_Immediate_Overriding(string type, string identifier, string expression) {
            var builder = new StringBuilder( 1000 );
            builder.Append( "public " );
            builder.Append( "override " );
            builder.Append( type );
            builder.Append( ' ' );
            builder.Append( identifier );
            builder.Append( '{' );
            builder.Append( "get;" );
            builder.Append( '}' );
            builder.Append( '=' );
            builder.Append( expression );
            builder.Append( ';' );
            return (PropertyDeclarationSyntax?) SyntaxFactory.ParseMemberDeclaration( builder.ToString() ) ?? throw new Exception( "Property declaration syntax is invalid" );
        }
        public static PropertyDeclarationSyntax PropertyDeclaration_Deferred_Overriding(string type, string identifier, string expression) {
            var builder = new StringBuilder( 1000 );
            builder.Append( "public " );
            builder.Append( "override " );
            builder.Append( type );
            builder.Append( ' ' );
            builder.Append( identifier );
            builder.Append( "=>" );
            builder.Append( expression );
            builder.Append( ';' );
            return (PropertyDeclarationSyntax?) SyntaxFactory.ParseMemberDeclaration( builder.ToString() ) ?? throw new Exception( "Property declaration syntax is invalid" );
        }


        // Syntax/Expressions
        public static string NewArrayExpression(string type, IEnumerable<string> items) {
            var builder = new StringBuilder( 1000 );
            builder.Append( "new " );
            builder.Append( type );
            builder.Append( "[] " );
            builder.Append( '{' );
            builder.AppendJoin( ",", items );
            builder.Append( '}' );
            return builder.ToString();
        }
        public static string TypeOfExpression(string type) {
            return $"typeof({type})";
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
