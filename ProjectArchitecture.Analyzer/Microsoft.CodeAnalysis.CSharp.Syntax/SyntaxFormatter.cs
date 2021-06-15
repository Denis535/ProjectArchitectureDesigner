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
            unit = (CompilationUnitSyntax) new SyntaxFormatterRewriter().Visit( unit );
            unit = (CompilationUnitSyntax) new SyntaxIndentFormatterRewriter().Visit( unit );
            return unit;
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


        // Format
        private static IEnumerable<SyntaxTrivia> FormatLeadingTrivia(SyntaxToken token) {
            // comment [eol]
            // token ...
            if (IsDesiredLineStart( token )) {
                var trivia = token.LeadingTrivia.Where( IsNotTrash );
                if (trivia.Any()) {
                    return WithEndOfLines( trivia );
                } else {
                    return Enumerable.Empty<SyntaxTrivia>();
                }
            }
            return Enumerable.Empty<SyntaxTrivia>();
        }
        private static IEnumerable<SyntaxTrivia> FormatTrailingTrivia(SyntaxToken token) {
            // ... token [space] comment [eol]
            // ... token [eol]
            // ... token [space]
            if (IsDesiredLineEnd( token )) {
                var trivia = token.TrailingTrivia.Where( IsNotTrash );
                if (trivia.Any()) {
                    return SyntaxFactory.Space.Append( WithEndOfLines( trivia ) );
                } else {
                    return SyntaxFactoryUtils.EndOfLine().AsEnumerable();
                }
            }
            if (HasDesiredSpace( token )) {
                return SyntaxFactory.Space.AsEnumerable();
            }
            return Enumerable.Empty<SyntaxTrivia>();
        }


        // Helpers/Token
        private static bool IsDesiredLineStart(SyntaxToken token) {
            var prevToken = token.GetPreviousToken();
            return prevToken == default || IsDesiredLineEnd( prevToken );
        }
        private static bool IsDesiredLineEnd(SyntaxToken token) {
            if (token.Kind() == SyntaxKind.OpenBraceToken && token.Parent is CompilationUnitSyntax or MemberDeclarationSyntax or BlockSyntax) return true; // { [eol]
            if (token.Kind() == SyntaxKind.SemicolonToken && token.Parent is CompilationUnitSyntax or UsingDirectiveSyntax or MemberDeclarationSyntax or StatementSyntax) return true; // ; [eol]
            if (token.Kind() == SyntaxKind.CloseBraceToken && token.Parent is CompilationUnitSyntax or MemberDeclarationSyntax or BlockSyntax) return true; // } [eol]
            return false;
        }
        private static bool HasDesiredSpace(SyntaxToken token) {
            var nextToken = token.GetNextToken();
            if (token.Kind() is SyntaxKind.DotToken) return false; // . [space]
            if (token.Kind() is SyntaxKind.OpenParenToken or SyntaxKind.OpenBracketToken) return false; // ([ [space]
            if (nextToken.Kind() is SyntaxKind.DotToken) return false; // [space] .
            if (nextToken.Kind() is SyntaxKind.OpenParenToken or SyntaxKind.OpenBracketToken) return false; // [space] ([
            if (nextToken.Kind() is SyntaxKind.CloseParenToken or SyntaxKind.CloseBracketToken) return false; // [space] ])
            if (nextToken.Kind() is SyntaxKind.SemicolonToken) return false; // [space] ;
            return true;
        }
        // Helpers/Trivia
        private static bool IsNotTrash(SyntaxTrivia trivia) {
            return trivia.FullSpan.Length > 0 && trivia.Kind() is not SyntaxKind.WhitespaceTrivia;
        }
        private static IEnumerable<SyntaxTrivia> WithEndOfLines(IEnumerable<SyntaxTrivia> trivia) {
            foreach (var trivia_ in trivia) {
                if (trivia_.Kind() is not SyntaxKind.EndOfLineTrivia) {
                    yield return trivia_;
                    yield return SyntaxFactoryUtils.EndOfLine();
                } else {
                    yield return SyntaxFactoryUtils.EndOfLine();
                }
            }
        }

    }
    internal class SyntaxIndentFormatterRewriter : CSharpSyntaxRewriter {

        public override SyntaxToken VisitToken(SyntaxToken token) {
            if (token.Parent != null && token.Span.Length > 0) {
                return token
                    .WithLeadingTrivia( FormatLeadingTrivia( token ) )
                    .WithTrailingTrivia( FormatTrailingTrivia( token ) );
            }
            return token;
        }


        // Format
        private static IEnumerable<SyntaxTrivia> FormatLeadingTrivia(SyntaxToken token) {
            if (IsLineStart( token )) {
                var indent = GetIndent( token );
                if (token.HasLeadingTrivia) {
                    return WithIndents( token.LeadingTrivia, indent ).Append( SyntaxFactory.Whitespace( indent ) );
                } else {
                    return SyntaxFactory.Whitespace( indent ).AsEnumerable();
                }
            }
            return token.LeadingTrivia;
        }
        private static IEnumerable<SyntaxTrivia> FormatTrailingTrivia(SyntaxToken token) {
            if (IsLineEnd( token )) {
                var indent = GetIndent( token );
                if (token.HasTrailingTrivia) {
                    return WithIndents( token.TrailingTrivia, indent ).Skip( 1 );
                } else {
                    return Enumerable.Empty<SyntaxTrivia>();
                }
            }
            return token.TrailingTrivia;
        }


        // Helpers/Token
        private static bool IsLineStart(SyntaxToken token) {
            var prevToken = token.GetPreviousToken();
            return prevToken == default || prevToken.TrailingTrivia.LastOrDefault().Kind() is SyntaxKind.EndOfLineTrivia;
        }
        private static bool IsLineEnd(SyntaxToken token) {
            return token.TrailingTrivia.Any( i => i.Kind() is SyntaxKind.EndOfLineTrivia );
        }
        private static string GetIndent(SyntaxToken token) {
            var level = token.Parent!.Ancestors().Where( i => i is MemberDeclarationSyntax or BlockSyntax ).Count();
            return new string( ' ', 4 * level );
        }
        // Helpers/Trivia
        private static IEnumerable<SyntaxTrivia> WithIndents(IEnumerable<SyntaxTrivia> trivia, string indent) {
            static bool IsLineEnd(SyntaxTrivia trivia) => trivia.Kind() is SyntaxKind.EndOfLineTrivia;
            foreach (var trivia_ in trivia.SplitByLast( IsLineEnd )) {
                yield return SyntaxFactory.Whitespace( indent );
                foreach (var i in trivia_) yield return i;
            }
        }


    }
}