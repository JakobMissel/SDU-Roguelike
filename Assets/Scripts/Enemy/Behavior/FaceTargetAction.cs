using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FaceTarget", story: "[Agent] faces [Target]", category: "Action", id: "28c6e60ac1f1f3ceacc962aea5304167")]
public partial class FaceTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> RotationSpeed;
    [SerializeReference] public BlackboardVariable<float> FacingAccuracy;
    [SerializeReference] public BlackboardVariable<bool> IsRanged;
    Vector3 direction;

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
        if (RotationSpeed.Value <= 0)
        {
            LogFailure("Rotation speed must be greater than zero.");
            return Status.Failure;
        }
        if (FacingAccuracy.Value < 0 || FacingAccuracy.Value > 1)
        {
            LogFailure("Facing accuracy must be between 0 and 1.");
            return Status.Failure;
        }
        return Status.Running;
    }
    Status FaceTarget(GameObject target)
    {
        if(target == null)
        {
            return Status.Failure;
        }
        if (IsRanged.Value)
        {
            direction = Agent.Value.transform.position - target.transform.position;
        }
        else
        {
            direction = target.transform.position - Agent.Value.transform.position;
        }
        direction.y = 0; // Ignore vertical direction
        var targetRotation = Quaternion.LookRotation(direction);
        Agent.Value.transform.rotation = Quaternion.Slerp(Agent.Value.transform.rotation, targetRotation, Time.deltaTime * RotationSpeed);

        float dotProduct = Vector3.Dot(Agent.Value.transform.forward, direction.normalized);
        if (dotProduct > FacingAccuracy.Value)
        {
            return Status.Success;
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return FaceTarget(Target.Value);
    }
}

