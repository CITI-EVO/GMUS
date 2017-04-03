using System;
using System.Collections.Generic;
using System.Linq;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Models;

namespace Gms.Portal.Web.Utils
{
    public static class FormStructureUtil
    {
        public static IEnumerable<ElementTreeNodeEntity> CreateTree(FormEntity parent)
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

        public static IEnumerable<ControlEntity> PreOrderSortedTraversal(FormEntity entity)
        {
            if (entity == null || entity.Controls == null)
                return Enumerable.Empty<ControlEntity>();

            var array = new ControlEntity[] { entity };
            return PreOrderSortedTraversal(array);
        }
        private static IEnumerable<ControlEntity> PreOrderSortedTraversal(IEnumerable<ControlEntity> source)
        {
            var ordered = source.OrderBy(n => n.OrderIndex).ThenBy(n => n.Name);

            foreach (var entity in ordered)
            {
                yield return entity;

                var container = entity as ContentEntity;
                if (container != null && container.Controls != null)
                {
                    var children = PreOrderSortedTraversal(container.Controls);
                    foreach (var child in children)
                        yield return child;
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

                    if (item is GridEntity && !ReferenceEquals(item, parent))
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

        public static IEnumerable<ControlEntity> InOrderTraversal(FormEntity entity)
        {
            if (entity == null || entity.Controls == null)
                return Enumerable.Empty<ControlEntity>();

            var array = new ControlEntity[] { entity };
            return InOrderTraversal(array);
        }
        private static IEnumerable<ControlEntity> InOrderTraversal(IEnumerable<ControlEntity> source)
        {
            var ordered = source.OrderBy(n => n.OrderIndex).ThenBy(n => n.Name);

            foreach (var entity in ordered)
            {
                yield return entity;

                var container = entity as ContentEntity;
                if (container != null && container.Controls != null)
                {
                    var children = InOrderTraversal(container.Controls);
                    foreach (var child in children)
                        yield return child;
                }
            }
        }

        public static IEnumerable<ControlEntity> InOrderFirstLevelTraversal(FormEntity entity)
        {
            if (entity == null || entity.Controls == null)
                return Enumerable.Empty<ControlEntity>();

            var array = new ControlEntity[] { entity };
            return InOrderFirstLevelTraversal(array);
        }
        private static IEnumerable<ControlEntity> InOrderFirstLevelTraversal(IEnumerable<ControlEntity> source)
        {
            var ordered = source.OrderBy(n => n.OrderIndex).ThenBy(n => n.Name);

            foreach (var entity in ordered)
            {
                if (entity is GridEntity)
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

        public static IEnumerable<ControlEntity> TraversalAndCorrectOrderIndexes(FormEntity entity)
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

        public static IEnumerable<ControlEntity> FirstLevelTraversalAndCorrectOrderIndexes(FormEntity entity)
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
                if (entity is GridEntity)
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