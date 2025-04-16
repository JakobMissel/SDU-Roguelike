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
    internal virtual void OnHit(){}
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Enemy") && SourceAbility.PlayerAbility)
        {
            other.gameObject.GetComponent<Health>().TakeDamage(SourceAbility.CalculateDamage());
            OnHit();
        }
        if (other.gameObject.CompareTag("Player") && !SourceAbility.PlayerAbility)
        {
            other.gameObject.GetComponentInParent<Health>().TakeDamage(SourceAbility.CalculateDamage());
            OnHit();
        }
    }
}
