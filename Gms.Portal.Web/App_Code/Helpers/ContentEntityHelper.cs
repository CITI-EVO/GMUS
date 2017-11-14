using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.ExpressionEngine;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Utils;
using CITI.EVO.Tools.Extensions;

namespace Gms.Portal.Web.Helpers
{
    public class ContentEntityHelper
    {
        private readonly ContentEntity _content;

        private readonly ILookup<String, ControlEntity> _controlsTreeLp;

        private readonly ILookup<String, ControlEntity> _fullControlsLp;

        private readonly ILookup<String, ControlEntity> _firstLevelControlsLp;

        public ContentEntityHelper(ContentEntity content)
        {
            _content = content;

            _controlsTreeLp = GetControlsTreeLp(content);
            _fullControlsLp = GetFullControlsLp(content);

            _firstLevelControlsLp = GetFirstLevelControlsLp(content);
        }

        public IEnumerable<NameValueEntity<ControlEntity>> Find(String path)
        {
            if (String.IsNullOrWhiteSpace(path))
                yield break;

            path = ExpressionParser.Escape(path);

            var parts = path.Split('.');
            if (parts.Length == 0)
                yield break;

            var index = 0;
            var parent = GetControls(parts[index]).SingleOrDefault();

            while (parent != null && index < parts.Length)
            {
                var result = new NameValueEntity<ControlEntity>
                {
                    Name = parts[index],
                    Value = parent,
                };

                yield return result;

                var key = $"{parent.ID}/{parts[++index]}";
                parent = _controlsTreeLp[key].SingleOrDefault();
            }
        }

        public IEnumerable<ControlEntity> GetControls()
        {
            return FormStructureUtil.PreOrderTraversal(_content);
        }
        public IEnumerable<ControlEntity> GetFirstLevelControls()
        {
            return FormStructureUtil.PreOrderFirstLevelTraversal(_content);
        }

        public IEnumerable<ControlEntity> GetControls(Object idOrNameOrAlias)
        {
            var key = Convert.ToString(idOrNameOrAlias);
            return _fullControlsLp[key];
        }
        public IEnumerable<ControlEntity> GetFirstLevelControls(Object idOrNameOrAlias)
        {
            var key = Convert.ToString(idOrNameOrAlias);
            return _firstLevelControlsLp[key];
        }

        private ILookup<String, ControlEntity> GetControlsTreeLp(ContentEntity content)
        {
            var tree = FormStructureUtil.CreateTree(content);
            var parents = tree.ToDictionary(n => n.ID, n => n.ParentID);
            var controls = FormStructureUtil.PreOrderTraversal(content);

            var allControls = GetAllControls(controls);

            var query = (from n in allControls
                         let parentID = parents.GetValueOrDefault(n.Value.ID)
                         select new NameValueEntity<ControlEntity>
                         {
                             Name = $"{parentID}/{n.Name}",
                             Value = n.Value
                         });

            var comparer = StringComparer.OrdinalIgnoreCase;

            var controlsLp = query.ToLookup(n => n.Name, n => n.Value, comparer);
            return controlsLp;
        }
        private ILookup<String, ControlEntity> GetFullControlsLp(ContentEntity content)
        {
            var controls = FormStructureUtil.PreOrderTraversal(content);
            return GetControlsLp(controls);
        }
        private ILookup<String, ControlEntity> GetFirstLevelControlsLp(ContentEntity content)
        {
            var controls = FormStructureUtil.PreOrderFirstLevelTraversal(content);
            return GetControlsLp(controls);
        }

        private ILookup<String, ControlEntity> GetControlsLp(IEnumerable<ControlEntity> controls)
        {
            var finalQuery = GetAllControls(controls);
            var comparer = StringComparer.OrdinalIgnoreCase;

            var controlsLp = finalQuery.ToLookup(n => n.Name, n => n.Value, comparer);
            return controlsLp;
        }

        private IEnumerable<NameValueEntity<ControlEntity>> GetAllControls(IEnumerable<ControlEntity> controls)
        {
            var queryById = (from n in controls
                             select new NameValueEntity<ControlEntity>
                             {
                                 Name = Convert.ToString(n.ID),
                                 Value = n
                             });

            var queryByFmtId = (from n in controls
                                select new NameValueEntity<ControlEntity>
                                {
                                    Name = $"{n.ID:n}",
                                    Value = n
                                });

            var queryByName = (from n in controls
                               select new NameValueEntity<ControlEntity>
                               {
                                   Name = (n.Name ?? String.Empty),
                                   Value = n
                               });

            var queryByAlias = (from n in controls
                                select new NameValueEntity<ControlEntity>
                                {
                                    Name = (n.Alias ?? String.Empty),
                                    Value = n
                                });

            var baseQuery = queryById.Union(queryByFmtId).Union(queryByName).Union(queryByAlias);

            var escQuery = (from n in baseQuery
                            select new NameValueEntity<ControlEntity>
                            {
                                Name = ExpressionParser.Escape(n.Name),
                                Value = n.Value
                            });

            var finalQuery = baseQuery.Union(escQuery);
            return finalQuery;
        }
    }
}