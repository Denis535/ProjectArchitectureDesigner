// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Analyzer {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal static class SyntaxAnalyzer {


        // Project
        public static string[] GetProjectData(TypeDeclarationSyntax @class) {
            return @class.GetAttributes().Where( IsModule ).Select( GetModule ).ToArray();
        }

        // Module
        public static (string Namespace, (string Group, string[] Types)[] Groups)[] GetModuleData(TypeDeclarationSyntax @class) {
            return @class.GetAttributes().Where( i => IsNamespace( i ) || IsType( i ) ).Split( IsNamespace ).Select( GetNamespace ).ToArray();
        }
        private static (string, (string, string[])[]) GetNamespace(this IEnumerable<AttributeSyntax> attributes) {
            if (!IsNamespace( attributes.First() )) {
                var @namespace = "Global";
                var group = attributes.Split( HasGroupName ).Select( GetGroup ).ToArray();
                return (@namespace, group);
            } else {
                var @namespace = GetNamespace( attributes.First() );
                var group = attributes.Skip( 1 ).Split( HasGroupName ).Select( GetGroup ).ToArray();
                return (@namespace, group);
            }
        }
        private static (string, string[]) GetGroup(this IEnumerable<AttributeSyntax> attributes) {
            if (!HasGroupName( attributes.First() )) {
                var group = "Default";
                var types = attributes.Select( GetType ).ToArray();
                return (group, types);
            } else {
                var group = GetGroup( attributes.First() );
                var types = attributes.Select( GetType ).ToArray();
                return (group, types);
            }
        }


        // Helpers/Syntax/Attribute
        private static bool IsModule(AttributeSyntax attribute) {
            return attribute.Name.ToString() is "Module";
        }
        private static bool IsNamespace(AttributeSyntax attribute) {
            return attribute.Name.ToString() is "Namespace";
        }
        private static bool HasGroupName(AttributeSyntax attribute) {
            var comment = attribute.Parent!.GetLeadingTrivia().Where( i => i.Kind() == SyntaxKind.SingleLineCommentTrivia ).LastOrDefault();
            return comment.ToString().StartsWith( "// " );
        }
        private static bool IsType(AttributeSyntax attribute) {
            return attribute.Name.ToString() is "Type";
        }


        // Helpers/Syntax/Attribute
        private static string GetModule(AttributeSyntax attribute) {
            var arg = attribute.ArgumentList!.Arguments.First();
            var module = ((TypeOfExpressionSyntax) arg.Expression).Type.ToString();
            return module;
        }
        private static string GetNamespace(AttributeSyntax attribute) {
            var arg = attribute.ArgumentList!.Arguments.First();
            var @namespace = ((LiteralExpressionSyntax) arg.Expression).Token.ValueText;
            return @namespace;
        }
        private static string GetGroup(AttributeSyntax attribute) {
            var comment = attribute.Parent!.GetLeadingTrivia().Where( i => i.Kind() == SyntaxKind.SingleLineCommentTrivia ).LastOrDefault();
            var group = GetCommentContent( comment.ToString() );
            return group;
        }
        private static string GetType(AttributeSyntax attribute) {
            var arg = attribute.ArgumentList!.Arguments.First();
            var type = ((TypeOfExpressionSyntax) arg.Expression).Type.ToString();
            return type;
        }


        // Helpers/Syntax/Misc
        private static string GetCommentContent(string comment) {
            var content = comment.SkipWhile( i => i == '/' ).TakeWhile( i => i != '/' );
            return string.Concat( content ).Trim();
        }


    }
}
