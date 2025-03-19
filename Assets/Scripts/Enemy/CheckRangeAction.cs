using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using static UnityEngine.EventSystems.EventTrigger;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckRange", story: "[Agent] checks distance to target.", category: "Action", id: "8dafec751a90175eab60294faeb861da")]
public partial class CheckRangeAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;

    Enemy enemy;

    protected override Status OnStart()
    {
        enemy = Agent.Value.GetComponent<Enemy>();
        var distance = Vector3.Distance(Agent.Value.gameObject.transform.position, enemy.currentTarget.transform.position);
        if (distance > enemy.attackRange)
        {
            LogFailure("Target out of range.");
            return Status.Failure;
        }
        else
        {
            Debug.Log("Target in range.");
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

