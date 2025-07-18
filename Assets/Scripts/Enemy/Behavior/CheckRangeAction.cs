using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "CheckRange", story: "[Agent] checks distance to [Target] within [Range]", category: "Action", id: "8dafec751a90175eab60294faeb861da")]
public partial class CheckRangeAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<Enemy> Enemy;
    [SerializeReference] public BlackboardVariable<float> AttackRange;
    [SerializeReference] public BlackboardVariable<float> DashRange;
    [SerializeReference] public BlackboardVariable<float> FleeRange;
    [SerializeReference] public BlackboardVariable<bool> IsRanged;


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
        FleeRange.Value = Enemy.Value.fleeRange;
        Animator.Value.SetFloat("speed", Enemy.Value.speed);
        var distance = Vector3.Distance(Agent.Value.transform.position, Target.Value.transform.position);
        if (distance > AttackRange.Value && !IsRanged.Value)
        {
            Animator.Value.SetBool("isMoving", true);
            return Status.Failure;
        }
        if (distance < FleeRange.Value && IsRanged.Value)
        {
            Animator.Value.SetBool("isMoving", true);
            return Status.Failure;
        }
        Animator.Value.SetBool("isMoving", false);
        return Status.Success;
    }
}

