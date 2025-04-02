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
    [SerializeReference] public BlackboardVariable<float> RotationSpeed;
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
    
    Status FaceTarget(GameObject target)
    {
        Vector3 direction = target.transform.position - Agent.Value.transform.position;
        direction.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(direction, target.transform.up);
        Agent.Value.transform.rotation = Quaternion.Slerp(Agent.Value.transform.rotation, targetRotation, Time.deltaTime * RotationSpeed);

        float dotProduct = Vector3.Dot(Agent.Value.transform.forward, direction);
        if (dotProduct > 0.8f)
        {
            Attack();
            return Status.Success;
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return FaceTarget(Target.Value);
    }
}

