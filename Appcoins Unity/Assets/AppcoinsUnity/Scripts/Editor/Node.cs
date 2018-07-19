using System;
using System.Collections.Generic;

// Node of a Tree or Graph
class Node<T>
{
    // Item to be stored by the node
    private T _item;

    // List with all childs (outer edges)
    private List<Node<T>> _childs;

    // Constructor with a specific 'T' item
    public Node(T item)
    {
        _item = item;
    }

    // Default constructor, item is default of 'T'
    public Node() : this(default(T))
    {
        _item = default(T);
        _childs = new List<Node<T>>();
    }

    // Get item of the node
    public T GetItem()
    {
        return _item;
    }

    // Set new item fot the node
    public void SetItem(T item)
    {
        _item = item;
    }

    // Apply delegate to all node's childs
    public void ForEach(Action<Node<T>> action)
    {
        _childs.ForEach(action);
    }

    // Add one child to node at the end of his 'childsList'
    public void AddChild(Node<T> child)
    {
        _childs.Add(child);
    }

    // Remove a node's child at a specific index
    public void RemoveChildAt(int index)
    {
        _childs.RemoveAt(index);
    }

    // Remove the first node's child with a specific item
    public bool RemoveChild(Node<T> childToRemove)
    {
        return _childs.Remove(childToRemove);
    }

    // Find a child by a predicate
    public Node<T> FindChild(Predicate<Node<T>> childToSearch)
    {
        return _childs.Find(childToSearch);
    }
}

class Tree<T>
{
    // Height of the tree
    private int _height;

    // Root node ('null' item)
    private Node<T> _root = null;

    // Constrctor with a specific root
    public Tree(Node<T> root)
    {
        _height = 0;
        _root = root;
    }

    // Contructor that creates a root node with a specific item
    public Tree(T item) : this(new Node<T>(item)) {}

    // (BFS) Find a the tree path from a list of nodes (returns the last node that exists at tree and list)
    // If a path's node exists in the tree that node is removed from the 'path' list
    private Node<T> FindPath(List<Node<T>> path)
    {
        bool childFounded = false;

        Node<T> treeNodeToSearch = _root;
        Node<T> currentPathNode = path[0];

        do
        {
            // Check if 'currentPathNode' is a child of 'treeNodeToSearch'
            Node<T> aux = treeNodeToSearch.FindChild(node => node.Equals(currentPathNode.GetItem()));

            if(!treeNodeToSearch.GetItem().Equals(default(T)))
            {
                treeNodeToSearch = aux;
                childFounded = true;
                path.RemoveAt(0);
                currentPathNode = path[0];
            }
            

        } while(childFounded && path.Count > 0);

        return treeNodeToSearch;
    }

    // Insert new path to tree
    private void InsertPath(Node<T> parentTreeNode, List<Node<T>> path)
    {
       if(path.Count <= 0)
       {
           return;
       }

       while(path.Count > 0)
       {
           parentTreeNode.AddChild(path[0]);
           parentTreeNode = path[0];
           path.RemoveAt(0);
       }
    }

    // Merge full path to tree (repeated nodes are not inserted)
    public void MergePath(List<Node<T>> path)
    {
        Node<T> parent = FindPath(path);
        InsertPath(parent, path);
    }
}