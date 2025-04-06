using UnityEngine;
using System.Collections.Generic;
using System;
public class AbilityInstance : MonoBehaviour {
    [SerializeField] internal Ability SourceAbility;
    internal Vector3 StartPosition;

    public void SetInfo(Ability source){
        SourceAbility = source;
        StartPosition = source.transform.position;
    }
}
