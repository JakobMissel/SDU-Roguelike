using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChooseTarget", story: "[Agent] chooses a [Tag] [Target]", category: "Action", id: "e1755334169e967d801e0f672311fe22")]
public partial class ChooseTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<string> Tag;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    [SerializeReference] public BlackboardVariable<bool> Random;

    Enemy enemy;

    bool isInitialized;

    GameObject[] targets;
    int currentTarget;
    protected override Status OnStart()
    {
        if (Agent.Value == null)
        {
            LogFailure("No agent assigned.");
            return Status.Failure;
        }
        if (Tag.Value == null || Tag.Value == "")
        {
            LogFailure("No target assigned.");
            return Status.Failure;
        }
        Initialize();
        return SetTarget();
    }


    void Initialize()
    {
        if (isInitialized) return;
        enemy = Agent.Value.GetComponent<Enemy>();
        GetTargets();
        isInitialized = true;
    }
    void GetTargets()
    {
        if (GameObject.FindGameObjectsWithTag(Tag.Value) == null)
        {
            LogFailure("No targets found.");
            return;
        }
        targets = GameObject.FindGameObjectsWithTag(Tag.Value);
    }

    Status SetTarget()
    {
        if (targets == null || targets.Length == 0)
        {
            LogFailure("No targets in array to choose.");
            return Status.Failure;
        }
        currentTarget = Random.Value ? GetRandomTarget() : GetClosestTarget();
        enemy.currentTarget = targets[currentTarget];
        Target.Value = targets[currentTarget];
        return Status.Success;
    }

    int GetClosestTarget()
    {
        int closestTarget = 0;
        float closestDistance = Mathf.Infinity;
        for (int i = 0; i < targets.Length; i++)
        {
            if(targets[i] == null) continue;
            float distance = Vector3.Distance(targets[i].transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = i;
            }
        }
        return closestTarget;
    }

    int GetRandomTarget()
    {
        int targetCount = targets.Length;
        int randomTarget = UnityEngine.Random.Range(0, targetCount);
        return randomTarget;
    }
}
