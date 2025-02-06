using UnityEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public enum Transition
{
    None = 0,
    SawPlayer,
    ReachedPlayer,
    LostPlayer,
    NoHealth,
}

public enum FSMStateID
{
    None = 0,
    Patrolling,
    Chasing,
    Combat,
    Dead,
}


public class AdvancedFSM : FSM
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
