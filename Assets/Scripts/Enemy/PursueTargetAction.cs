using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PursueTarget", story: "[Agent] pursues chosen target.", category: "Action", id: "5fb6ca2157126fafc14917583a4dc6da")]
public partial class PursueTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;

    NavMeshAgent navMeshAgent;
    Enemy enemy;
    protected override Status OnStart()
    {
        if (Agent.Value == null)
        {
            LogFailure("No agent assigned.");
            return Status.Failure;
        }
        Initialize();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Pursue();
        return Status.Success;
    }
    
    void Initialize()
    {
        navMeshAgent = Agent.Value.GetComponent<NavMeshAgent>();
        enemy = Agent.Value.GetComponent<Enemy>();
    }
    void Pursue()
    {
        navMeshAgent.SetDestination(enemy.currentTarget.transform.position);
    }
}

