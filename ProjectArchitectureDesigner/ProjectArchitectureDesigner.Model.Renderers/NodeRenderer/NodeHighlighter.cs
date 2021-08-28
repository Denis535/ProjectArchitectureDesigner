// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class HierarchyHighlighter : NodeRenderer {
        public HierarchyHighlighter(NodeRenderer? source = null) : base( source ) {
        }
        public override string Render(ProjectArchNode project, string text)
            => "{0}".Format( text );
        public override string Render(ModuleArchNode module, string text)
            => "{0}".Format( text ).Indent( module );
        public override string Render(NamespaceArchNode @namespace, string text)
            => "{0}".Format( text ).Indent( @namespace );
        public override string Render(GroupArchNode group, string text)
            => "{0}".Format( text ).Indent( group );
        public override string Render(TypeArchNode type, string text)
            => "{0}".Format( text ).Indent( type );
    }
    public class MarkdownHighlighter : NodeRenderer {
        public MarkdownHighlighter(NodeRenderer? source = null) : base( source ) {
        }
        public override string Render(ProjectArchNode project, string text)
            => "**{0}**".Format( text );
        public override string Render(ModuleArchNode module, string text)
            => "**{0}**".Format( text );
        public override string Render(NamespaceArchNode @namespace, string text)
            => "**{0}**".Format( text );
        public override string Render(GroupArchNode group, string text)
            => "**{0}**".Format( text );
        public override string Render(TypeArchNode type, string text)
            => "{0}".Format( text );
    }
    // Helpers
    internal static class HierarchyHighlighterHelper {
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
}
