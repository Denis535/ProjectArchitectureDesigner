// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

#pragma warning disable RS2008 // Enable analyzer release tracking
namespace ProjectArchitecture {
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

        private static readonly DiagnosticDescriptor DiagnosticDescriptor = new DiagnosticDescriptor(
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
            //Debugger.Launch();
#endif
            var compilation = context.Compilation;
            var trees = compilation.SyntaxTrees;
            var cancellationToken = context.CancellationToken;

            foreach (var tree in trees) {
                if (tree.FilePath.Contains( ".nuget" )) continue;
                if (tree.FilePath.Contains( "\\obj\\Debug\\" )) continue;
                if (tree.FilePath.Contains( "\\obj\\Release\\" )) continue;

                try {
                    var root = (CompilationUnitSyntax) tree.GetRoot( cancellationToken );
                    var source = CreateCompilationUnit( root );
                    if (source != null) {
                        var name = GetSourceName( tree );
                        var content = source.NormalizeWhitespace().ToFullString();
#if DEBUG
                        Trace.WriteLine( "Generated source: " + name );
                        Trace.WriteLine( content );
#endif
                        context.AddSource( name, content );
                    }
                } catch (Exception ex) {
                    context.ReportDiagnostic( Diagnostic.Create( DiagnosticDescriptor, null, ex.Message ) );
                }
            }
        }


        // Helpers
        private static string GetSourceName(SyntaxTree tree) {
            return Path.GetFileNameWithoutExtension( tree.FilePath ) + $".Generated.{Guid.NewGuid()}.cs";
        }
        // Helpers/Generation/CompilationUnit
        private static CompilationUnitSyntax? CreateCompilationUnit(CompilationUnitSyntax unit) {
            var members = unit.Members.Select( CreateMemberDeclaration ).OfType<MemberDeclarationSyntax>().ToArray();
            if (!members.Any()) return null;
            return SyntaxFactoryUtils.CreateCompilationUnit( unit ).AddMembers( members );
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
            return SyntaxFactoryUtils.CreateNamespaceDeclaration( @namespace ).AddMembers( members );
        }


    }
}
