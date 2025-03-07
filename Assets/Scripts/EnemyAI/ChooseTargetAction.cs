using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ChooseTarget", story: "[Agent] chooses a random [Tag] target to pursue after [minValue] - [maxValue] seconds.", category: "Action", id: "85b1f390edb409dc2b2a287e71d7326a")]
public partial class ChooseTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<string> Tag;
    [SerializeReference] public BlackboardVariable<float> minValue;
    [SerializeReference] public BlackboardVariable<float> maxValue;

    bool isInitialized;

    NavMeshAgent navMeshAgent;
    GameObject[] targets;
    int currentTarget;
    bool isTargetBeingChosen;
    float time;
    float currentTime;
    protected override Status OnStart()
    {
        if(Agent.Value == null)
        {
            LogFailure("No agent assigned.");
            return Status.Failure;
        }
        if (Tag == null || Tag == "")
        {
            LogFailure("No target assigned.");
            return Status.Failure;
        }
        if (minValue == null || maxValue == null)
        {
            LogFailure("No min or max value assigned.");
            return Status.Failure;
        }
        if (minValue.Value == 0 && maxValue.Value == 0)
        {
            LogFailure("No min or max value assigned.");
            return Status.Failure;
        }
        if (minValue.Value > maxValue.Value)
        {
            LogFailure("Min value is greater than max value.");
            return Status.Failure;
        }
        Initialize();   
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        PursueTarget();
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }

    void Initialize()
    {
        if(isInitialized) return;
        navMeshAgent = Agent.Value.GetComponent<NavMeshAgent>();
        GetTargets();
        GetRandomTarget();
        isTargetBeingChosen = false;
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
        Debug.Log($"Found {targets.Length} targets with tag {Tag.Value}");
    }

    void PursueTarget()
    {
        if(targets == null || targets.Length == 0)
        {
            LogFailure("No targets in array to choose.");
            return;
        }
        ChooseTarget();
        Vector3 currentTargetPosition = targets[currentTarget].transform.position;
        Debug.Log($"Pursuing target: {targets[currentTarget].name}");
        navMeshAgent.SetDestination(currentTargetPosition);
    }

    void ChooseTarget()
    {
        if (minValue.Value == 0 && maxValue.Value == 0)
        {
            LogFailure("No min or max value assigned.");
            return;
        }
        if(minValue.Value > maxValue.Value)
        {
            LogFailure("Min value is greater than max value.");
            return;
        }
        time += Time.deltaTime;

        if (!isTargetBeingChosen && currentTime == 0)
        {
            currentTime = GetRandomTime();
            Debug.Log($"Choosing new target in {currentTime} seconds.");
            isTargetBeingChosen = true;
        }
        if (time > 0)
        {
            Debug.Log($"Time ticking.");
        }
        if (isTargetBeingChosen && time >= currentTime)
        {
            currentTarget = GetRandomTarget();
            Debug.Log($"Chose target: {targets[currentTarget].name}");
            time = 0;
            currentTime = 0;
            isTargetBeingChosen = false;
        }
    }

    float GetRandomTime()
    {
        float randomTime = UnityEngine.Random.Range(minValue.Value, maxValue.Value);
        return randomTime;
    }

    int GetRandomTarget()
    {
        int targetCount = targets.Length;
        int randomTarget = UnityEngine.Random.Range(0, targetCount);
        return randomTarget;
    }
}

