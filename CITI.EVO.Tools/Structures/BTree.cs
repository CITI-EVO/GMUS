﻿using System;
using System.Collections.Generic;

namespace CITI.EVO.Tools.Structures
{
    public class BTree<TItem>
    {
        private readonly IComparer<TItem> _comparer;
        private readonly int _degree;

        private BNode<TItem> _root;
        private int _height;
        private int _count;

        public BTree(IComparer<TItem> comparer) : this(comparer, 64)
        {
        }
        public BTree(IComparer<TItem> comparer, int degree)
        {
            if (degree < 2)
                throw new ArgumentException("BTree degree must be at least 2", "degree");

            _comparer = comparer;

            _degree = degree;
            _height = 1;

            _root = new BNode<TItem>(_degree);
        }

        public int Count
        {
            get { return _count; }
        }

        public int Degree
        {
            get { return _degree; }
        }

        public int Height
        {
            get { return _height; }
        }

        public BNode<TItem> Find(TItem item)
        {
            var node = FindNode(_root, item);
            return node;
        }

        public bool Add(TItem item)
        {
            // there is space in the root node
            if (!_root.HasReachedMaxSize)
            {
                if (AddNonFull(_root, item))
                {
                    _count++;
                    return true;
                }

                return false;
            }

            // need to create new node and have it split
            var oldRoot = _root;

            _root = new BNode<TItem>(_degree);
            _root.InsertNode(oldRoot);

            _height++;

            SplitChild(_root, 0, oldRoot);

            if (AddNonFull(_root, item))
            {
                _count++;
                return true;
            }

            return false;
        }

        public bool Remove(TItem item)
        {
            if (RemoveInternal(_root, item))
            {
                _count--;
                return true;
            }

            if (_root.Items.Count == 0 && !_root.IsLeaf)
            {
                if (_root.Nodes.Count != 1)
                    throw new Exception();

                _root = _root.Nodes[0];
                _height--;
            }

            return false;
        }

        public bool Contains(TItem item)
        {
            var node = FindNode(_root, item);
            return node != null;
        }

        public void Clear()
        {
            _root = new BNode<TItem>(_degree);
        }

        private bool AddNonFull(BNode<TItem> node, TItem newItem)
        {
            while (true)
            {
                var indexToInsert = BinarySearchFindLast(node.Items, newItem);
                if (indexToInsert < node.Items.Count && CompareItems(newItem, node.Items[indexToInsert]) == 0)
                {
                    return false;
                    //throw new Exception("Item already exists");
                }


                // leaf node
                if (node.IsLeaf)
                {
                    node.InsertItem(indexToInsert, newItem);
                    return true;
                }

                // non-leaf
                var child = node.Nodes[indexToInsert];
                if (child.HasReachedMaxSize)
                {
                    SplitChild(node, indexToInsert, child);

                    if (CompareItems(newItem, node.Items[indexToInsert]) > 0)
                        indexToInsert++;
                }

                node = node.Nodes[indexToInsert];
            }
        }

        private bool RemoveInternal(BNode<TItem> node, TItem itemToDelete)
        {
            var count = BinarySearchFindFirst(node.Items, itemToDelete);
            if (count < node.Items.Count && CompareItems(node.Items[count], itemToDelete) == 0)
                return RemoveItemFromNode(node, itemToDelete, count);

            if (!node.IsLeaf)
                return RemoveItemFromSubtree(node, itemToDelete, count);

            return false;
        }

        private bool RemoveItemFromSubtree(BNode<TItem> parentNode, TItem itemToDelete, int subtreeIndexInNode)
        {
            var childNode = parentNode.Nodes[subtreeIndexInNode];

            // node has reached min # of entries, and removing any from it will break the btree property,
            // so this block makes sure that the "child" has at least "degree" # of nodes by moving an 
            // entry from a sibling node or merging nodes
            if (childNode.HasReachedMinSize)
            {
                var leftIndex = subtreeIndexInNode - 1;
                var leftSibling = subtreeIndexInNode > 0 ? parentNode.Nodes[leftIndex] : null;

                var rightIndex = subtreeIndexInNode + 1;
                var rightSibling = subtreeIndexInNode < parentNode.Nodes.Count - 1 ? parentNode.Nodes[rightIndex] : null;

                if (leftSibling != null && leftSibling.Items.Count > _degree - 1)
                {
                    // left sibling has a node to spare, so this moves one node from left sibling 
                    // into parent's node and one node from parent into this current node ("child")
                    childNode.InsertItem(0, parentNode.Items[subtreeIndexInNode]);
                    parentNode.Items[subtreeIndexInNode] = leftSibling.Items[leftSibling.Items.Count - 1];
                    leftSibling.RemoveItem(leftSibling.Items.Count - 1);

                    if (!leftSibling.IsLeaf)
                    {
                        childNode.InsertNode(0, leftSibling.Nodes[leftSibling.Nodes.Count - 1]);
                        leftSibling.RemoveNode(leftSibling.Nodes.Count - 1);
                    }
                }
                else if (rightSibling != null && rightSibling.Items.Count > _degree - 1)
                {
                    // right sibling has a node to spare, so this moves one node from right sibling 
                    // into parent's node and one node from parent into this current node ("child")
                    childNode.InsertItem(parentNode.Items[subtreeIndexInNode]);
                    parentNode.Items[subtreeIndexInNode] = rightSibling.Items[0];
                    rightSibling.RemoveItem(0);

                    if (!rightSibling.IsLeaf)
                    {
                        childNode.InsertNode(rightSibling.Nodes[0]);
                        rightSibling.RemoveNode(0);
                    }
                }
                else
                {
                    // this block merges either left or right sibling into the current node "child"
                    if (leftSibling != null)
                    {
                        childNode.InsertItem(0, parentNode.Items[subtreeIndexInNode]);

                        var oldEntries = childNode.Items;

                        childNode.Items = leftSibling.Items;
                        childNode.InsertItems(oldEntries);

                        if (!leftSibling.IsLeaf)
                        {
                            var oldChildren = childNode.Nodes;

                            childNode.Nodes = leftSibling.Nodes;
                            childNode.InsertNodes(oldChildren);
                        }

                        parentNode.RemoveNode(leftIndex);
                        parentNode.RemoveItem(subtreeIndexInNode);
                    }
                    else
                    {
                        childNode.InsertItem(parentNode.Items[subtreeIndexInNode]);
                        childNode.InsertItems(rightSibling.Items);

                        if (!rightSibling.IsLeaf)
                        {
                            childNode.InsertNodes(rightSibling.Nodes);
                        }

                        parentNode.RemoveNode(rightIndex);
                        parentNode.RemoveItem(subtreeIndexInNode);
                    }
                }
            }

            // at this point, we know that "child" has at least "degree" nodes, so we can
            // move on - this guarantees that if any node needs to be removed from it to
            // guarantee BTree's property, we will be fine with that
            return RemoveInternal(childNode, itemToDelete);
        }

        private bool RemoveItemFromNode(BNode<TItem> node, TItem itemToDelete, int itemIndexInNode)
        {
            // if leaf, just remove it from the list of entries (we're guaranteed to have
            // at least "degree" # of entries, to BTree property is maintained
            if (node.IsLeaf)
            {
                node.RemoveItem(itemIndexInNode);
                return true;
            }

            var predecessorChild = node.Nodes[itemIndexInNode];
            if (predecessorChild.Items.Count >= _degree)
            {
                var predecessor = RemovePredecessor(predecessorChild);
                node.Items[itemIndexInNode] = predecessor;
            }
            else
            {
                var successorChild = node.Nodes[itemIndexInNode + 1];
                if (successorChild.Items.Count >= _degree)
                {
                    var successor = RemoveSuccessor(predecessorChild);
                    node.Items[itemIndexInNode] = successor;
                }
                else
                {
                    predecessorChild.InsertItem(node.Items[itemIndexInNode]);

                    predecessorChild.InsertItems(successorChild.Items);
                    predecessorChild.InsertNodes(successorChild.Nodes);

                    node.RemoveItem(itemIndexInNode);
                    node.RemoveNode(itemIndexInNode + 1);

                    return RemoveInternal(predecessorChild, itemToDelete);
                }
            }

            return false;
        }

        private TItem RemovePredecessor(BNode<TItem> node)
        {
            while (true)
            {
                if (node.IsLeaf)
                {
                    var result = node.Items[node.Items.Count - 1];
                    node.RemoveItem(node.Items.Count - 1);

                    return result;
                }

                node = node.Nodes[node.Nodes.Count - 1];
            }
        }

        private TItem RemoveSuccessor(BNode<TItem> node)
        {
            while (true)
            {
                if (node.IsLeaf)
                {
                    var result = node.Items[0];
                    node.RemoveItem(0);

                    return result;
                }

                node = node.Nodes[0];
            }
        }

        private BNode<TItem> FindNode(BNode<TItem> node, TItem item)
        {
            while (true)
            {
                var index = BinarySearchFindFirst(node.Items, item);
                index = Clip(index, 0, node.Items.Count - 1);

                var order = CompareItems(item, node.Items[index]);
                if (order == 0)
                    return node;

                if (order > 0)
                    index++;

                if (node.IsLeaf)
                    return null;

                node = node.Nodes[index];
            }
        }

        private void SplitChild(BNode<TItem> parentNode, int nodeToBeSplitIndex, BNode<TItem> nodeToBeSplit)
        {
            var newNode = new BNode<TItem>(_degree);

            parentNode.InsertItem(nodeToBeSplitIndex, nodeToBeSplit.Items[_degree - 1]);
            parentNode.InsertNode(nodeToBeSplitIndex + 1, newNode);

            var keyRange = nodeToBeSplit.GetItems(_degree, _degree - 1);
            newNode.InsertItems(keyRange);

            nodeToBeSplit.RemoveItems(_degree - 1, _degree);

            if (!nodeToBeSplit.IsLeaf)
            {
                var nodesRange = nodeToBeSplit.GetNodes(_degree, _degree);
                newNode.InsertNodes(nodesRange);

                nodeToBeSplit.RemoveNodes(_degree, _degree);
            }
        }

        private int BinarySearch(IList<TItem> list, TItem value)
        {
            var index = 0;
            var length = list.Count;

            int low = index;
            int high = low + length - 1;

            while (low <= high)
            {
                var mid = low + ((high - low) / 2);

                var order = CompareItems(value, list[mid]);
                if (order == 0)
                    return mid;

                if (order < 0)
                    high = mid - 1;
                else
                    low = mid + 1;
            }

            return ~low;
        }
        private int BinarySearchFindFirst(IList<TItem> list, TItem item)
        {
            if (list.Count > 0)
            {
                if (CompareItems(item, list[0]) < 0)
                    return 0;

                if (CompareItems(item, list[list.Count - 1]) > 0)
                    return list.Count;
            }

            int start = 0;
            int end = list.Count - 1;

            int low = start;
            int high = end;

            while (low <= high)
            {
                var mid = low + ((high - low) / 2);

                var order = CompareItems(item, list[mid]);
                if (order == 0)
                {
                    if (mid == start)
                        return mid;

                    order = CompareItems(item, list[mid - 1]);
                    if (order != 0)
                        return mid;

                    high = mid - 1;
                }
                else if (order < 0)
                {
                    high = mid - 1;
                }
                else
                {
                    low = mid + 1;
                }
            }

            return low;
            //return ~low;
        }
        private int BinarySearchFindLast(IList<TItem> list, TItem item)
        {
            if (list.Count > 0)
            {
                if (CompareItems(item, list[0]) < 0)
                    return 0;

                if (CompareItems(item, list[list.Count - 1]) >= 0)
                    return list.Count;
            }

            int start = 0;
            int end = list.Count - 1;

            int low = start;
            int high = end;

            while (low <= high)
            {
                var mid = low + ((high - low) / 2);

                var order = CompareItems(item, list[mid]);
                if (order == 0)
                {
                    if (mid == end)
                        return mid;

                    order = CompareItems(item, list[mid + 1]);
                    if (order != 0)
                        return mid;

                    low = mid + 1;
                }
                else if (order < 0)
                {
                    high = mid - 1;
                }
                else
                {
                    low = mid + 1;
                }
            }

            return low;
            //return ~low;
        }

        private int CompareItems(TItem x, TItem y)
        {
            if (x == null || y == null)
                throw new Exception();

            return _comparer.Compare(x, y);
        }

        public IEnumerable<TItem> TraversalItems()
        {
            return TraversalItems(_root);
        }
        private IEnumerable<TItem> TraversalItems(BNode<TItem> node)
        {
            if (node == null)
            {
                yield break;
            }

            var stack = new Stack<BNode<TItem>>();
            stack.Push(node);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                if (current != null)
                {
                    if (current.Items != null && current.Items.Count > 0)
                    {
                        foreach (var key in current.Items)
                            yield return key;
                    }

                    if (current.Nodes != null && current.Nodes.Count > 0)
                    {
                        foreach (var child in current.Nodes)
                        {
                            if (child != null)
                                stack.Push(child);
                        }
                    }
                }
            }
        }

        public IEnumerable<BNode<TItem>> TraversalNodes()
        {
            return TraversalNodes(_root);
        }
        private IEnumerable<BNode<TItem>> TraversalNodes(BNode<TItem> node)
        {
            if (node == null)
                yield break;

            var stack = new Stack<BNode<TItem>>();
            stack.Push(node);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                if (current != null)
                {
                    yield return current;

                    if (current.Nodes != null && current.Nodes.Count > 0)
                    {
                        foreach (var child in current.Nodes)
                        {
                            if (child != null)
                                stack.Push(child);
                        }
                    }
                }
            }
        }

        private int Clip(int n, int minValue, int maxValue)
        {
            return Math.Min(Math.Max(n, minValue), maxValue);
        }
    }
}