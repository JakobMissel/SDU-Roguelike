using UnityEngine;
using System.Collections.Generic;

public class BreathInstance : AbilityInstance {
    void Update() {
        transform.SetPositionAndRotation(SourceAbility.transform.position, Quaternion.LookRotation(SourceAbility.transform.forward));
    }

    void OnTriggerEnter(Collider other) {
        if ((other.gameObject.CompareTag("Enemy") && SourceAbility.PlayerAbility) || 
        (other.gameObject.CompareTag("Player") && !SourceAbility.PlayerAbility)){
            other.gameObject.GetComponent<Health>().TakeDamage(SourceAbility.CalculateDamage());
        }
    }
}
