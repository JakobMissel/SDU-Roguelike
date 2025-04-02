using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnemyAttack", story: "[Agent] attacks [Target]", category: "Action", id: "6a1af6ec6d59a2fc2b30641cc03e4602")]
public partial class EnemyAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<Enemy> Enemy;

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
        return Status.Running;
    }

    void Attack()
    {
        GameObject target = Target.Value;
        Enemy.Value.Attack(target);
    }

    protected override Status OnUpdate()
    {
        Attack();
        return Status.Running;
    }
}

