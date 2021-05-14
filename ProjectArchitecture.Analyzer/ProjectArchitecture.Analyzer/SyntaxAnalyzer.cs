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
            return @class.GetAttributes().Where( IsModule ).Select( GetModuleType ).ToArray();
        }

        // Module
        public static (string Namespace, (string Group, string[] Types)[] Groups)[] GetModuleData(TypeDeclarationSyntax @class) {
            return @class.GetAttributes().Where( i => IsNamespace( i ) || IsType( i ) ).Split( IsNamespace ).Select( GetNamespace ).ToArray();
        }
        private static (string, (string, string[])[]) GetNamespace(this IEnumerable<AttributeSyntax> attributes) {
            if (!IsNamespace( attributes.First() )) {
                var @namespace = "Namespace_Global";
                var group = attributes.Split( HasGroupName ).Select( GetGroup ).ToArray();
                return (@namespace, group);
            } else {
                var @namespace = GetNamespaceType( attributes.First() );
                var group = attributes.Skip( 1 ).Split( HasGroupName ).Select( GetGroup ).ToArray();
                return (@namespace, group);
            }
        }
        private static (string, string[]) GetGroup(this IEnumerable<AttributeSyntax> attributes) {
            if (!HasGroupName( attributes.First() )) {
                var group = "Group_Default";
                var types = attributes.Select( GetType ).ToArray();
                return (group, types);
            } else {
                var group = GetGroupType( attributes.First() );
                var types = attributes.Select( GetType ).ToArray();
                return (group, types);
            }
        }


        //public static (string Namespace, (string Group, string[] Types)[] Groups)[] GetModuleData(ClassDeclarationSyntax @class) {
        //    return @class.GetAttributes().GetModuleData().ToNamespaceHierarchy().ToArray();
        //}
        //private static IEnumerable<(string Value, int Level)> GetModuleData(this IEnumerable<AttributeSyntax> attributes) {
        //    foreach (var attribute in attributes) {
        //        if (IsNamespace( attribute )) {
        //            var @namespace = GetNamespaceType( attribute );
        //            yield return (@namespace, 0);
        //        } else
        //        if (IsType( attribute )) {
        //            var group = GetGroupType( attribute );
        //            var type = GetType( attribute );
        //            if (group != null) {
        //                yield return (group, 1);
        //                yield return (type, 2);
        //            } else {
        //                yield return (type, 2);
        //            }
        //        }
        //    }
        //}
        //private static IEnumerable<(string Namespace, (string Group, string[] Types)[] Groups)> ToNamespaceHierarchy(this IEnumerable<(string Value, int Level)> flatten) {
        //    return flatten
        //        .Unflatten( i => i.Level == 0, key => key.Value, child => child )
        //        .Select( i => (i.Key ?? "Namespace_Global", i.Children.ToGroupHierarchy().ToArray()) );
        //}
        //private static IEnumerable<(string Group, string[] Types)> ToGroupHierarchy(this IEnumerable<(string Value, int Level)> flatten) {
        //    return flatten
        //        .Unflatten( i => i.Level == 1, key => key.Value, child => child.Value )
        //        .Select( i => (i.Key ?? "Group_Default", i.Children) );
        //}


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
        private static string GetModuleType(AttributeSyntax attribute) {
            var arg = attribute.ArgumentList!.Arguments.First();
            var module = ((TypeOfExpressionSyntax) arg.Expression).Type.ToString();
            return module;
        }
        private static string GetNamespaceType(AttributeSyntax attribute) {
            var arg = attribute.ArgumentList!.Arguments.First();
            var @namespace = ((LiteralExpressionSyntax) arg.Expression).Token.ValueText;
            @namespace = "Namespace_" + @namespace.Replace( '.', '_' );
            return @namespace;
        }
        private static string GetGroupType(AttributeSyntax attribute) {
            var comment = attribute.Parent!.GetLeadingTrivia().Where( i => i.Kind() == SyntaxKind.SingleLineCommentTrivia ).LastOrDefault();
            var group = GetCommentContent( comment.ToString() );
            group = "Group_" + group.Replace( '.', '_' ).Replace( '-', '_' ).Replace( ' ', '_' );
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
