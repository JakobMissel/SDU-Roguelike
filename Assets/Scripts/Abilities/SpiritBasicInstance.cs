using UnityEngine;

public class SpiritBasicInstance : AbilityInstance {
    void Update() {
        transform.SetPositionAndRotation(SourceAbility.transform.position, Quaternion.LookRotation(SourceAbility.transform.forward));    
    }
}
