using UnityEngine;

public class SpiritBasicInstance : AbilityInstance {
    void Update() {
        if(SourceAbility == null) return;
        transform.SetPositionAndRotation(SourceAbility.transform.position, Quaternion.LookRotation(SourceAbility.transform.forward));    
    }
}
