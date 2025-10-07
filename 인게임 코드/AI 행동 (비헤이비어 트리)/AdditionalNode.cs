using UnityEngine;

public class WaitNode : LeafNode
{
    public float time { get; protected set; }
    public float waitTime { get; protected set; }

    public override eNodeState Excute()
    {
        time += Time.deltaTime;        
        if (time < waitTime) return eNodeState.Running;        
        return eNodeState.Success;
    }
    public override void Reset()
    {
        time = 0;        
    }

    public WaitNode(float wait)
    {
        time = 0;
        waitTime = wait;
    }
}

public class RandomNode : CompositeNode
{
    public int weight { get; protected set; }

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
                    Reset();
                    return eNodeState.Failure;
            }
        }
        Reset();
        return eNodeState.Failure;
    }
    public override void Reset()
    {
        var rand = Random.Range(0, weight * children.Count);
        var value = 0;

        for (var i = 0; i < children.Count; i++)
        {
            value += weight;
            if (value >= rand)
            {
                index = i;
                break;
            }
        }
    }

    public RandomNode(int weight, params INode[] children) : base(children)
    {
        this.weight = weight;
    }
    public RandomNode(params INode[] children) : base(children)
    {
        weight = 100;
    }

}
