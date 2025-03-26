using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckRange", story: "[Agent] checks distance to [Target] for [Range]", category: "Action", id: "8dafec751a90175eab60294faeb861da")]
public partial class CheckRangeAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Range;
    
    Enemy enemy;

    protected override Status OnStart()
    {
        if (Agent.Value == null)
        {
            LogFailure("No agent assigned.");
            return Status.Failure;
        }
        enemy = Agent.Value.GetComponent<Enemy>();
        Range.Value = enemy.attackRange;
        var distance = Vector3.Distance(Agent.Value.transform.position, Target.Value.transform.position);
        if (distance > Range.Value)
        {
            LogFailure("Target out of range.");
        }
        Debug.Log("Target in range.");
        return Status.Running;
    }
}

