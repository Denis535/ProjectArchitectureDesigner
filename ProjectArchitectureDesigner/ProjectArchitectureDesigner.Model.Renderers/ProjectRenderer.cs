// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ProjectArchitectureDesigner.Model;

    public abstract class ProjectRenderer {

        private NodeRenderer Renderer { get; }


        public ProjectRenderer(NodeRenderer renderer) {
            Renderer = renderer;
        }


        // Create
        public static TProjectRenderer Create<TProjectRenderer, TNodeRenderer>()
            where TProjectRenderer : ProjectRenderer
            where TNodeRenderer : NodeRenderer {
            var renderer = CreateNodeRenderer<TNodeRenderer>();
            return CreateProjectRenderer<TProjectRenderer>( renderer );
        }
        public static TProjectRenderer Create<TProjectRenderer, TNodeRenderer1, TNodeRenderer2>()
            where TProjectRenderer : ProjectRenderer
            where TNodeRenderer1 : NodeRenderer
            where TNodeRenderer2 : NodeRenderer {
            var renderer = CreateNodeRenderer<TNodeRenderer1>( CreateNodeRenderer<TNodeRenderer2>() );
            return CreateProjectRenderer<TProjectRenderer>( renderer );
        }
        public static TProjectRenderer Create<TProjectRenderer, TNodeRenderer1, TNodeRenderer2, TNodeRenderer3>()
            where TProjectRenderer : ProjectRenderer
            where TNodeRenderer1 : NodeRenderer
            where TNodeRenderer2 : NodeRenderer
            where TNodeRenderer3 : NodeRenderer {
            var renderer = CreateNodeRenderer<TNodeRenderer1>( CreateNodeRenderer<TNodeRenderer2>( CreateNodeRenderer<TNodeRenderer3>() ) );
            return CreateProjectRenderer<TProjectRenderer>( renderer );
        }
        public static TProjectRenderer Create<TProjectRenderer, TNodeRenderer1, TNodeRenderer2, TNodeRenderer3, TNodeRenderer4>()
            where TProjectRenderer : ProjectRenderer
            where TNodeRenderer1 : NodeRenderer
            where TNodeRenderer2 : NodeRenderer
            where TNodeRenderer3 : NodeRenderer
            where TNodeRenderer4 : NodeRenderer {
            var renderer = CreateNodeRenderer<TNodeRenderer1>( CreateNodeRenderer<TNodeRenderer2>( CreateNodeRenderer<TNodeRenderer3>( CreateNodeRenderer<TNodeRenderer4>() ) ) );
            return CreateProjectRenderer<TProjectRenderer>( renderer );
        }
        public static TProjectRenderer Create<TProjectRenderer, TNodeRenderer1, TNodeRenderer2, TNodeRenderer3, TNodeRenderer4, TNodeRenderer5>()
            where TProjectRenderer : ProjectRenderer
            where TNodeRenderer1 : NodeRenderer
            where TNodeRenderer2 : NodeRenderer
            where TNodeRenderer3 : NodeRenderer
            where TNodeRenderer4 : NodeRenderer
            where TNodeRenderer5 : NodeRenderer {
            var renderer = CreateNodeRenderer<TNodeRenderer1>( CreateNodeRenderer<TNodeRenderer2>( CreateNodeRenderer<TNodeRenderer3>( CreateNodeRenderer<TNodeRenderer4>( CreateNodeRenderer<TNodeRenderer5>() ) ) ) );
            return CreateProjectRenderer<TProjectRenderer>( renderer );
        }
        private static TProjectRenderer CreateProjectRenderer<TProjectRenderer>(NodeRenderer renderer) where TProjectRenderer : ProjectRenderer {
            return (TProjectRenderer) Activator.CreateInstance( typeof( TProjectRenderer ), new NodeRenderer[] { renderer } )!;
        }
        private static TNodeRenderer CreateNodeRenderer<TNodeRenderer>(NodeRenderer? renderer = null) where TNodeRenderer : NodeRenderer {
            return (TNodeRenderer) Activator.CreateInstance( typeof( TNodeRenderer ), new NodeRenderer?[] { renderer } )!;
        }


        // Render
        public abstract ProjectRenderer Render(ProjectArchNode project);


        // AppendHierarchy
        protected virtual void AppendHierarchy(ProjectArchNode project) {
            AppendNode( project );
            foreach (var module in project.Modules) {
                AppendHierarchy( module );
            }
        }
        protected virtual void AppendHierarchy(ModuleArchNode module) {
            AppendNode( module );
            foreach (var @namespace in module.Namespaces) {
                AppendHierarchy( @namespace );
            }
        }
        protected virtual void AppendHierarchy(NamespaceArchNode @namespace) {
            AppendNode( @namespace );
            foreach (var group in @namespace.Groups) {
                AppendHierarchy( group );
            }
        }
        protected virtual void AppendHierarchy(GroupArchNode group) {
            AppendNode( group );
            foreach (var type in group.Types) {
                AppendNode( type );
            }
        }


        // AppendNode
        protected virtual void AppendNode(ArchNode node) {
            AppendNode( node, GetStringWithHighlight( node ) );
        }
        protected virtual void AppendNode(ArchNode node, string text) {
            if (node is ProjectArchNode project) AppendNode( project, text );
            if (node is ModuleArchNode module) AppendNode( module, text );
            if (node is NamespaceArchNode @namespace) AppendNode( @namespace, text );
            if (node is GroupArchNode group) AppendNode( group, text );
            if (node is TypeArchNode type) AppendNode( type, text );
        }
        protected virtual void AppendNode(ProjectArchNode project, string text) {
        }
        protected virtual void AppendNode(ModuleArchNode module, string text) {
        }
        protected virtual void AppendNode(NamespaceArchNode @namespace, string text) {
        }
        protected virtual void AppendNode(GroupArchNode group, string text) {
        }
        protected virtual void AppendNode(TypeArchNode type, string text) {
        }


        // GetString
        protected virtual string GetString(ArchNode node) {
            return Render( Renderer, node, node.GetName() );
        }
        protected virtual string GetStringWithHighlight(ArchNode node) {
            return RenderAndHighlight( Renderer, node, node.GetName() );
        }


        // Helpers/Render
        private static string Render(NodeRenderer renderer, ArchNode node, string text) {
            foreach (var renderer_ in Flatten( renderer ).Reverse()) {
                text = renderer_.Render( node, text );
            }
            return text;
        }
        private static string RenderAndHighlight(NodeRenderer renderer, ArchNode node, string text) {
            foreach (var renderer_ in Flatten( renderer ).Reverse()) {
                text = renderer_.Render( node, text );
                text = renderer_.Highlight( node, text );
            }
            return text;
        }
        private static IEnumerable<NodeRenderer> Flatten(NodeRenderer? renderer) {
            while (renderer != null) {
                yield return renderer;
                renderer = renderer.Source;
            }
        }


    }
}
