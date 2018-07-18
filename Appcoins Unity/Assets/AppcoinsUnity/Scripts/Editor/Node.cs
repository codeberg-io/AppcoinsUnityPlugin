class Node<T>
{
    private T _item;
    private List<Node<T>> _childs;

    public Node()
    {
        _item = new T();
        _childs = New List<Node<T>>();
    }

    public Node(T item) : this()
    {
        _item = item;
    }

    public GetItem()
    {
        return _item;
    }

    public SetItem(T item)
    {
        _item = item;
    }

    public void ForEach(Action<Node<T>> action)
    {
        _childs.ForEach(action);
    }

    public void AddChild(Node<T> child)
    {
        _childs.Add(child);
    }

    public void RemoveChildAt(int index)
    {
        return _childs.RemoveAt(index);
    }

    public bool RemoveChild(Node<T> childToRemove)
    {
        return _childs.Remove(childToRemove);
    }

    public FindChild(Node<T> childToSearch)
    {
        return _childs.Find(childToSearch);
    }
}

class Tree
{

}