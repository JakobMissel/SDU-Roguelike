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

    Enemy enemy;

    protected override Status OnStart()
    {
        if (Agent.Value == null)
        {
            LogFailure("No agent assigned.");
            return Status.Failure;
        }
        return Status.Running;
    }

    void Attack()
    {
        enemy = Agent.Value.GetComponent<Enemy>();
        GameObject target = Target.Value;
        //Agent.Value.transform.LookAt(target.transform);
        Vector3 direction = target.transform.position - Agent.Value.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction, target.transform.up);
        Agent.Value.transform.rotation = Quaternion.Slerp(Agent.Value.transform.rotation, targetRotation, Time.deltaTime * 5);
        target.GetComponentInParent<Health>().TakeDamage(enemy.damage);
    }

    protected override Status OnUpdate()
    {
        Attack();
        return Status.Running;
    }
}

