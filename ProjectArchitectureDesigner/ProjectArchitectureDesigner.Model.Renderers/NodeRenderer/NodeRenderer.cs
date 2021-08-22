// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public abstract class NodeRenderer {
        internal NodeRenderer? Source { get; }

        public NodeRenderer(NodeRenderer? source) {
            Source = source;
        }

        // Create
        public static TNodeRenderer Create<TNodeRenderer>()
            where TNodeRenderer : NodeRenderer {
            return Create<TNodeRenderer>( null );
        }
        public static TNodeRenderer1 Create<TNodeRenderer1, TNodeRenderer2>()
            where TNodeRenderer1 : NodeRenderer
            where TNodeRenderer2 : NodeRenderer {
            return Create<TNodeRenderer1>( Create<TNodeRenderer2>( null ) );
        }
        public static TNodeRenderer1 Create<TNodeRenderer1, TNodeRenderer2, TNodeRenderer3>()
            where TNodeRenderer1 : NodeRenderer
            where TNodeRenderer2 : NodeRenderer
            where TNodeRenderer3 : NodeRenderer {
            return Create<TNodeRenderer1>( Create<TNodeRenderer2>( Create<TNodeRenderer3>( null ) ) );
        }
        public static TNodeRenderer1 Create<TNodeRenderer1, TNodeRenderer2, TNodeRenderer3, TNodeRenderer4>()
            where TNodeRenderer1 : NodeRenderer
            where TNodeRenderer2 : NodeRenderer
            where TNodeRenderer3 : NodeRenderer
            where TNodeRenderer4 : NodeRenderer {
            return Create<TNodeRenderer1>( Create<TNodeRenderer2>( Create<TNodeRenderer3>( Create<TNodeRenderer4>( null ) ) ) );
        }
        public static TNodeRenderer1 Create<TNodeRenderer1, TNodeRenderer2, TNodeRenderer3, TNodeRenderer4, TNodeRenderer5>()
            where TNodeRenderer1 : NodeRenderer
            where TNodeRenderer2 : NodeRenderer
            where TNodeRenderer3 : NodeRenderer
            where TNodeRenderer4 : NodeRenderer
            where TNodeRenderer5 : NodeRenderer {
            return Create<TNodeRenderer1>( Create<TNodeRenderer2>( Create<TNodeRenderer3>( Create<TNodeRenderer4>( Create<TNodeRenderer5>( null ) ) ) ) );
        }
        private static TNodeRenderer Create<TNodeRenderer>(NodeRenderer? renderer) where TNodeRenderer : NodeRenderer {
            return (TNodeRenderer) Activator.CreateInstance( typeof( TNodeRenderer ), new NodeRenderer?[] { renderer } )!;
        }

        public virtual string Render(ArchNode node, string text) {
            if (node is ProjectArchNode project) return Render( project, text );
            if (node is ModuleArchNode module) return Render( module, text );
            if (node is NamespaceArchNode @namespace) return Render( @namespace, text );
            if (node is GroupArchNode group) return Render( group, text );
            if (node is TypeArchNode type) return Render( type, text );
            throw new ArgumentException( "Node is invalid: " + node?.ToString() ?? "null" );
        }
        public virtual string Render(ProjectArchNode project, string text) => text;
        public virtual string Render(ModuleArchNode module, string text) => text;
        public virtual string Render(NamespaceArchNode @namespace, string text) => text;
        public virtual string Render(GroupArchNode group, string text) => text;
        public virtual string Render(TypeArchNode type, string text) => text;

        public virtual string Highlight(ArchNode node, string text) {
            if (node is ProjectArchNode project) return Highlight( project, text );
            if (node is ModuleArchNode module) return Highlight( module, text );
            if (node is NamespaceArchNode @namespace) return Highlight( @namespace, text );
            if (node is GroupArchNode group) return Highlight( group, text );
            if (node is TypeArchNode type) return Highlight( type, text );
            throw new ArgumentException( "Node is invalid: " + node?.ToString() ?? "null" );
        }
        public virtual string Highlight(ProjectArchNode project, string text) => text;
        public virtual string Highlight(ModuleArchNode module, string text) => text;
        public virtual string Highlight(NamespaceArchNode @namespace, string text) => text;
        public virtual string Highlight(GroupArchNode group, string text) => text;
        public virtual string Highlight(TypeArchNode type, string text) => text;
    }
    public class TextRenderer : NodeRenderer {
        public TextRenderer(NodeRenderer? source = null) : base( source ) {
        }
        public override string Render(ProjectArchNode project, string text)
            => "Project: {0}".Format( text );
        public override string Render(ModuleArchNode module, string text)
            => "Module: {0}".Format( text );
        public override string Render(NamespaceArchNode @namespace, string text)
            => "Namespace: {0}".Format( text );
        public override string Render(GroupArchNode group, string text)
            => "{0}".Format( text );
        public override string Render(TypeArchNode type, string text)
            => "{0}".Format( text );
    }
    public class LeftAlignedTextRenderer : NodeRenderer {
        public LeftAlignedTextRenderer(NodeRenderer? source = null) : base( source ) {
        }
        public override string Render(ProjectArchNode project, string text)
            => "Project: ——— {0}".Format( text );
        public override string Render(ModuleArchNode module, string text)
            => "Module: ———— {0}".Format( text );
        public override string Render(NamespaceArchNode @namespace, string text)
            => "Namespace: — {0}".Format( text );
        public override string Render(GroupArchNode group, string text)
            => "Group: ————— {0}".Format( text );
        public override string Render(TypeArchNode type, string text)
            => "Type: —————— {0}".Format( text );
    }
    public class RightAlignedTextRenderer : NodeRenderer {
        public RightAlignedTextRenderer(NodeRenderer? source = null) : base( source ) {
        }
        public override string Render(ProjectArchNode project, string text)
            => "    Project: {0}".Format( text );
        public override string Render(ModuleArchNode module, string text)
            => "     Module: {0}".Format( text );
        public override string Render(NamespaceArchNode @namespace, string text)
            => "  Namespace: {0}".Format( text );
        public override string Render(GroupArchNode group, string text)
            => "      Group: {0}".Format( text );
        public override string Render(TypeArchNode type, string text)
            => "       Type: {0}".Format( text );
    }
}
