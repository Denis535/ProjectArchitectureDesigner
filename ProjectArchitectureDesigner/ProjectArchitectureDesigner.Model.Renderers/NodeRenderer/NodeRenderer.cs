// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public abstract class NodeRenderer {
        internal NodeRenderer? Source { get; }

        public NodeRenderer(NodeRenderer? source) {
            Source = source;
        }

        // Create
        public static T Create<T>() where T : NodeRenderer {
            return Create_<T>();
        }
        public static T1 Create<T1, T2>() where T1 : NodeRenderer where T2 : NodeRenderer {
            return Create_<T1>( Create_<T2>() );
        }
        public static T1 Create<T1, T2, T3>() where T1 : NodeRenderer where T2 : NodeRenderer where T3 : NodeRenderer {
            return Create_<T1>( Create_<T2>( Create_<T3>() ) );
        }
        public static T1 Create<T1, T2, T3, T4>() where T1 : NodeRenderer where T2 : NodeRenderer where T3 : NodeRenderer where T4 : NodeRenderer {
            return Create_<T1>( Create_<T2>( Create_<T3>( Create_<T4>() ) ) );
        }
        public static T1 Create<T1, T2, T3, T4, T5>() where T1 : NodeRenderer where T2 : NodeRenderer where T3 : NodeRenderer where T4 : NodeRenderer where T5 : NodeRenderer {
            return Create_<T1>( Create_<T2>( Create_<T3>( Create_<T4>( Create_<T5>() ) ) ) );
        }
        private static T Create_<T>(NodeRenderer? renderer = null) where T : NodeRenderer {
            return (T) Activator.CreateInstance( typeof( T ), new[] { renderer } )!;
        }

        // Render
        public string Render(ArchNode node) {
            var text = node.GetName();
            foreach (var renderer in Flatten( this ).Reverse()) {
                text = renderer.Render( node, text );
                text = renderer.Highlight( node, text );
            }
            return text;
        }

        // Render
        protected virtual string Render(ArchNode node, string text) {
            if (node is ProjectArchNode project) return Render( project, text );
            if (node is ModuleArchNode module) return Render( module, text );
            if (node is NamespaceArchNode @namespace) return Render( @namespace, text );
            if (node is GroupArchNode group) return Render( group, text );
            if (node is TypeArchNode type) return Render( type, text );
            throw new ArgumentException( "Node is invalid: " + node?.ToString() ?? "null" );
        }
        protected virtual string Render(ProjectArchNode project, string text) => text;
        protected virtual string Render(ModuleArchNode module, string text) => text;
        protected virtual string Render(NamespaceArchNode @namespace, string text) => text;
        protected virtual string Render(GroupArchNode group, string text) => text;
        protected virtual string Render(TypeArchNode type, string text) => text;

        // Highlight
        protected virtual string Highlight(ArchNode node, string text) {
            if (node is ProjectArchNode project) return Highlight( project, text );
            if (node is ModuleArchNode module) return Highlight( module, text );
            if (node is NamespaceArchNode @namespace) return Highlight( @namespace, text );
            if (node is GroupArchNode group) return Highlight( group, text );
            if (node is TypeArchNode type) return Highlight( type, text );
            throw new ArgumentException( "Node is invalid: " + node?.ToString() ?? "null" );
        }
        protected virtual string Highlight(ProjectArchNode project, string text) => text;
        protected virtual string Highlight(ModuleArchNode module, string text) => text;
        protected virtual string Highlight(NamespaceArchNode @namespace, string text) => text;
        protected virtual string Highlight(GroupArchNode group, string text) => text;
        protected virtual string Highlight(TypeArchNode type, string text) => text;

        // Helpers
        private static IEnumerable<NodeRenderer> Flatten(NodeRenderer? renderer) {
            while (renderer != null) {
                yield return renderer;
                renderer = renderer.Source;
            }
        }
    }
    public class TextRenderer : NodeRenderer {
        public TextRenderer(NodeRenderer? source = null) : base( source ) {
        }
        protected override string Render(ProjectArchNode project, string text)
            => "Project: {0}".Format( text );
        protected override string Render(ModuleArchNode module, string text)
            => "Module: {0}".Format( text );
        protected override string Render(NamespaceArchNode @namespace, string text)
            => "Namespace: {0}".Format( text );
        protected override string Render(GroupArchNode group, string text)
            => "{0}".Format( text );
        protected override string Render(TypeArchNode type, string text)
            => "{0}".Format( text );
    }
    public class LeftAlignedTextRenderer : NodeRenderer {
        public LeftAlignedTextRenderer(NodeRenderer? source = null) : base( source ) {
        }
        protected override string Render(ProjectArchNode project, string text)
            => "Project: ——— {0}".Format( text );
        protected override string Render(ModuleArchNode module, string text)
            => "Module: ———— {0}".Format( text );
        protected override string Render(NamespaceArchNode @namespace, string text)
            => "Namespace: — {0}".Format( text );
        protected override string Render(GroupArchNode group, string text)
            => "Group: ————— {0}".Format( text );
        protected override string Render(TypeArchNode type, string text)
            => "Type: —————— {0}".Format( text );
    }
    public class RightAlignedTextRenderer : NodeRenderer {
        public RightAlignedTextRenderer(NodeRenderer? source = null) : base( source ) {
        }
        protected override string Render(ProjectArchNode project, string text)
            => "    Project: {0}".Format( text );
        protected override string Render(ModuleArchNode module, string text)
            => "     Module: {0}".Format( text );
        protected override string Render(NamespaceArchNode @namespace, string text)
            => "  Namespace: {0}".Format( text );
        protected override string Render(GroupArchNode group, string text)
            => "      Group: {0}".Format( text );
        protected override string Render(TypeArchNode type, string text)
            => "       Type: {0}".Format( text );
    }
}
