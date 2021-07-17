// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

#pragma warning disable RS2008 // Enable analyzer release tracking
namespace ProjectArchitecture.Model {
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
                //var model = new Lazy<SemanticModel>( () => compilation.GetSemanticModel( unit.SyntaxTree ) );
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
            var unit_generated = Generate( unit );
            if (unit_generated != null) {
                var name = GetGeneratedName( unit );
                var source = unit_generated.Format().ToFullString();
                Debug.WriteLine( "Generated source: " + name );
                Debug.WriteLine( source );
                context.AddSource( name, source );
            }
        }


        // Helpers
        private static CompilationUnitSyntax? Generate(CompilationUnitSyntax unit) {
            var members_generated = Generate( unit.Members ).ToArray();
            if (members_generated.Any()) {
                return SyntaxFactoryUtils.CompilationUnit( unit ).AddMembers( members_generated );
            }
            return null;
        }
        private static IEnumerable<MemberDeclarationSyntax> Generate(IEnumerable<MemberDeclarationSyntax> members) {
            foreach (var member in members) {
                if (member is NamespaceDeclarationSyntax @namespace) {
                    var members_generated = Generate( @namespace.Members ).ToArray();
                    if (members_generated.Any()) {
                        yield return SyntaxFactoryUtils.NamespaceDeclaration( @namespace ).AddMembers( members_generated );
                    }
                } else
                if (member is ClassDeclarationSyntax @class) {
                    if (@class.IsPartial() && @class.IsProject()) {
                        yield return SyntaxGenerator.CreateClassDeclaration_Project( @class, @class.GetProjectInfo() );
                    }
                    if (@class.IsPartial() && @class.IsModule()) {
                        yield return SyntaxGenerator.CreateClassDeclaration_Module( @class, @class.GetModuleInfo() );
                    }
                }
            }
        }
        private static string GetGeneratedName(CompilationUnitSyntax unit) {
            return Path.GetFileNameWithoutExtension( unit.SyntaxTree.FilePath ) + ".Generated.cs";
        }


    }
}
