using UnityEngine;
using System.Collections.Generic;

public class BreathInstance : AbilityInstance {
    [SerializeField] List<Health> ObjectsInZone;
    [SerializeField] int damageleft;
    void Start() {
        damageleft = SourceAbility.Damage;
        ObjectsInZone = new List<Health>();
    }
    void Update() {
        if(SourceAbility == null) return;
        transform.SetPositionAndRotation(SourceAbility.transform.position, Quaternion.LookRotation(SourceAbility.transform.forward));
        var damageThisFrame = SourceAbility.CalculateDamage() * Time.deltaTime;
        foreach (Health item in ObjectsInZone) {
            if (item != null) {
                item.TakeDamage(damageThisFrame);
            }
        }
        if (damageleft <= 0) {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other){
        ObjectsInZone.Add(other.gameObject.GetComponent<Health>());
    }
    void OnTriggerExit(Collider other){
        ObjectsInZone.Remove(other.gameObject.GetComponent<Health>());
    }
}
