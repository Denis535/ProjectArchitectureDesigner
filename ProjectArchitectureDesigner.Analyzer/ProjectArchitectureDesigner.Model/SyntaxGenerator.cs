// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal static class SyntaxGenerator {


        // Project
        public static ClassDeclarationSyntax CreateProjectClass(ClassDeclarationSyntax @class, ProjectInfo project) {
            var builder = new StringBuilder();
            builder.AppendLine();
            builder.Comment( "// Project" );
            builder.Class( "$modifiers class $type {", @class.Modifiers, @class.Identifier );
            {
                builder.Comment( "/// Properties" );
                builder.Property( "public override string Name { get; } = \"$name\";", project.Name );
                builder.Property( "public override ModuleArchNode[] Modules { get; }" );
                builder.Property( project.Modules, "public $type $name { get; }", i => i.Type, i => i.Property );
                builder.Comment( "/// Constructor" );
                builder.Constructor( "public $type() {", project.Type );
                {
                    builder.Statement( "this.Modules = new ModuleArchNode[] {" );
                    builder.Statement( project.Modules, "this.$name = new $type( this ),", i => i.Property, i => i.Type );
                    builder.Statement( "};" );
                }
                builder.Constructor( "}" );
            }
            builder.Class( "}" );
            return SyntaxFactory2.ClassDeclaration( builder.ToString() );
        }
        // Module
        public static ClassDeclarationSyntax CreateModuleClass(ClassDeclarationSyntax @class, ModuleInfo module) {
            var builder = new StringBuilder();
            builder.AppendLine();
            builder.Comment( "// Module" );
            builder.Class( "$modifiers class $type {", @class.Modifiers, @class.Identifier );
            {
                builder.Comment( "/// Properties" );
                builder.Property( "public override string Name { get; } = \"$name\";", module.Name );
                builder.Property( "public override NamespaceArchNode[] Namespaces { get; }" );
                builder.Property( module.Namespaces, "public $type $name { get; }", i => i.Type, i => i.Property );
                builder.Comment( "/// Constructor" );
                builder.Constructor( "public $type(ProjectArchNode project) : base( project ) {", module.Type );
                {
                    builder.Statement( "this.Namespaces = new NamespaceArchNode[] {" );
                    builder.Statement( module.Namespaces, "this.$name = new $type( this ),", i => i.Property, i => i.Type );
                    builder.Statement( "};" );
                }
                builder.Constructor( "}" );
                foreach (var @namespace in module.Namespaces) {
                    builder.AppendNamespaceClass( @namespace );
                }
            }
            builder.Class( "}" );
            return SyntaxFactory2.ClassDeclaration( builder.ToString() );
        }


        // Helpers/Namespace
        private static void AppendNamespaceClass(this StringBuilder builder, NamespaceEntry @namespace) {
            builder.Comment( "// Namespace" );
            builder.Class( "public class $type : NamespaceArchNode {", @namespace.Type );
            {
                builder.Comment( "/// Properties" );
                builder.Property( "public override string Name { get; } = \"$name\";", @namespace.Name );
                builder.Property( "public override GroupArchNode[] Groups { get; }" );
                builder.Property( @namespace.Groups, "public $type $name { get; }", i => i.Type, i => i.Property );
                builder.Comment( "/// Constructor" );
                builder.Constructor( "public $type(ModuleArchNode module) : base( module ) {", @namespace.Type );
                {
                    builder.Statement( "this.Groups = new GroupArchNode[] {" );
                    builder.Statement( @namespace.Groups, "this.$name = new $type( this ),", i => i.Property, i => i.Type );
                    builder.Statement( "};" );
                }
                builder.Constructor( "}" );
                foreach (var group in @namespace.Groups) {
                    builder.AppendGroupClass( group );
                }
            }
            builder.Class( "}" );
        }
        // Helpers/Group
        private static void AppendGroupClass(this StringBuilder builder, GroupEntry group) {
            builder.Comment( "// Group" );
            builder.Class( "public class $type : GroupArchNode {", group.Type );
            {
                builder.Comment( "/// Properties" );
                builder.Property( "public override string Name { get; } = \"$name\";", group.Name );
                builder.Property( "public override TypeArchNode[] Types { get; }" );
                builder.Comment( "/// Constructor" );
                builder.Constructor( "public $type(NamespaceArchNode @namespace) : base( @namespace ) {", group.Type );
                {
                    builder.Statement( "this.Types = new TypeArchNode[] {" );
                    builder.Statement( group.Types, "new TypeArchNode( typeof($type), this ),", i => i.Type );
                    builder.Statement( "};" );
                }
                builder.Constructor( "}" );
            }
            builder.Class( "}" );
        }


    }
}