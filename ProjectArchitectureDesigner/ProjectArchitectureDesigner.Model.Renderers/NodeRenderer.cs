// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ProjectArchitectureDesigner.Model;

    public abstract class NodeRenderer {
        internal NodeRenderer? Source { get; }
        public NodeRenderer(NodeRenderer? source) {
            Source = source;
        }
        public virtual string Render(ArchNode node, string text) {
            return text;
        }
        public virtual string Highlight(ArchNode node, string text) {
            return text;
        }
    }
    public class TextNodeRenderer : NodeRenderer {
        public TextNodeRenderer(NodeRenderer? source = null) : base( source ) {
        }
        public override string Render(ArchNode node, string text) {
            return node switch {
                ProjectArchNode
                => "Project: {0}".Format( text ),
                ModuleArchNode
                => "Module: {0}".Format( text ),
                NamespaceArchNode
                => "Namespace: {0}".Format( text ),
                GroupArchNode
                => "{0}".Format( text ),
                TypeArchNode
                => "{0}".Format( text ),
                { } => throw new ArgumentException( "Node is invalid: " + node ),
                null => throw new ArgumentNullException( nameof( node ), "Node is null" ),
            };
        }
    }
    public class LeftAlignedTextNodeRenderer : NodeRenderer {
        public LeftAlignedTextNodeRenderer(NodeRenderer? source = null) : base( source ) {
        }
        public override string Render(ArchNode node, string text) {
            return node switch {
                ProjectArchNode
                => "Project: ——— {0}".Format( text ),
                ModuleArchNode
                => "Module: ———— {0}".Format( text ),
                NamespaceArchNode
                => "Namespace: — {0}".Format( text ),
                GroupArchNode
                => "Group: ————— {0}".Format( text ),
                TypeArchNode
                => "Type: —————— {0}".Format( text ),
                { } => throw new ArgumentException( "Node is invalid: " + node ),
                null => throw new ArgumentNullException( nameof( node ), "Node is null" ),
            };
        }
    }
    public class RightAlignedTextNodeRenderer : NodeRenderer {
        public RightAlignedTextNodeRenderer(NodeRenderer? source = null) : base( source ) {
        }
        public override string Render(ArchNode node, string text) {
            return node switch {
                ProjectArchNode
                => "    Project: {0}".Format( text ),
                ModuleArchNode
                => "     Module: {0}".Format( text ),
                NamespaceArchNode
                => "  Namespace: {0}".Format( text ),
                GroupArchNode
                => "      Group: {0}".Format( text ),
                TypeArchNode
                => "       Type: {0}".Format( text ),
                { } => throw new ArgumentException( "Node is invalid: " + node ),
                null => throw new ArgumentNullException( nameof( node ), "Node is null" ),
            };
        }
    }
    public class HierarchyNodeHighlighter : NodeRenderer {
        public HierarchyNodeHighlighter(NodeRenderer? source = null) : base( source ) {
        }
        public override string Highlight(ArchNode node, string text) {
            return node switch {
                ProjectArchNode
                => "{0}".Format( text ),
                ModuleArchNode module
                => "{0}".Format( text ).Indent( module ),
                NamespaceArchNode @namespace
                => "{0}".Format( text ).Indent( @namespace ),
                GroupArchNode group
                => "{0}".Format( text ).Indent( group ),
                TypeArchNode type
                => "{0}".Format( text ).Indent( type ),
                { } => throw new ArgumentException( "Node is invalid: " + node ),
                null => throw new ArgumentNullException( nameof( node ), "Node is null" ),
            };
        }
    }
    internal static class HierarchyNodeHighlighterHelper {
        public static string Indent(this string text, ModuleArchNode module) {
            return "| - " + text;
        }
        public static string Indent(this string text, NamespaceArchNode @namespace) {
            const string indent1 = "|   | - ";
            const string indent2 = "    | - ";

            return !@namespace.Module.IsLast() switch {
                true => indent1 + text,
                false => indent2 + text,
            };
        }
        public static string Indent(this string text, GroupArchNode group) {
            const string indent1 = "|   |   | - ";
            const string indent2 = "|       | - ";
            const string indent3 = "    |   | - ";
            const string indent4 = "        | - ";

            return (!group.GetModule().IsLast(), !group.Namespace.IsLast()) switch {
                (true, true ) => indent1 + text,
                (true, false ) => indent2 + text,
                (false, true ) => indent3 + text,
                (false, false ) => indent4 + text,
            };
        }
        public static string Indent(this string text, TypeArchNode type) {
            const string indent1 = "|   |   |   ";
            const string indent2 = "|       |   ";
            const string indent3 = "    |   |   ";
            const string indent4 = "        |   ";

            return (!type.GetModule().IsLast(), !type.GetNamespace().IsLast()) switch {
                (true, true ) => indent1 + text,
                (true, false ) => indent2 + text,
                (false, true ) => indent3 + text,
                (false, false ) => indent4 + text,
            };
        }
    }
    public class MarkdownNodeHighlighter : NodeRenderer {
        public MarkdownNodeHighlighter(NodeRenderer? source = null) : base( source ) {
        }
        public override string Highlight(ArchNode node, string text) {
            return node switch {
                ProjectArchNode
                => "**{0}**".Format( text ),
                ModuleArchNode
                => "**{0}**".Format( text ),
                NamespaceArchNode
                => "**{0}**".Format( text ),
                GroupArchNode
                => "**{0}**".Format( text ),
                TypeArchNode
                => "{0}".Format( text ),
                { } => throw new ArgumentException( "Node is invalid: " + node ),
                null => throw new ArgumentNullException( nameof( node ), "Node is null" ),
            };
        }
    }
}
