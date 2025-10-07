using System.Collections.Generic;

public enum eNodeState
{
    Success,
    Failure,
    Running,
}

public interface INode
{
    eNodeState Excute();
    void Reset();
}

public abstract class LeafNode : INode
{
    public abstract eNodeState Excute();
    public abstract void Reset();
}

public abstract class CompositeNode : INode
{
    public int index { get; protected set; }
    public List<INode> children { get; protected set; }

    public void AddNode(INode node)
    {
        children.Add(node);
    }

    public abstract eNodeState Excute();
    public virtual void Reset()
    {
        foreach (var item in children)
            item.Reset();
        index = 0;
    }

    public CompositeNode(params INode[] children)
    {
        this.children = new List<INode>(children);
    }
}