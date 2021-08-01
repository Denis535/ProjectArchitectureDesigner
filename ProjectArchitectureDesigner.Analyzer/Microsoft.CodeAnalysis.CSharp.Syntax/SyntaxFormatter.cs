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
            // [indent] comment [eol]
            // [indent] comment [eol]
            // [indent] ... comment [eol]
            unit = (CompilationUnitSyntax) new SyntaxFormatterHelper().Visit( unit );
            unit = (CompilationUnitSyntax) new SyntaxFormatterHelper2().Visit( unit );
            return unit;
        }

    }
    internal class SyntaxFormatterHelper : CSharpSyntaxRewriter {


        public override SyntaxToken VisitToken(SyntaxToken token) {
            if (token.Parent is null && token.Span.Length == 0) return token;

            // LeadingTrivia
            var leadingTrivia = token.LeadingTrivia.WhereNot( IsTrash );
            leadingTrivia = WithEndOfLines( leadingTrivia );

            // TrailingTrivia
            var trailingTrivia = token.TrailingTrivia.WhereNot( IsTrash );
            if (ShouldBeLast( token )) {
                trailingTrivia = WithEndOfLine( trailingTrivia );
            } else
            if (ShouldHaveSpaceAfter( token ) && ShouldHaveSpaceBefore( token.GetNextToken() )) {
                trailingTrivia = WithSpace( trailingTrivia );
            }

            return token.WithLeadingTrivia( leadingTrivia ).WithTrailingTrivia( trailingTrivia );
        }


        // Helpers/Token
        private static bool ShouldBeLast(SyntaxToken token) {
            if (token.Parent is CompilationUnitSyntax or MemberDeclarationSyntax or BlockSyntax) {
                if (token.Kind() == SyntaxKind.OpenBraceToken) return true;  // { [eol]
                if (token.Kind() == SyntaxKind.CloseBraceToken) return true; // } [eol]
            }
            if (token.Parent is CompilationUnitSyntax or UsingDirectiveSyntax or MemberDeclarationSyntax or StatementSyntax) {
                if (token.Kind() == SyntaxKind.SemicolonToken) return true;  // ; [eol]
            }
            return false;
        }
        private static bool ShouldHaveSpaceAfter(SyntaxToken token) {
            if (token.Kind() is SyntaxKind.OpenParenToken) return false;    // ( token
            if (token.Kind() is SyntaxKind.OpenBracketToken) return false;  // [ token
            if (token.Parent is TypeArgumentListSyntax) {
                if (token.Kind() is SyntaxKind.LessThanToken) return false; // < token
            }
            if (token.Kind() is SyntaxKind.DotToken) return false;          // . token
            return true;
        }
        private static bool ShouldHaveSpaceBefore(SyntaxToken next) {
            if (next.Kind() is SyntaxKind.OpenParenToken) return false;       // token (
            if (next.Kind() is SyntaxKind.OpenBracketToken) return false;     // token [
            if (next.Parent is TypeArgumentListSyntax) {
                if (next.Kind() is SyntaxKind.LessThanToken) return false;    // token <
            }
            if (next.Kind() is SyntaxKind.CloseParenToken) return false;      // token )
            if (next.Kind() is SyntaxKind.CloseBracketToken) return false;    // token ]
            if (next.Parent is TypeArgumentListSyntax) {
                if (next.Kind() is SyntaxKind.GreaterThanToken) return false; // token >
            }
            if (next.Kind() is SyntaxKind.DotToken) return false;             // token .
            if (next.Kind() is SyntaxKind.CommaToken) return false;           // token ,
            if (next.Kind() is SyntaxKind.SemicolonToken) return false;       // token ;
            return true;
        }
        // Helpers/Trivia
        private static IEnumerable<SyntaxTrivia> WithEndOfLines(IEnumerable<SyntaxTrivia> trivia) {
            foreach (var (trivia_, next) in trivia.WithNext()) {
                if (trivia_.Kind() is SyntaxKind.WhitespaceTrivia or SyntaxKind.EndOfLineTrivia) {
                    yield return trivia_;
                } else {
                    yield return trivia_;
                    if (next.ValueOrDefault.Kind() is not SyntaxKind.EndOfLineTrivia) yield return SyntaxFactory2.EndOfLine();
                }
            }
        }
        private static IEnumerable<SyntaxTrivia> WithEndOfLine(IEnumerable<SyntaxTrivia> trivia) {
            var result = trivia.ToList();
            if (result.LastOrDefault().Kind() is not SyntaxKind.EndOfLineTrivia) {
                result.Add( SyntaxFactory2.EndOfLine() );
            }
            return result;
        }
        private static IEnumerable<SyntaxTrivia> WithSpace(IEnumerable<SyntaxTrivia> trivia) {
            var result = trivia.ToList();
            if (result.LastOrDefault().Kind() is not (SyntaxKind.WhitespaceTrivia or SyntaxKind.EndOfLineTrivia)) {
                result.Add( SyntaxFactory.Space );
            }
            return result;
        }
        private static bool IsTrash(SyntaxTrivia trivia) {
            return trivia.FullSpan.Length == 0;
        }


    }
    internal class SyntaxFormatterHelper2 : CSharpSyntaxRewriter {


        public override SyntaxToken VisitToken(SyntaxToken token) {
            if (token.Parent is null && token.Span.Length == 0) return token;

            // LeadingTrivia
            if (IsLineStart( token )) {
                var indent = GetIndent( token );
                var trivia = token.LeadingTrivia.AsEnumerable();
                trivia = Indent( trivia, indent );
                token = token.WithLeadingTrivia( trivia );
            }
            return token;
        }


        // Helpers/Token
        private static bool IsLineStart(SyntaxToken token) {
            var prev = token.GetPreviousToken();
            return
                prev == default ||
                prev.TrailingTrivia.LastOrDefault().Kind() is SyntaxKind.EndOfLineTrivia;
        }
        private static string GetIndent(SyntaxToken token) {
            var level = token.Parent!.Ancestors().Where( i => i is NamespaceDeclarationSyntax or BaseTypeDeclarationSyntax or InitializerExpressionSyntax or BlockSyntax ).Count();
            return new string( ' ', 4 * level );
        }
        // Helpers/Trivia
        private static IEnumerable<SyntaxTrivia> Indent(IEnumerable<SyntaxTrivia> trivia, string indent) {
            foreach (var slice in trivia.SplitByLast( i => i.Kind() is SyntaxKind.EndOfLineTrivia )) {
                yield return SyntaxFactory.Whitespace( indent );
                foreach (var i in slice) yield return i;
            }
            yield return SyntaxFactory.Whitespace( indent );
        }


    }
}