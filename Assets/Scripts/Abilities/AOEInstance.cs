using UnityEngine;
using System.Collections.Generic;
public class AOEInstance : AbilityInstance {
    List<Health> ObjectsInZone;
    void Start() {
        ObjectsInZone = new List<Health>();
    }

    void Update() {

    // if (ObjectsInZone.Count != 0) {
    //     foreach (Health item in ObjectsInZone) {
            
    //     }
    // }

    }

    void OnTriggerEnter(Collider other) {
        if ((other.gameObject.CompareTag("Enemy") && SourceAbility.PlayerAbility) || 
        (other.gameObject.CompareTag("Player") && !SourceAbility.PlayerAbility)){
            other.gameObject.GetComponent<Health>().TakeDamage(SourceAbility.CalculateDamage());
        }
        // if (other.gameObject.CompareTag("Player") && !SourceAbility.PlayerAbility){
        //     other.gameObject.GetComponent<Health>().TakeDamage(SourceAbility.CalculateDamage());
        //     Destroy(this);
        // }
        // switch(other.gameObject.tag){
        //     case "Enemy":
        //     break;
        //     case "Player":
        //     break;
        // }
    }
}
