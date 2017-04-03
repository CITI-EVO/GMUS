using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Models;
using CITI.EVO.UserManagement.Web.Units;
using NHibernate;
using NHibernate.Linq;

namespace CITI.EVO.UserManagement.Web.Utils
{
    public static class UmSchemasUtil
    {
        public static IEnumerable<TreeNodeUnit> CreateListOfTree(ISession session)
        {
            var projects = (from n in session.Query<UM_Project>()
                            where n.DateDeleted == null
                            orderby n.Name
                            select n).ToList();

            var globalProjectEntity = new UM_Project
            {
                ID = Guid.Empty,
                Name = "Global",
            };

            projects.Insert(0, globalProjectEntity);

            foreach (var project in projects)
            {
                var projectUnit = new TreeNodeUnit
                {
                    ID = project.ID,
                    Name = project.Name,
                    Type = "Project"
                };

                yield return projectUnit;

                if (project.AttributesSchemas != null)
                {
                    foreach (var schema in project.AttributesSchemas)
                    {
                        if (schema.DateDeleted != null)
                            continue;

                        var schemaUnit = new TreeNodeUnit
                        {
                            ID = schema.ID,
                            Name = schema.Name,
                            ParentID = project.ID,
                            Type = "Schema"
                        };

                        yield return schemaUnit;

                        if (schema.AttributeFields != null)
                        {
                            foreach (var field in schema.AttributeFields)
                            {
                                if (field.DateDeleted != null)
                                    continue;

                                var nodeUnit = new TreeNodeUnit
                                {
                                    ID = field.ID,
                                    Name = field.Name,
                                    ParentID = schema.ID,
                                    Type = "Field"
                                };

                                yield return nodeUnit;
                            }
                        }

                    }
                }
            }
        }

        public static IEnumerable<TreeNodeUnit> CreateListOfTree(ISession session, AttributeSchemasModel model)
        {
            if (model == null || model.List == null)
                yield break;

            var projects = (from n in session.Query<UM_Project>()
                            where n.DateDeleted == null
                            orderby n.Name
                            select n).ToList();

            var globalProjectEntity = new UM_Project
            {
                ID = Guid.Empty,
                Name = "Global",
            };

            projects.Insert(0, globalProjectEntity);

            var schemasLp = model.List.ToLookup(n => n.ProjectID.GetValueOrDefault());

            foreach (var project in projects)
            {
                var projectUnit = new TreeNodeUnit
                {
                    ID = project.ID,
                    Name = project.Name,
                    Type = "Project"
                };

                yield return projectUnit;

                var schemas = schemasLp[project.ID];
                foreach (var schema in schemas)
                {
                    var schemaUnit = new TreeNodeUnit
                    {
                        ID = schema.ID,
                        Name = schema.Name,
                        ParentID = project.ID,
                        Type = "Schema"
                    };

                    yield return schemaUnit;

                    var fields = schema.Fields;
                    if (fields != null && fields.List != null)
                    {
                        foreach (var field in schema.Fields.List)
                        {
                            var nodeUnit = new TreeNodeUnit
                            {
                                ID = field.ID,
                                Name = field.Name,
                                ParentID = schema.ID,
                                Type = "Field"
                            };

                            yield return nodeUnit;
                        }
                    }
                }
            }
        }
    }
}