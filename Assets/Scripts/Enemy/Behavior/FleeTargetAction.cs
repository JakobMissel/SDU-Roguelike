using System;
using Unity.Behavior;
using Action = Unity.Behavior.Action;
using UnityEngine;
using Random = UnityEngine.Random;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FleeTarget", story: "[Agent] flees from target", category: "Action", id: "f8044c730d164c3556c754d149001877")]
public partial class FleeTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<NavMeshAgent> NavMeshAgent;
    [SerializeReference] public BlackboardVariable<Enemy> Enemy;
    Vector3 direction;
    float randomAngle;

    protected override Status OnStart()
    {
        if (Agent.Value == null)
        {
            LogFailure("No agent assigned.");
            return Status.Failure;
        }
        randomAngle = Random.Range(-60, 60);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(Enemy.Value.currentTarget == null) return Status.Failure;
        direction = (Agent.Value.transform.position - Enemy.Value.currentTarget.transform.position).normalized;
        GiveFleePoint();
        return Status.Running;
    }

    void GiveFleePoint()
    {
        direction = Quaternion.Euler(0, randomAngle, 0) * direction;
        var fleePosition = Agent.Value.transform.position + direction * Enemy.Value.fleeRange;
        NavMeshAgent.Value.SetDestination(fleePosition);
        NavMeshAgent.Value.stoppingDistance = 0;
    }
}

