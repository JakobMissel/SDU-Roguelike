using UnityEngine;
using System.Collections.Generic;

public class BreathInstance : AbilityInstance {
    void Update() {
        if(SourceAbility == null) return;
        transform.SetPositionAndRotation(SourceAbility.transform.position, Quaternion.LookRotation(SourceAbility.transform.forward));
    }
}
