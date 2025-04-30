using UnityEngine;
using System.Collections.Generic;

public class BreathInstance : AbilityInstance {
    [SerializeField] List<Health> ObjectsInZone;
    [SerializeField] int damageleft;
    int FramesForDamage;
    int frameNumber;
    void Start() {
        FramesForDamage = 10;
        frameNumber = 0;
        damageleft = SourceAbility.Damage;
        ObjectsInZone = new List<Health>();
    }
    void Update() {
        if(SourceAbility == null) return;
        transform.SetPositionAndRotation(SourceAbility.transform.position, Quaternion.LookRotation(SourceAbility.transform.forward));
        if (frameNumber < FramesForDamage) {
            frameNumber++;
        } else {
            frameNumber = 0;
        }
        if (frameNumber == 0) {
            var damageThisFrame = (int)Mathf.Ceil((SourceAbility.CalculateDamage() * Time.deltaTime));
            damageleft -= damageThisFrame;
            foreach (Health item in ObjectsInZone) {
                if (item != null) {
                    item.TakeDamage(damageThisFrame);
                }
            }
        }
        if (damageleft <= 0) {
            Destroy(gameObject);
        }
        // if (ObjectsInZone.Count != 0) {
        //     int damageThisFrame = (int)Mathf.Round((SourceAbility.CalculateDamage() * Time.deltaTime));
        //     damageleft -= damageThisFrame;
        //     foreach (Health item in ObjectsInZone) {
        //         if (item != null) {
        //             item.TakeDamage(damageThisFrame);
        //         }
        //     }
        // }
    }
    void OnTriggerEnter(Collider other){
        ObjectsInZone.Add(other.gameObject.GetComponent<Health>());
    }
    void OnTriggerExit(Collider other){
        ObjectsInZone.Remove(other.gameObject.GetComponent<Health>());
    }
}
