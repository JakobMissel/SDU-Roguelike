using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckRange", story: "[Agent] checks distance to [Target] within [Range]", category: "Action", id: "8dafec751a90175eab60294faeb861da")]
public partial class CheckRangeAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<Enemy> Enemy;
    [SerializeReference] public BlackboardVariable<float> AttackRange;
    [SerializeReference] public BlackboardVariable<float> DashRange;
    

    protected override Status OnStart()
    {
        if (Agent.Value == null)
        {
            LogFailure("No agent assigned.");
            return Status.Failure;
        }
        if (Target.Value == null)
        {
            LogFailure("No target assigned.");
            return Status.Failure;
        }
        AttackRange.Value = Enemy.Value.attackRange;
        DashRange.Value = Enemy.Value.dashRange;
        var distance = Vector3.Distance(Agent.Value.transform.position, Target.Value.transform.position);
        if (distance > AttackRange.Value)
        {
            LogFailure("Target out of range.");
            return Status.Failure;
        }
        Debug.Log("Target in range.");
        return Status.Success;
    }
}

