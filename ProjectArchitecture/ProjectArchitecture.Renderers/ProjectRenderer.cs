// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

namespace ProjectArchitecture.Renderers {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ProjectArchitecture.Model;

    public static class ProjectRenderer {


        public static string Render(this Project project) {
            var builder = new HierarchicalStringBuilder();
            using (builder.AppendTitle( "Project: {0}", project.Name )) {
                foreach (var module in project.Modules) {
                    using (builder.AppendSection( "Module: {0}", module.Name )) {
                        foreach (var @namespace in module.Namespaces) {
                            using (builder.AppendSection( "Namespace: {0}", @namespace.Name )) {
                                foreach (var group in @namespace.Groups) {
                                    using (builder.AppendSection( group.Name )) {
                                        foreach (var type in group.Types) {
                                            builder.AppendLineWithPrefix( "| * ", type.Name );
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return builder.ToString();
        }


    }
}
