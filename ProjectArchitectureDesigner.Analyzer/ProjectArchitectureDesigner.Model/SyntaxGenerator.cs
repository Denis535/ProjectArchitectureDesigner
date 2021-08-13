﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal static class SyntaxGenerator {


        // Project
        public static void ProjectClass(this SyntaxBuilder builder, ClassDeclarationSyntax @class, ProjectInfo project) {
            builder.EndOfLine();
            builder.Comment( "// Project" );
            builder.Class( "$modifiers class $identifier : ProjectArchNode {", @class.Modifiers, @class.Identifier );
            {
                builder.Comment( "/// Properties" );
                builder.Property( "public override string Name => \"$name\";", project.Name );
                builder.Property( project.Modules, "public $type $property { get; }", i => (i.Type, i.Property) );
                builder.Comment( "/// Constructor" );
                builder.Constructor( "public $type() {", project.Type );
                {
                    builder.Statement( "base.Modules = new ModuleArchNode[] {" );
                    builder.Statement( project.Modules, "this.$property = new $type( this ),", i => (i.Property, i.Type) );
                    builder.Statement( "};" );
                }
                builder.Constructor( "}" );
            }
            builder.Class( "}" );
        }
        // Module
        public static void ModuleClass(this SyntaxBuilder builder, ClassDeclarationSyntax @class, ModuleInfo module) {
            builder.EndOfLine();
            builder.Comment( "// Module" );
            builder.Class( "$modifiers class $identifier : ModuleArchNode {", @class.Modifiers, @class.Identifier );
            {
                builder.Comment( "/// Properties" );
                builder.Property( "public override string Name => \"$name\";", module.Name );
                builder.Property( module.Namespaces, "public $type $property { get; }", i => (i.Type, i.Property) );
                builder.Comment( "/// Constructor" );
                builder.Constructor( "public $type(ProjectArchNode project) {", module.Type );
                {
                    builder.Statement( "base.Project = project;" );
                    builder.Statement( "base.Namespaces = new NamespaceArchNode[] {" );
                    builder.Statement( module.Namespaces, "this.$property = new $type( this ),", i => (i.Property, i.Type) );
                    builder.Statement( "};" );
                }
                builder.Constructor( "}" );
                foreach (var @namespace in module.Namespaces) {
                    builder.NamespaceClass( @namespace );
                }
            }
            builder.Class( "}" );
        }


        // Helpers/Namespace
        private static void NamespaceClass(this SyntaxBuilder builder, NamespaceEntry @namespace) {
            builder.Comment( "// Namespace" );
            builder.Class( "public class $identifier : NamespaceArchNode {", @namespace.Type );
            {
                builder.Comment( "/// Properties" );
                builder.Property( "public override string Name => \"$name\";", @namespace.Name );
                builder.Property( @namespace.Groups, "public $type $property { get; }", i => (i.Type, i.Property) );
                builder.Comment( "/// Constructor" );
                builder.Constructor( "public $type(ModuleArchNode module) {", @namespace.Type );
                {
                    builder.Statement( "base.Module = module;" );
                    builder.Statement( "base.Groups = new GroupArchNode[] {" );
                    builder.Statement( @namespace.Groups, "this.$property = new $type( this ),", i => (i.Property, i.Type) );
                    builder.Statement( "};" );
                }
                builder.Constructor( "}" );
                foreach (var group in @namespace.Groups) {
                    builder.GroupClass( group );
                }
            }
            builder.Class( "}" );
        }
        // Helpers/Group
        private static void GroupClass(this SyntaxBuilder builder, GroupEntry group) {
            builder.Comment( "// Group" );
            builder.Class( "public class $identifier : GroupArchNode {", group.Type );
            {
                builder.Comment( "/// Properties" );
                builder.Property( "public override string Name => \"$name\";", group.Name );
                builder.Comment( "/// Constructor" );
                builder.Constructor( "public $type(NamespaceArchNode @namespace) {", group.Type );
                {
                    builder.Statement( "base.Namespace = @namespace;" );
                    builder.Statement( "base.Types = new TypeArchNode[] {" );
                    builder.Statement( group.Types, "new TypeArchNode( typeof($type), this ),", i => i.Type );
                    builder.Statement( "};" );
                }
                builder.Constructor( "}" );
            }
            builder.Class( "}" );
        }


    }
}