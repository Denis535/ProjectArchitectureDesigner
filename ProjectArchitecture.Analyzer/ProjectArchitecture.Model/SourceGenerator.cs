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
            //Debugger.Launch();
#endif
            var receiver = (SyntaxReceiver) context.SyntaxReceiver!;
            var compilation = context.Compilation;
            foreach (var unit in receiver.Units) {
                var model = new Lazy<SemanticModel>( () => compilation.GetSemanticModel( unit.SyntaxTree ) );
                if (unit.SyntaxTree.FilePath.Contains( ".nuget" )) continue;
                if (unit.SyntaxTree.FilePath.Contains( "\\obj\\Debug\\" )) continue;
                if (unit.SyntaxTree.FilePath.Contains( "\\obj\\Release\\" )) continue;

                try {
                    Execute( context, unit, model );
                } catch (Exception ex) {
                    context.ReportDiagnostic( Diagnostic.Create( ErrorDiagnosticDescriptor, null, ex.Message ) );
                }
            }
        }
        private static void Execute(GeneratorExecutionContext context, CompilationUnitSyntax unit, Lazy<SemanticModel> model) {
            var name = GetGeneratedSourceName( unit );
            var source = GetGeneratedSource( unit, model );
            if (source != null) {
                Debug.WriteLine( "Generated source: " + name );
                Debug.WriteLine( source );
                context.AddSource( name, source );
            }
        }


        // Helpers
        private static string GetGeneratedSourceName(CompilationUnitSyntax unit) {
            return Path.GetFileNameWithoutExtension( unit.SyntaxTree.FilePath ) + ".Generated.cs";
        }
        private static string? GetGeneratedSource(CompilationUnitSyntax unit, Lazy<SemanticModel> model) {
            return CreateCompilationUnit( unit, model )?.Format().ToFullString();
        }
        // Helpers/CreateSyntax
        private static CompilationUnitSyntax? CreateCompilationUnit(CompilationUnitSyntax unit, Lazy<SemanticModel> model) {
            var members = unit.Members.Select( i => CreateMemberDeclaration( i, model ) ).OfType<MemberDeclarationSyntax>().ToArray();
            if (!members.Any()) return null;
            return SyntaxFactoryUtils.CompilationUnit( unit ).AddMembers( members );
        }
        private static MemberDeclarationSyntax? CreateMemberDeclaration(MemberDeclarationSyntax member, Lazy<SemanticModel> model) {
            if (member is NamespaceDeclarationSyntax @namespace) return CreateNamespaceDeclaration( @namespace, model );
            if (member is ClassDeclarationSyntax @class) return CreateClassDeclaration( @class, model );
            return null;
        }
        private static NamespaceDeclarationSyntax? CreateNamespaceDeclaration(NamespaceDeclarationSyntax @namespace, Lazy<SemanticModel> model) {
            var members = @namespace.Members.Select( i => CreateMemberDeclaration( i, model ) ).OfType<MemberDeclarationSyntax>().ToArray();
            if (!members.Any()) return null;
            return SyntaxFactoryUtils.NamespaceDeclaration( @namespace ).AddMembers( members );
        }
        private static ClassDeclarationSyntax? CreateClassDeclaration(ClassDeclarationSyntax @class, Lazy<SemanticModel> model) {
            if (@class.IsPartial() && SyntaxAnalyzer.IsProject( @class )) {
                var project = SyntaxAnalyzer.GetProjectInfo( @class, model.Value );
                return SyntaxGenerator.CreateClassDeclaration_Project( @class, project );
            }
            if (@class.IsPartial() && SyntaxAnalyzer.IsModule( @class )) {
                var module = SyntaxAnalyzer.GetModuleInfo( @class, model.Value );
                return SyntaxGenerator.CreateClassDeclaration_Module( @class, module );
            }
            return null;
        }


    }
}
