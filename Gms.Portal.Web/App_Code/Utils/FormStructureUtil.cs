using System;
using System.Collections.Generic;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Helpers;
using Gms.Portal.Web.Models;

namespace Gms.Portal.Web.Utils
{
    public static class FormStructureUtil
    {
        public static IEnumerable<FormTreeNodeEntity> CreateTree(IEnumerable<FormModel> parents)
        {
            if (parents == null)
                yield break;

            foreach (var parent in parents)
            {
                foreach (var entity in CreateTree(parent))
                    yield return entity;
            }
        }
        public static IEnumerable<FormTreeNodeEntity> CreateTree(FormModel parent)
        {
            if (parent == null || parent.FormEntity == null)
                yield break;

            var stack = new Stack<ControlTreeEntity>();
            stack.Push(new ControlTreeEntity(parent.FormEntity));

            while (stack.Count > 0)
            {
                var item = stack.Pop();
                var entity = item.Control;

                var nodeEntity = new FormTreeNodeEntity
                {
                    ID = entity.ID,
                    Type = GetEntityTypeName(entity),
                    FormID = parent.ID,
                    ParentID = item.ParentID,
                };

                if (stack.Count == 0)
                {
                    nodeEntity.Name = parent.Name;
                    nodeEntity.Number = parent.Number;
                    nodeEntity.Language = parent.Language;
                }

                var namedControl = entity as NamedControlEntity;
                if (namedControl != null)
                    nodeEntity.Name = namedControl.Name;

                var fieldEntity = entity as FieldEntity;
                if (fieldEntity == null)
                    nodeEntity.OrderIndex = nodeEntity.OrderIndex;

                yield return nodeEntity;

                var container = entity as ContainerControlEntity;
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

        public static IEnumerable<TreeNodeEntity> CreateTree(IEnumerable<ContainerControlEntity> parents)
        {
            if (parents == null)
                yield break;

            foreach (var parent in parents)
            {
                foreach (var entity in CreateTree(parent))
                    yield return entity;
            }
        }
        public static IEnumerable<TreeNodeEntity> CreateTree(ContainerControlEntity parent)
        {
            if (parent == null || parent.Controls == null)
                yield break;

            var stack = new Stack<ControlTreeEntity>();
            stack.Push(new ControlTreeEntity(parent));

            while (stack.Count > 0)
            {
                var item = stack.Pop();
                var entity = item.Control;

                var nodeEntity = new TreeNodeEntity
                {
                    ID = entity.ID,
                    ParentID = item.ParentID,
                };

                var namedControl = entity as NamedControlEntity;
                if (namedControl != null)
                    nodeEntity.Name = namedControl.Name;

                yield return nodeEntity;

                var container = entity as ContainerControlEntity;
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

        public static IEnumerable<ControlEntity> PreOrderTraversal(IEnumerable<ControlEntity> parents)
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

            var parentContainer = parent as ContainerControlEntity;
            if (parentContainer != null)
            {
                var stack = new Stack<ControlEntity>();
                stack.Push(parent);

                while (stack.Count > 0)
                {
                    var item = stack.Pop();

                    yield return item;

                    var childContainer = item as ContainerControlEntity;
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

        public static IEnumerable<ControlEntity> InOrderTraversal(IEnumerable<ContainerControlEntity> parents)
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
            var container = parent as ContainerControlEntity;
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

            var parentContainer = parent as ContainerControlEntity;
            if (parentContainer != null)
            {
                var stack = new Stack<ControlEntity>();
                stack.Push(parent);

                while (stack.Count > 0)
                {
                    var item = stack.Pop();

                    yield return item;

                    if (item is GridEntity)
                        continue;

                    var childContainer = item as ContainerControlEntity;
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

        private static bool IsLeaf(ControlEntity entity)
        {
            var container = entity as ContainerControlEntity;

            if (container == null ||
                container.Controls == null ||
                container.Controls.Count == 0)
            {
                return true;
            }

            return false;
        }

        private static String GetEntityTypeName(ControlEntity entity)
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