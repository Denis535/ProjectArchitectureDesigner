// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace System.Text {
    using System;
    using System.Collections.Generic;

    public class HierarchicalStringBuilder {
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

        public const string TitlePrefix = "";
        public const string SectionPrefix = "| - ";
        public const string LinePrefix = "| - ";
        public const string IndentPrefix = "|   ";
        public const string IndentPrefix_Empty = "    ";

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


        // Clear
        public void Clear() {
            Nodes.Clear();
        }


        // Utils
        public override string ToString() {
            EnsureLevelIsZero( Level );
            var builder = new StringBuilder();
            AppendHierarchy( builder, Nodes, 0, "" );
            return builder.ToString();
        }


        // Helpers/Ensure
        private static void EnsureLevelIsZero(int level) {
            if (level == 0) return;
            throw new InvalidOperationException( "Level must be zero: " + level );
        }
        private static void EnsureLevelIsGreaterThanZero(int level) {
            if (level > 0) return;
            throw new InvalidOperationException( "Level must be not zero: " + level );
        }
        // Helpers/AppendHierarchy
        private static void AppendHierarchy(StringBuilder builder, IEnumerable<Node> nodes, int level, string indent) {
            foreach (var ((node, children), isLast) in nodes.Unflatten( i => i.Level == level ).TagLast()) {
                builder.Append( indent ).Append( node.Value.Prefix ).AppendLine( node.Value.Text );
                if (level == 0) {
                    AppendHierarchy( builder, children, level + 1, indent );
                } else {
                    var indent2 = !isLast ? IndentPrefix : IndentPrefix_Empty;
                    AppendHierarchy( builder, children, level + 1, indent + indent2 );
                }
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