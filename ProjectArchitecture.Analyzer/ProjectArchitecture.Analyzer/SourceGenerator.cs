// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

#pragma warning disable RS2008 // Enable analyzer release tracking
namespace ProjectArchitecture.Analyzer {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    [Generator]
    public class SourceGenerator : ISourceGenerator {

        private static readonly DiagnosticDescriptor ErrorDiagnosticDescriptor = new DiagnosticDescriptor(
            "SourceGenerator",
            "SourceGenerator",
            "Error: {0}",
            "Error",
            DiagnosticSeverity.Error,
            true );


        public void Initialize(GeneratorInitializationContext context) {
        }


        public void Execute(GeneratorExecutionContext context) {
#if DEBUG
            Debugger.Launch();
#endif
            var compilation = context.Compilation;
            var cancellationToken = context.CancellationToken;

            foreach (var tree in compilation.SyntaxTrees) {
                if (tree.FilePath.Contains( ".nuget" )) continue;
                if (tree.FilePath.Contains( "\\obj\\Debug\\" )) continue;
                if (tree.FilePath.Contains( "\\obj\\Release\\" )) continue;

                try {
                    Execute( context, tree );
                } catch (Exception ex) {
                    context.ReportDiagnostic( Diagnostic.Create( ErrorDiagnosticDescriptor, null, ex.Message ) );
                }
            }
        }
        private static void Execute(GeneratorExecutionContext context, SyntaxTree tree) {
            var name = GetGeneratedSourceName( tree );
            var source = GetGeneratedSource( tree );
            if (source != null) {
                Debug.WriteLine( "Generated source: " + name );
                Debug.WriteLine( source );
                context.AddSource( name, source );
            }
        }


        // Helpers
        private static string GetGeneratedSourceName(SyntaxTree tree) {
            return Path.GetFileNameWithoutExtension( tree.FilePath ) + $".Generated.{Guid.NewGuid()}.cs";
        }
        private static string? GetGeneratedSource(SyntaxTree tree) {
            return CreateCompilationUnit( (CompilationUnitSyntax) tree.GetRoot( default ) )?.NormalizeWhitespace().ToString();
        }
        // Helpers/Generation/CompilationUnit
        private static CompilationUnitSyntax? CreateCompilationUnit(CompilationUnitSyntax unit) {
            var members = unit.Members.Select( CreateMemberDeclaration ).OfType<MemberDeclarationSyntax>().ToArray();
            if (!members.Any()) return null;
            return SyntaxFactoryUtils.CompilationUnit( unit ).AddMembers( members );
        }
        // Helpers/Generation/Member
        private static MemberDeclarationSyntax? CreateMemberDeclaration(MemberDeclarationSyntax member) {
            if (member is NamespaceDeclarationSyntax @namespace) {
                return CreateNamespaceDeclaration( @namespace );
            }
            if (member is ClassDeclarationSyntax @class) {
                if (@class.IsPartial() && @class.IsChildOf( "ProjectNode" )) {
                    var modules = SyntaxAnalyzer.GetProjectData( @class ).ToArray();
                    return SyntaxGenerator.CreateClassDeclaration_Project( @class, modules );
                }
                if (@class.IsPartial() && @class.IsChildOf( "ModuleNode" )) {
                    var namespaces = SyntaxAnalyzer.GetModuleData( @class ).ToArray();
                    return SyntaxGenerator.CreateClassDeclaration_Module( @class, namespaces );
                }
            }
            return null;
        }
        // Helpers/Generation/Member/Namespace
        private static NamespaceDeclarationSyntax? CreateNamespaceDeclaration(NamespaceDeclarationSyntax @namespace) {
            var members = @namespace.Members.Select( CreateMemberDeclaration ).OfType<MemberDeclarationSyntax>().ToArray();
            if (!members.Any()) return null;
            return SyntaxFactoryUtils.NamespaceDeclaration( @namespace ).AddMembers( members );
        }


    }
}
