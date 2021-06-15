// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace System.Text {
    using System;
    using System.Collections.Generic;

    internal static class StringBuilderExtensions {
        public static StringBuilder AppendLineFormat(this StringBuilder builder, string format, params object?[] args) {
            return builder.AppendLine( string.Format( format, args ) );
        }
    }
    internal class HierarchicalStringBuilder {
        private struct Node {
            public int Level { get; }
            public string Prefix { get; }
            public string Text { get; }
            public Node(int level, string prefix, string text) {
                (Level, Prefix, Text) = (level, prefix, text);
            }
            public override string ToString() {
                return string.Format( "Node: {0}, {1}", Level, Text );
            }
        }
        private class Scope : IDisposable {
            private readonly HierarchicalStringBuilder builder;
            public Scope(HierarchicalStringBuilder builder) {
                this.builder = builder;
                this.builder.Level++;
            }
            public void Dispose() {
                this.builder.Level--;
            }
        }

        private const string IndentPrefix = "|   ";
        private const string IndentEmptyPrefix = "    ";
        private const string TitlePrefix = "";
        private const string SectionPrefix = "| - ";
        private const string LinePrefix = "| - ";

        private List<Node> Nodes { get; }
        private int Level { get; set; }


        public HierarchicalStringBuilder() {
            Nodes = new List<Node>();
            Level = 0;
        }
        private HierarchicalStringBuilder(List<Node> nodes, int level) {
            Nodes = nodes;
            Level = level;
        }


        // Append/Scope
        public IDisposable AppendTitle(string text, params object?[] args) {
            EnsureLevelIsZero( Level );
            AppendNode( TitlePrefix, text, args );
            return new Scope( this );
        }
        public IDisposable AppendSection(string text, params object?[] args) {
            EnsureLevelIsGreaterThanZero( Level );
            AppendNode( SectionPrefix, text, args );
            return new Scope( this );
        }
        // Append/Line
        public HierarchicalStringBuilder AppendLine(string text, params object?[] args) {
            EnsureLevelIsGreaterThanZero( Level );
            return AppendNode( LinePrefix, text, args );
        }
        public HierarchicalStringBuilder AppendLineWithPrefix(string prefix, string text, params object?[] args) {
            EnsureLevelIsGreaterThanZero( Level );
            return AppendNode( prefix, text, args );
        }
        // Append/Node
        private HierarchicalStringBuilder AppendNode(string prefix, string text, object?[]? args = null) {
            if (args != null) text = Format( text, args );
            Nodes.Add( new Node( Level, prefix, text ) );
            return this;
        }


        // WithIndent
        public HierarchicalStringBuilder WithIndent() {
            return new HierarchicalStringBuilder( Nodes, Level + 1 );
        }


        // Utils
        public override string ToString() {
            if (Level != 0) throw new InvalidOperationException( "Level is invalid: " + Level );
            var builder = new StringBuilder();
            AppendHierarchy( builder, ToHierarchy( Nodes ) );
            return builder.ToString();
        }


        // Helpers
        private static void EnsureLevelIsZero(int level) {
            if (level == 0) return;
            throw new InvalidOperationException( "Level must be zero: " + level );
        }
        private static void EnsureLevelIsGreaterThanZero(int level) {
            if (level > 0) return;
            throw new InvalidOperationException( "Level must be not zero: " + level );
        }
        // Helpers/Hierarchy
        private static IReadOnlyList<object> ToHierarchy(IReadOnlyList<Node> nodes) {
            var index = 0;
            return ToHierarchy( nodes, 0, ref index );
        }
        private static IReadOnlyList<object> ToHierarchy(IReadOnlyList<Node> nodes, int level, ref int index) {
            var result = new List<object>();
            for (; index < nodes.Count;) {
                var node = nodes[ index ];
                if (node.Level == level) {
                    result.Add( node );
                    index++;
                } else
                if (node.Level > level) {
                    result.Add( ToHierarchy( nodes, node.Level, ref index ) );
                } else
                if (node.Level < level) {
                    break;
                }
            }
            return result;
        }
        // Helpers/StringBuilder
        private static void AppendHierarchy(StringBuilder builder, IReadOnlyList<object> hierarchy) {
            foreach (var item in hierarchy) {
                if (item is Node node) {
                    builder.AppendLine( node.Text );
                    continue;
                }
                if (item is IReadOnlyList<object> children) {
                    AppendHierarchy( builder, children, "" );
                    continue;
                }
                throw new Exception( "Hierarchy item is invalid: " + item );
            }
        }
        private static void AppendHierarchy(StringBuilder builder, IReadOnlyList<object> hierarchy, string indent) {
            for (var i = 0; i < hierarchy.Count; i++) {
                var item = hierarchy[ i ];
                var isLast = i == hierarchy.Count - 1;
                if (item is Node node) {
                    builder.Append( indent ).Append( node.Prefix ).AppendLine( node.Text );
                    continue;
                }
                if (item is IReadOnlyList<object> children) {
                    if (!isLast) {
                        AppendHierarchy( builder, children, indent + IndentPrefix );
                    } else {
                        AppendHierarchy( builder, children, indent + IndentEmptyPrefix );
                    }
                    continue;
                }
                throw new Exception( "Hierarchy item is invalid: " + item );
            }
        }
        // Helpers/String
        private static string Format(string text, object?[] args) {
            for (var i = 0; i < args.Length; i++) {
                args[ i ] = args[ i ] ?? "Null";
            }
            return string.Format( text, args );
        }


    }
}
