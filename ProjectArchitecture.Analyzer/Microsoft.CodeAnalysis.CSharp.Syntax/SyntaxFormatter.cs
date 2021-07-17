// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace Microsoft.CodeAnalysis.CSharp.Syntax {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    internal static class SyntaxFormatter {

        public static CompilationUnitSyntax Format(this CompilationUnitSyntax unit) {
            return (CompilationUnitSyntax) new SyntaxFormatterRewriter().Visit( unit );
        }

    }
    internal class SyntaxFormatterRewriter : CSharpSyntaxRewriter {

        public override SyntaxToken VisitToken(SyntaxToken token) {
            if (token.Parent != null && token.Span.Length > 0) {
                return token
                    .WithLeadingTrivia( FormatLeadingTrivia( token ) )
                    .WithTrailingTrivia( FormatTrailingTrivia( token ) );
            }
            return token;
        }


        // Format/Leading
        private static IEnumerable<SyntaxTrivia> FormatLeadingTrivia(SyntaxToken token) {
            // comment [eol]
            // token ...
            if (ShouldBeFirst( token )) {
                var indent = GetIndent( token );
                var trivia = Format( token.LeadingTrivia, indent );
                return trivia.Append( SyntaxFactory.Whitespace( indent ) );
            }
            return Enumerable.Empty<SyntaxTrivia>();
        }
        // Format/Trailing
        private static IEnumerable<SyntaxTrivia> FormatTrailingTrivia(SyntaxToken token) {
            // ... token [space] comment [eol]
            // ... token [eol]
            // ... token [space]
            if (ShouldBeLast( token )) {
                var indent = GetIndent( token );
                var trivia = Format( token.TrailingTrivia, indent ).Skip( 1 );
                if (trivia.Any()) {
                    return SyntaxFactory.Space.Append( trivia );
                } else {
                    return SyntaxFactoryUtils.EndOfLine().AsEnumerable();
                }
            }
            if (ShouldHaveSpace( token, token.GetNextToken() )) {
                return SyntaxFactory.Space.AsEnumerable();
            }
            return Enumerable.Empty<SyntaxTrivia>();
        }


        // Helpers/Token
        private static bool ShouldBeFirst(SyntaxToken token) {
            var prevToken = token.GetPreviousToken();
            return prevToken == default || ShouldBeLast( prevToken );
        }
        private static bool ShouldBeLast(SyntaxToken token) {
            if (token.Kind() == SyntaxKind.OpenBraceToken && token.Parent is CompilationUnitSyntax or MemberDeclarationSyntax or BlockSyntax) return true; // { [eol]
            if (token.Kind() == SyntaxKind.SemicolonToken && token.Parent is CompilationUnitSyntax or UsingDirectiveSyntax or MemberDeclarationSyntax or StatementSyntax) return true; // ; [eol]
            if (token.Kind() == SyntaxKind.CloseBraceToken && token.Parent is CompilationUnitSyntax or MemberDeclarationSyntax or BlockSyntax) return true; // } [eol]
            return false;
        }
        private static bool ShouldHaveSpace(SyntaxToken current, SyntaxToken next) {
            if (current.Kind() is SyntaxKind.OpenParenToken) return false;   // ( [space]
            if (current.Kind() is SyntaxKind.OpenBracketToken) return false; // [ [space]
            if (current.Kind() is SyntaxKind.LessThanToken && current.Parent is TypeArgumentListSyntax) return false;  // < [space]
            if (current.Kind() is SyntaxKind.DotToken) return false;         // . [space]

            if (next.Kind() is SyntaxKind.OpenParenToken) return false;   // [space] (
            if (next.Kind() is SyntaxKind.OpenBracketToken) return false; // [space] [
            if (next.Kind() is SyntaxKind.LessThanToken && next.Parent is TypeArgumentListSyntax) return false;    // [space] <

            if (next.Kind() is SyntaxKind.CloseParenToken) return false;   // [space] )
            if (next.Kind() is SyntaxKind.CloseBracketToken) return false; // [space] ]
            if (next.Kind() is SyntaxKind.GreaterThanToken && next.Parent is TypeArgumentListSyntax) return false;  // [space] >

            if (next.Kind() is SyntaxKind.DotToken) return false;       // [space] .
            if (next.Kind() is SyntaxKind.SemicolonToken) return false; // [space] ;
            return true;
        }
        private static string GetIndent(SyntaxToken token) {
            var level = token.Parent!.Ancestors().Where( i => i is MemberDeclarationSyntax or BlockSyntax ).Count();
            return new string( ' ', 4 * level );
        }
        // Helpers/Trivia
        private static IEnumerable<SyntaxTrivia> Format(IEnumerable<SyntaxTrivia> trivia, string indent) {
            foreach (var trivia_ in trivia.Where( IsNotTrash )) {
                if (trivia_.Kind() is not SyntaxKind.EndOfLineTrivia) {
                    yield return SyntaxFactory.Whitespace( indent );
                    yield return trivia_;
                    yield return SyntaxFactoryUtils.EndOfLine();
                } else {
                    yield return SyntaxFactoryUtils.EndOfLine();
                }
            }
        }
        private static bool IsNotTrash(SyntaxTrivia trivia) {
            return trivia.FullSpan.Length > 0 && trivia.Kind() is not SyntaxKind.WhitespaceTrivia;
        }


    }
}