// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

#pragma warning disable RS2008 // Enable analyzer release tracking
namespace ProjectArchitectureDesigner.Model {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    [Generator]
    public class SourceGenerator : ISourceGenerator {
        private class SyntaxReceiver : ISyntaxReceiver {
            public IList<CompilationUnitSyntax> Units { get; } = new List<CompilationUnitSyntax>();
            public void OnVisitSyntaxNode(SyntaxNode syntax) {
                if (syntax is CompilationUnitSyntax unit) Units.Add( unit );
            }
        }

        private static readonly DiagnosticDescriptor ErrorDiagnosticDescriptor = new DiagnosticDescriptor(
            "ProjectArchitecture",
            "Error",
            "{0}",
            "Error",
            DiagnosticSeverity.Error,
            true );


        public void Initialize(GeneratorInitializationContext context) {
            context.RegisterForSyntaxNotifications( () => new SyntaxReceiver() );
        }


        public void Execute(GeneratorExecutionContext context) {
#if DEBUG
            //if (!Debugger.IsAttached) Debugger.Launch();
#endif
            var receiver = (SyntaxReceiver) context.SyntaxReceiver!;
            var compilation = context.Compilation;
            foreach (var unit in receiver.Units) {
                if (unit.SyntaxTree.FilePath.Contains( ".nuget" )) continue;
                if (unit.SyntaxTree.FilePath.Contains( "\\obj\\Debug\\" )) continue;
                if (unit.SyntaxTree.FilePath.Contains( "\\obj\\Release\\" )) continue;

                try {
                    Execute( context, unit );
                } catch (Exception ex) {
                    context.ReportDiagnostic( Diagnostic.Create( ErrorDiagnosticDescriptor, null, ex.Message ) );
                }
            }
        }
        private static void Execute(GeneratorExecutionContext context, CompilationUnitSyntax unit) {
            var name = GetGeneratedName( unit );
            var source = Generate( unit );
            if (source.IsNotEmpty()) {
                Debug.WriteLine( "Generated source: " + name );
                Debug.WriteLine( source );
                context.AddSource( name, source );
            }
        }


        // Helpers/Generate
        private static string Generate(CompilationUnitSyntax unit) {
            var builder = new SyntaxBuilder();
            CompilationUnit( builder, unit );
            return builder.ToString();
        }
        private static void CompilationUnit(SyntaxBuilder builder, CompilationUnitSyntax unit) {
            if (!GetTypes( unit ).Any( IsProjectOrModule )) return;
            builder.UsingDirective( unit.Usings );
            builder.ExternAliasDirective( unit.Externs );
            foreach (var child in unit.Members) {
                if (child is NamespaceDeclarationSyntax @namespace) {
                    Namespace( builder, @namespace );
                }
                if (child is ClassDeclarationSyntax @class) {
                    Class( builder, @class );
                }
            }
        }
        private static void Namespace(SyntaxBuilder builder, NamespaceDeclarationSyntax @namespace) {
            if (!GetTypes( @namespace ).Any( IsProjectOrModule )) return;
            builder.Namespace( "namespace $name {", @namespace.Name );
            {
                builder.UsingDirective( @namespace.Usings );
                builder.ExternAliasDirective( @namespace.Externs );
                foreach (var child in @namespace.Members) {
                    if (child is NamespaceDeclarationSyntax namespace_) {
                        Namespace( builder, namespace_ );
                    }
                    if (child is ClassDeclarationSyntax @class) {
                        Class( builder, @class );
                    }
                }
            }
            builder.Namespace( "}" );
        }
        private static void Class(SyntaxBuilder builder, ClassDeclarationSyntax @class) {
            if (@class.IsPartial() && @class.IsProject()) {
                builder.ProjectClass( @class, @class.GetProjectInfo() );
            }
            if (@class.IsPartial() && @class.IsModule()) {
                builder.ModuleClass( @class, @class.GetModuleInfo() );
            }
        }
        // Helpers/Syntax
        private static IEnumerable<BaseTypeDeclarationSyntax> GetTypes(CompilationUnitSyntax unit) {
            return unit.Members.SelectMany( GetTypes );
        }
        private static IEnumerable<BaseTypeDeclarationSyntax> GetTypes(MemberDeclarationSyntax member) {
            if (member is NamespaceDeclarationSyntax @namespace) {
                return @namespace.Members.SelectMany( GetTypes );
            }
            if (member is BaseTypeDeclarationSyntax type) {
                return type.ToEnumerable();
            }
            return Enumerable.Empty<BaseTypeDeclarationSyntax>();
        }
        private static bool IsProjectOrModule(BaseTypeDeclarationSyntax type) {
            return type is ClassDeclarationSyntax @class && @class.IsPartial() && (@class.IsProject() || @class.IsModule());
        }
        // Helpers/Syntax
        private static string GetGeneratedName(CompilationUnitSyntax unit) {
            return Path.GetFileNameWithoutExtension( unit.SyntaxTree.FilePath ) + ".Generated.cs";
        }


    }
}
