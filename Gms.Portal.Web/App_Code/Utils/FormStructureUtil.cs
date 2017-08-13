using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.ExpressionEngine;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Models;

namespace Gms.Portal.Web.Utils
{
    public static class FormStructureUtil
    {
        public static String ComputeHashCode(ContentEntity content, FormDataUnit formData)
        {
            var controls = PreOrderFirstLevelTraversal(content);
            return ComputeHashCode(controls, formData);
        }
        public static String ComputeHashCode(IEnumerable<ControlEntity> controls, FormDataUnit formData)
        {
            var uniqFields = (from n in controls.OfType<FieldEntity>()
                              where n.Unique.GetValueOrDefault()
                              orderby n.ID
                              select n);

            return ComputeHashCode(uniqFields, formData);
        }
        public static String ComputeHashCode(IEnumerable<FieldEntity> fields, FormDataUnit formData)
        {
            var keysQuery = (from n in fields
                             where n.Unique.GetValueOrDefault()
                             let k = Convert.ToString(n.ID)
                             select k);

            return ComputeHashCode(keysQuery, formData);
        }
        public static String ComputeHashCode(IEnumerable<String> keys, FormDataUnit formData)
        {
            var valList = (from n in keys
                           let v = (Convert.ToString(formData[n]) ?? String.Empty)
                           select v.Trim()).ToList();

            if (valList.Count == 0)
                return null;

            var values = String.Join("§", valList);

            var hashCode = CryptographyUtil.ComputeSHA1(values);
            return hashCode;
        }

        public static IDictionary<String, String> CreateCompitabilityMap(ContentEntity entity)
        {
            var result = new Dictionary<String, String>();

            var controls = PreOrderTraversal(entity);
            foreach (var control in controls)
            {
                var name = control.Alias;
                if (String.IsNullOrWhiteSpace(name))
                    name = control.Name;

                var key = Convert.ToString(control.ID);

                result.Add(key, ExpressionParser.Escape(name));
            }

            return result;
        }

        public static IEnumerable<ElementTreeNodeEntity> CreateTree(ContentEntity parent)
        {
            if (parent == null)
                yield break;

            var stack = new Stack<ControlTreeEntity>();
            stack.Push(new ControlTreeEntity(parent));

            while (stack.Count > 0)
            {
                var item = stack.Pop();
                var entity = item.Control;

                var nodeEntity = new ElementTreeNodeEntity
                {
                    ID = entity.ID,
                    Name = entity.Name,
                    Visible = entity.Visible,
                    ParentID = item.ParentID,
                    OrderIndex = entity.OrderIndex,
                    ElementType = GetElementTypeName(entity),
                    ControlType = GetControlTypeName(entity),
                };

                yield return nodeEntity;

                var container = entity as ContentEntity;
                if (container != null && container.Controls != null)
                {
                    foreach (var child in container.Controls)
                    {
                        if (child != null)
                            stack.Push(new ControlTreeEntity(child, entity.ID));
                    }
                }
            }
        }
        public static IEnumerable<ElementTreeNodeEntity> CreateTree(IEnumerable<ControlEntity> parents)
        {
            if (parents == null)
                yield break;

            foreach (var parent in parents)
            {
                foreach (var entity in CreateTree(parent))
                    yield return entity;
            }
        }
        public static IEnumerable<ElementTreeNodeEntity> CreateTree(ControlEntity parent)
        {
            if (parent == null)
                yield break;

            var stack = new Stack<ControlTreeEntity>();
            stack.Push(new ControlTreeEntity(parent));

            while (stack.Count > 0)
            {
                var item = stack.Pop();
                var entity = item.Control;

                var nodeEntity = new ElementTreeNodeEntity
                {
                    ID = entity.ID,
                    ParentID = item.ParentID,
                    Name = entity.Name,
                    Alias = entity.Alias,
                    Visible = entity.Visible,
                    OrderIndex = entity.OrderIndex,
                    ElementType = GetElementTypeName(entity),
                    ControlType = GetControlTypeName(entity),
                };

                yield return nodeEntity;

                var container = entity as ContentEntity;
                if (container != null && container.Controls != null)
                {
                    foreach (var child in container.Controls)
                    {
                        if (child != null)
                            stack.Push(new ControlTreeEntity(child, entity.ID));
                    }
                }
            }
        }

        public static IEnumerable<ElementTreeNodeEntity> ParentsTraversal(IDictionary<Guid?, ElementTreeNodeEntity> dict, Guid? controlID)
        {
            var node = dict.GetValueOrDefault(controlID);

            while (node != null)
            {
                var parent = dict.GetValueOrDefault(node.ParentID.GetValueOrDefault());
                if (parent != null)
                    yield return parent;

                node = parent;
            }
        }

        public static IEnumerable<ControlEntity> PreOrderTraversal(IEnumerable<ContentEntity> parents)
        {
            if (parents == null)
                yield break;

            foreach (var parent in parents)
            {
                foreach (var entity in PreOrderTraversal(parent))
                    yield return entity;
            }
        }
        public static IEnumerable<ControlEntity> PreOrderTraversal(ControlEntity parent)
        {
            if (parent == null)
                yield break;

            var parentContainer = parent as ContentEntity;
            if (parentContainer != null)
            {
                var stack = new Stack<ControlEntity>();
                stack.Push(parent);

                while (stack.Count > 0)
                {
                    var item = stack.Pop();

                    yield return item;

                    var childContainer = item as ContentEntity;
                    if (childContainer != null && childContainer.Controls != null)
                    {
                        foreach (var child in childContainer.Controls)
                        {
                            if (child != null)
                                stack.Push(child);
                        }
                    }
                }
            }
        }

        public static IEnumerable<ControlEntity> PreOrderIndexedTraversal(ContentEntity entity)
        {
            if (entity == null || entity.Controls == null)
                return Enumerable.Empty<ControlEntity>();

            var array = new ControlEntity[] { entity };
            return PreOrderIndexedTraversal(array);
        }
        private static IEnumerable<ControlEntity> PreOrderIndexedTraversal(IEnumerable<ControlEntity> source)
        {
            var sorted = (from n in source
                          orderby n.OrderIndex, n.Name
                          select n);

            var stack = new Stack<ControlEntity>(sorted);

            while (stack.Count > 0)
            {
                var item = stack.Pop();

                yield return item;

                var container = item as ContentEntity;
                if (container != null && container.Controls != null)
                {
                    var children = (from n in container.Controls
                                    orderby n.OrderIndex, n.Name
                                    select n);

                    foreach (var child in children)
                    {
                        if (child != null)
                            stack.Push(child);
                    }
                }
            }
        }

        public static IEnumerable<ControlEntity> PreOrderFirstLevelTraversal(IEnumerable<ControlEntity> parents)
        {
            if (parents == null)
                yield break;

            foreach (var parent in parents)
            {
                foreach (var entity in PreOrderFirstLevelTraversal(parent))
                    yield return entity;
            }
        }
        public static IEnumerable<ControlEntity> PreOrderFirstLevelTraversal(ControlEntity parent)
        {
            if (parent == null)
                yield break;

            var parentContainer = parent as ContentEntity;
            if (parentContainer != null)
            {
                var stack = new Stack<ControlEntity>();
                stack.Push(parent);

                while (stack.Count > 0)
                {
                    var item = stack.Pop();

                    yield return item;

                    if ((item is GridEntity || item is TreeEntity) && !ReferenceEquals(item, parent))
                        continue;

                    var childContainer = item as ContentEntity;
                    if (childContainer != null && childContainer.Controls != null)
                    {
                        foreach (var child in childContainer.Controls)
                        {
                            if (child != null)
                                stack.Push(child);
                        }
                    }
                }
            }
        }

        public static IEnumerable<ControlEntity> InOrderTraversal(IEnumerable<ContentEntity> parents)
        {
            if (parents == null)
                yield break;

            foreach (var parent in parents)
            {
                foreach (var entity in InOrderTraversal(parent))
                    yield return entity;
            }
        }
        public static IEnumerable<ControlEntity> InOrderTraversal(ControlEntity parent)
        {
            var container = parent as ContentEntity;
            if (container != null && container.Controls != null)
            {
                foreach (var item in container.Controls)
                {
                    if (!IsLeaf(item))
                    {
                        var children = InOrderTraversal(item);
                        foreach (var child in children)
                            yield return child;
                    }

                    yield return item;
                }
            }
        }

        public static IEnumerable<ControlEntity> InOrderIndexedTraversal(ContentEntity entity)
        {
            if (entity == null || entity.Controls == null)
                return Enumerable.Empty<ControlEntity>();

            var array = new ControlEntity[] { entity };
            return InOrderIndexedTraversal(array);
        }
        private static IEnumerable<ControlEntity> InOrderIndexedTraversal(IEnumerable<ControlEntity> source)
        {
            var sorted = source.OrderBy(n => n.OrderIndex).ThenBy(n => n.Name);

            foreach (var entity in sorted)
            {
                yield return entity;

                var container = entity as ContentEntity;
                if (container != null && container.Controls != null)
                {
                    var children = InOrderIndexedTraversal(container.Controls);
                    foreach (var child in children)
                        yield return child;
                }
            }
        }

        public static IEnumerable<ControlEntity> InOrderFirstLevelTraversal(ContentEntity entity)
        {
            if (entity == null || entity.Controls == null)
                return Enumerable.Empty<ControlEntity>();

            var array = new ControlEntity[] { entity };
            return InOrderFirstLevelTraversal(array);
        }
        private static IEnumerable<ControlEntity> InOrderFirstLevelTraversal(IEnumerable<ControlEntity> source)
        {
            var sorted = source.OrderBy(n => n.OrderIndex).ThenBy(n => n.Name);

            foreach (var entity in sorted)
            {
                if (entity is GridEntity || entity is TreeEntity)
                    continue;

                yield return entity;

                var container = entity as ContentEntity;
                if (container != null && container.Controls != null)
                {
                    var children = InOrderFirstLevelTraversal(container.Controls);
                    foreach (var child in children)
                        yield return child;
                }
            }
        }

        public static IEnumerable<ControlEntity> TraversalAndCorrectOrderIndexes(ContentEntity entity)
        {
            if (entity == null || entity.Controls == null)
                return Enumerable.Empty<ControlEntity>();

            var array = new ControlEntity[] { entity };
            return TraversalAndCorrectOrderIndexes(array, entity.OrderIndex);
        }
        private static IEnumerable<ControlEntity> TraversalAndCorrectOrderIndexes(IEnumerable<ControlEntity> source, int parentIndex)
        {
            foreach (var entity in source)
            {
                entity.OrderIndex += parentIndex;
                yield return entity;

                var container = entity as ContentEntity;
                if (container != null && container.Controls != null)
                {
                    var children = FirstLevelTraversalAndCorrectOrderIndexes(container.Controls, entity.OrderIndex);
                    foreach (var child in children)
                        yield return child;
                }
            }
        }

        public static IEnumerable<ControlEntity> FirstLevelTraversalAndCorrectOrderIndexes(ContentEntity entity)
        {
            if (entity == null || entity.Controls == null)
                return Enumerable.Empty<ControlEntity>();

            var array = new ControlEntity[] { entity };
            return FirstLevelTraversalAndCorrectOrderIndexes(array, entity.OrderIndex);
        }
        private static IEnumerable<ControlEntity> FirstLevelTraversalAndCorrectOrderIndexes(IEnumerable<ControlEntity> source, int parentIndex)
        {
            foreach (var entity in source)
            {
                if (entity is GridEntity || entity is TreeEntity)
                    continue;

                entity.OrderIndex += parentIndex;
                yield return entity;

                var container = entity as ContentEntity;
                if (container != null && container.Controls != null)
                {
                    var children = FirstLevelTraversalAndCorrectOrderIndexes(container.Controls, entity.OrderIndex);
                    foreach (var child in children)
                        yield return child;
                }
            }
        }

        private static bool IsLeaf(ControlEntity entity)
        {
            var container = entity as ContentEntity;

            if (container == null ||
                container.Controls == null ||
                container.Controls.Count == 0)
            {
                return true;
            }

            return false;
        }

        private static String GetControlTypeName(ControlEntity entity)
        {
            var field = entity as FieldEntity;
            if (field != null)
                return field.Type;

            return GetElementTypeName(entity);
        }

        private static String GetElementTypeName(ControlEntity entity)
        {
            if (entity is FormEntity)
                return "Form";

            if (entity is GridEntity)
                return "Grid";

            if (entity is TreeEntity)
                return "Tree";

            if (entity is FieldEntity)
                return "Field";

            if (entity is GroupEntity)
                return "Group";

            if (entity is TabPageEntity)
                return "TabPage";

            if (entity is TabContainerEntity)
                return "TabContainer";

            return "UNKNOWN";
        }
    }
}