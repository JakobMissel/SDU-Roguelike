using UnityEngine;
using System.Collections.Generic;

public class BreathInstance : AbilityInstance {
    void Update() {
        transform.SetPositionAndRotation(SourceAbility.transform.position, Quaternion.LookRotation(SourceAbility.transform.forward));
    }
}
