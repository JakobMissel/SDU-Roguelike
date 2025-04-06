using UnityEngine;
using System.Collections.Generic;
using System;
public class AbilityInstance : MonoBehaviour {
    internal Ability SourceAbility;
    void Start() {
    }

    public void SetInfo(Ability source){
        SourceAbility = source;
    }

    // Update is called once per frame
    void Update() {
        
    }
}
