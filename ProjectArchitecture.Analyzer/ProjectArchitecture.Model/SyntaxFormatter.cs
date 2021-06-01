// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Model {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal static class SyntaxFormatter {

        public static CompilationUnitSyntax Format(this CompilationUnitSyntax unit) {
            unit = (CompilationUnitSyntax) new BlankLinesFormatter().Visit( unit );
            unit = (CompilationUnitSyntax) new OpenBracesFormatter().Visit( unit );
            return unit;
        }

    }
    internal class BlankLinesFormatter : CSharpSyntaxRewriter {
        public override SyntaxToken VisitToken(SyntaxToken token) {
            // Remove extra blank lines
            if (token.HasLeadingTrivia && token.LeadingTrivia.First().Kind() == SyntaxKind.EndOfLineTrivia) {
                var leadingTrivia = token.LeadingTrivia.SkipWhile( i => i.Kind() == SyntaxKind.EndOfLineTrivia ); // Remove first blank lines
                token = token.WithLeadingTrivia( leadingTrivia );
            }
            if (token.HasTrailingTrivia && token.TrailingTrivia.Last().Kind() == SyntaxKind.EndOfLineTrivia) {
                var trailingTrivia = token.TrailingTrivia.Reverse().SkipWhile( i => i.Kind() == SyntaxKind.EndOfLineTrivia ).Reverse().Append( SyntaxFactory.EndOfLine( "\r\n" ) ); // Remove last blank lines
                token = token.WithTrailingTrivia( trailingTrivia );
            }
            return token;
        }
    }
    internal class OpenBracesFormatter : CSharpSyntaxRewriter {
        public override SyntaxToken VisitToken(SyntaxToken token) {
            // Make OpenBraceToken don't start with new line
            var next = token.GetNextToken();
            if (token.HasTrailingTrivia && next.Kind() == SyntaxKind.OpenBraceToken) { // Token is before OpenBraceToken
                token = token.WithTrailingTrivia( SyntaxFactory.Whitespace( " " ) ); // Remove end of line before OpenBraceToken and add whitespace
            }
            if (token.HasLeadingTrivia && token.Kind() == SyntaxKind.OpenBraceToken) { // Token is OpenBraceToken
                token = token.WithLeadingTrivia(); // Remove whitespace before OpenBraceToken
            }
            return token;
        }
    }
}