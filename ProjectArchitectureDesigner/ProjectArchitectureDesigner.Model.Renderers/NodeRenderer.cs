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
        public TextNodeRenderer(NodeRenderer? source) : base( source ) {
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
                => text,
                TypeArchNode
                => text,
                { } => throw new ArgumentException( "Node is invalid: " + node ),
                null => throw new ArgumentNullException( nameof( node ), "Node is null" ),
            };
        }
    }
    public class LeftAlignedTextNodeRenderer : NodeRenderer {
        public LeftAlignedTextNodeRenderer(NodeRenderer? source) : base( source ) {
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
        public RightAlignedTextNodeRenderer(NodeRenderer? source) : base( source ) {
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
    public class MarkdownHighlighter : NodeRenderer {
        public MarkdownHighlighter(NodeRenderer? source) : base( source ) {
        }
        public override string Highlight(ArchNode node, string text) {
            //text = text.Replace( " ", "&nbsp" );
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
