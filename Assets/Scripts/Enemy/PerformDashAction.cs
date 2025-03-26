using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PerformDash", story: "[Agent] performs [Dash]", category: "Action", id: "c53eac7ef076518838cd87f66b608c9a")]
public partial class PerformDashAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<DashAbility> Dash;

    protected override Status OnStart()
    {
        Dash.Value.BeginDash();
        return Status.Running;
    }
}

