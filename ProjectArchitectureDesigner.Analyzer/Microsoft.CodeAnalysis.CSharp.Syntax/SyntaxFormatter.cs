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

        public static CompilationUnitSyntax FormatIndentation(this CompilationUnitSyntax unit) {
            // [indent] comment [eol]
            // [indent] comment [eol]
            // [indent] ... comment [eol]
            unit = (CompilationUnitSyntax) new SyntaxFormatterHelper2().Visit( unit );
            return unit;
        }

    }
    internal class SyntaxFormatterHelper : CSharpSyntaxRewriter {


        public override SyntaxToken VisitToken(SyntaxToken token) {
            if (token.Parent is null && token.Span.Length == 0) return token;

            // LeadingTrivia
            var leadingTrivia = token.LeadingTrivia.Where( IsNotEmpty );
            leadingTrivia = leadingTrivia.WithEndOfLines();

            // TrailingTrivia
            var trailingTrivia = token.TrailingTrivia.Where( IsNotEmpty );
            if (ShouldBeLast( token, token.GetNextToken() )) {
                trailingTrivia = trailingTrivia.WithEndOfLine();
            } else
            if (ShouldHaveSpaceAfter( token ) && ShouldHaveSpaceBefore( token.GetNextToken() )) {
                trailingTrivia = trailingTrivia.WithSpace();
            }

            return token.WithLeadingTrivia( leadingTrivia ).WithTrailingTrivia( trailingTrivia );
        }


        // Helpers/Token
        private static bool ShouldBeLast(SyntaxToken token, SyntaxToken next) {
            if (token.Parent is AccessorListSyntax) return false;
            if (token.Parent is AccessorDeclarationSyntax) return false;
            if (token.Parent is InitializerExpressionSyntax) return false;

            if (token.Kind() is SyntaxKind.OpenBraceToken) return true;  // { [eol]
            if (token.Kind() is SyntaxKind.CloseBraceToken) return true; // } [eol]
            if (token.Kind() is SyntaxKind.SemicolonToken) return true;  // ; [eol]
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
        private static bool IsNotEmpty(SyntaxTrivia trivia) {
            return trivia.FullSpan.Length > 0;
        }


    }
    internal class SyntaxFormatterHelper2 : CSharpSyntaxRewriter {


        public override SyntaxToken VisitToken(SyntaxToken token) {
            if (token.Parent is null && token.Span.Length == 0) return token;

            // LeadingTrivia
            if (IsFirst( token )) {
                var leadingTrivia = token.LeadingTrivia.AsEnumerable();
                leadingTrivia = leadingTrivia.WithIndents( token.GetIndent() );
                token = token.WithLeadingTrivia( leadingTrivia );
            }
            return token;
        }


        // Helpers/Token
        private static bool IsFirst(SyntaxToken token) {
            var prev = token.GetPreviousToken();
            return
                prev == default ||
                prev.TrailingTrivia.LastOrDefault().Kind() is SyntaxKind.EndOfLineTrivia;
        }


    }
    internal static class SyntaxFormatterHelper3 {


        // LeadingTrivia
        public static IEnumerable<SyntaxTrivia> WithEndOfLines(this IEnumerable<SyntaxTrivia> trivia) {
            foreach (var (trivia_, next) in trivia.WithNext()) {
                if (trivia_.ShouldHaveEndOfLine()) {
                    yield return trivia_;
                    if (!next.ValueOrDefault.IsEndOfLine()) yield return SyntaxFactory.EndOfLine( "\r\n" );
                } else {
                    yield return trivia_;
                }
            }
        }
        public static IEnumerable<SyntaxTrivia> WithIndents(this IEnumerable<SyntaxTrivia> trivia, string indent) {
            foreach (var slice in trivia.SplitByLast( IsEndOfLine )) {
                yield return SyntaxFactory.Whitespace( indent );
                foreach (var i in slice) yield return i;
            }
            yield return SyntaxFactory.Whitespace( indent );
        }
        public static string GetIndent(this SyntaxToken token) {
            var level = token.Parent!.Ancestors().Where( i => i is NamespaceDeclarationSyntax or BaseTypeDeclarationSyntax or InitializerExpressionSyntax or BlockSyntax ).Count();
            return new string( ' ', 4 * level );
        }


        // TrailingTrivia
        public static IEnumerable<SyntaxTrivia> WithEndOfLine(this IEnumerable<SyntaxTrivia> trivia) {
            var result = trivia.ToList();
            if (result.LastOrDefault().ShouldHaveEndOfLine()) result.Add( SyntaxFactory.EndOfLine( "\r\n" ) );
            return result;
        }
        public static IEnumerable<SyntaxTrivia> WithSpace(this IEnumerable<SyntaxTrivia> trivia) {
            var result = trivia.ToList();
            if (result.LastOrDefault().ShouldHaveSpace()) result.Add( SyntaxFactory.Space );
            return result;
        }


        // Helpers
        private static bool IsEndOfLine(this SyntaxTrivia trivia) {
            return trivia.Kind() is SyntaxKind.EndOfLineTrivia or SyntaxKind.SingleLineDocumentationCommentTrivia;
        }
        private static bool ShouldHaveEndOfLine(this SyntaxTrivia trivia) {
            return trivia.Kind() is not (SyntaxKind.EndOfLineTrivia or SyntaxKind.SingleLineDocumentationCommentTrivia);
        }
        private static bool ShouldHaveSpace(this SyntaxTrivia trivia) {
            return trivia.Kind() is not (SyntaxKind.EndOfLineTrivia or SyntaxKind.SingleLineDocumentationCommentTrivia or SyntaxKind.WhitespaceTrivia);
        }


    }
}