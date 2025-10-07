using System;

public class ActionNode : LeafNode
{
    public Func<eNodeState> Action { get; protected set; }

    public override eNodeState Excute()
    {        
        return Action();
    }
    public override void Reset() { }    

    public ActionNode(Func<eNodeState> action)
    {
        Action = action;
    }
}

public class ConditionNode : LeafNode
{
    public Func<bool> Action { get; protected set; }

    public override eNodeState Excute()
    {
        return Action() ? eNodeState.Success : eNodeState.Failure;
    }
    public override void Reset() { }    

    public ConditionNode(Func<bool> action)
    { 
        Action = action; 
    }
}

public class SelectorNode : CompositeNode
{
    public override eNodeState Excute()
    {
        if (index < children.Count)
        {            
            switch (children[index].Excute())
            {
                case eNodeState.Success:
                    Reset();
                    return eNodeState.Success;
                case eNodeState.Running:
                    return eNodeState.Running;
                case eNodeState.Failure:
                    index++;
                    return eNodeState.Running;
            }
        }
        Reset();
        return eNodeState.Failure;
    }

    public SelectorNode(params INode[] children) : base(children) { }
}

public class SequenceNode : CompositeNode
{
    public override eNodeState Excute()
    {
        if (index < children.Count)
        {
            switch (children[index].Excute())
            {
                case eNodeState.Success:
                    index++;
                    return eNodeState.Running;
                case eNodeState.Running:
                    return eNodeState.Running;
                case eNodeState.Failure:
                    Reset();
                    return eNodeState.Failure;
            }
        }
        Reset();
        return eNodeState.Success;
    }

    public SequenceNode(params INode[] children) : base(children) { }
}

public class ParalleNode : CompositeNode
{
    public override eNodeState Excute()
    {
        foreach (var item in children)
            item.Excute();
        return eNodeState.Success;
    }

    public ParalleNode(params INode[] children) : base(children) { }
}