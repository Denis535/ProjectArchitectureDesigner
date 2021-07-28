// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ProjectArchitectureDesigner.Model;

    public interface INodeRenderer {
        public static TextNodeRenderer TextRenderer => new TextNodeRenderer();
        public static LeftAlignedTextNodeRenderer LeftAlignedTextRenderer => new LeftAlignedTextNodeRenderer();
        public static RightAlignedTextNodeRenderer RightAlignedTextRenderer => new RightAlignedTextNodeRenderer();
        string Render(ArchNode node);
        public static DelegateNodeRenderer FromDelegate(Func<ArchNode, string> @delegate) {
            return new DelegateNodeRenderer( @delegate );
        }
    }
    public class DelegateNodeRenderer : INodeRenderer {
        private readonly Func<ArchNode, string> @delegate;
        public DelegateNodeRenderer(Func<ArchNode, string> @delegate) {
            this.@delegate = @delegate;
        }
        public string Render(ArchNode node) {
            return @delegate( node );
        }
    }
    public class TextNodeRenderer : INodeRenderer {
        public string Render(ArchNode node) {
            return node switch {
                ProjectArchNode
                => "Project: {0}".Format( node.Name ),
                ModuleArchNode
                => "Module: {0}".Format( node.Name ),
                NamespaceArchNode
                => "Namespace: {0}".Format( node.Name ),
                GroupArchNode
                => node.Name,
                TypeArchNode
                => node.Name,
                { } => throw new ArgumentException( "Node is invalid: " + node.GetType() ),
                null => throw new ArgumentNullException( nameof( node ), "Node is null" ),
            };
        }
    }
    public class LeftAlignedTextNodeRenderer : INodeRenderer {
        public string Render(ArchNode node) {
            return node switch {
                ProjectArchNode
                => "Project: ——— {0}".Format( node.Name ),
                ModuleArchNode
                => "Module: ———— {0}".Format( node.Name ),
                NamespaceArchNode
                => "Namespace: — {0}".Format( node.Name ),
                GroupArchNode
                => "Group: ————— {0}".Format( node.Name ),
                TypeArchNode
                => "Type: —————— {0}".Format( node.Name ),
                { } => throw new ArgumentException( "Node is invalid: " + node.GetType() ),
                null => throw new ArgumentNullException( nameof( node ), "Node is null" ),
            };
        }
    }
    public class RightAlignedTextNodeRenderer : INodeRenderer {
        public string Render(ArchNode node) {
            return node switch {
                ProjectArchNode
                => "    Project: {0}".Format( node.Name ),
                ModuleArchNode
                => "     Module: {0}".Format( node.Name ),
                NamespaceArchNode
                => "  Namespace: {0}".Format( node.Name ),
                GroupArchNode
                => "      Group: {0}".Format( node.Name ),
                TypeArchNode
                => "       Type: {0}".Format( node.Name ),
                { } => throw new ArgumentException( "Node is invalid: " + node.GetType() ),
                null => throw new ArgumentNullException( nameof( node ), "Node is null" ),
            };
        }
    }
}
