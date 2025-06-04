using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.Mathematics;
public class AbilityInstance : MonoBehaviour {
    [SerializeField] internal Ability SourceAbility;
    internal Vector3 StartPosition;
    internal quaternion StartRotation;

    public void SetInfo(Ability source){
        SourceAbility = source;
        StartPosition = source.transform.position;
        StartRotation = Quaternion.LookRotation(source.transform.forward);
    }
    internal virtual void OnHit(Collider other){}
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Enemy") && SourceAbility.PlayerAbility)
        {
            other.gameObject.GetComponent<Health>().TakeDamage(SourceAbility.CalculateDamage());
            OnHit(other);
        }
        if (other.gameObject.CompareTag("Player") && !SourceAbility.PlayerAbility)
        {
            other.gameObject.GetComponent<DamageTaker>().TakeDamage(SourceAbility.CalculateDamage());
            OnHit(other);
        }
    }
}
