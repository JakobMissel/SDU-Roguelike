using System;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class PlayerDistanceManager : MonoBehaviour
{
    public static PlayerDistanceManager Instance;
    Health health;
    
    [Header("Player")]
    [SerializeField] [Tooltip("A reference to the midpoint of the two players' position for the camera to look at.")] public GameObject playersMidpointGO;
    [SerializeField] [Tooltip("A reference to the two players' GameObjects.")] GameObject[] players;
    
    [Header("Bond Break Material")]
    [SerializeField] [Tooltip("A reference to the visual effect that serves as a visual warning for players to mind the distance between them.")] Material bondBreakMaterial;
    [SerializeField] [Tooltip("The base of the color. \nDefault: -40")] float baseColor = -40;
    [SerializeField] [Tooltip("The MINIMUM power value to be inserted into the vignette in the shader. \nDefault: 8")] float minVignettePower = 8f;
    [SerializeField] [Tooltip("The MAXIMUM power value to be inserted into the vignette in the shader. \nDefault: 6")] float maxVignettePower = 6f;
    [SerializeField] [Tooltip("The MINIMUM value of the multiplier on the color based on distance. \nDefault: 0.5")] float minColorMultiplier = 0.5f;
    [SerializeField] [Tooltip("The MAXIMUM value of the multiplier on the color based on distance. \nDefault: 10")] float maxColorMultiplier = 10f;
    [SerializeField] [Tooltip("The exponent value that controls the ramping of the visual effect. \nDefault: 3")] int exponent = 3;
    [SerializeField] [Tooltip("The MINIMUM value of the multiplier on the vignette in the shader. \nDefault: 0.15")] float minVignetteMultiplier = .15f;
    [SerializeField] [Tooltip("The MAXIMUM value of the multiplier on the vignette in the shader. \nDefault: 0.2")] float maxVignetteMultiplier = 0.2f;
    
    [Header("Distance")]
    [SerializeField] [Tooltip("The distance at which damage will begin to be taken. \nDefault: 12")] float maxDistance = 12f;
    
    [Header("Damage and Tick rate")]
    [SerializeField] [Tooltip("The amount of damage taken upon distancing to greatly. \nDefault: 5")] float damageTickAmount = 5f;
    [SerializeField] [Tooltip("Check this box if the damage taken value should be in percentages.")] bool isPercentage;
    [SerializeField] [Tooltip("The rate of which damage is taken in seconds. \nDefault: 2")] float currentTickRate = 2f;
    [SerializeField] [Tooltip("The MINIMUM rate of which damage is taken in seconds. \nDefault: 0.5")] float minTickRate = 0.5f;
    [SerializeField] [Tooltip("The rate of which the TICK RATE increases in seconds. \nDefault: 0.1")] float tickRateIntervalIncrease = 0.1f;
    [HideInInspector] public float currentDistance;
    LineRenderer bond;
    float time;
    float maxTickRate;
    bool bondBreaking;
    float currentVignettePower;
    float newVignettePower;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        health = GetComponent<Health>();
        maxTickRate = currentTickRate;
        currentVignettePower = minVignettePower;
        newVignettePower = currentVignettePower;
        bond = GetComponent<LineRenderer>();
    }

    void Update()
    {
        currentDistance = Vector3.Distance(players[0].transform.position, players[1].transform.position);
        //find the midpoint find the midpoint of the players. t = 0.5f since 0 is the pos of value a, and 1 is the pos of value b. 
        Vector3 playersMidpoint = Vector3.Lerp(players[0].transform.position, players[1].transform.position, 0.5f);
        playersMidpointGO.transform.position = playersMidpoint;
        CheckDistance();
        VisualEffect();
        DrawBond();
    }

    void CheckDistance()
    {
        if (currentDistance >= maxDistance)
        {
            time += Time.deltaTime;
            if (!(time >= currentTickRate)) return;
            time = 0;
            TakeDamage();
            //every time the players are too far away from each other increase the frequency of taken damage 
            //do this until the tick rate is the same as the minimum value.
            if(currentTickRate <= minTickRate) return;
            currentTickRate = currentTickRate < minTickRate ? minTickRate : Mathf.Min(currentTickRate, currentTickRate - tickRateIntervalIncrease);
        }
        else
        {
            time += Time.deltaTime;
            //using minTickRate to change the frequency back to normal faster than using the current tick rate
            if (!(time >= minTickRate)) return;
            time = 0;
            //when the players are close, decrease the frequency of damage taken when far away from each other (evening it out)
            //do that until the tick rate is the same as it was initially.
            if(currentTickRate >= maxTickRate) return;
            currentTickRate = currentTickRate > maxTickRate ? maxTickRate : Mathf.Max(currentTickRate, currentTickRate + tickRateIntervalIncrease);
        }
    }

    void TakeDamage()
    {
        if(health.currentHealth <= 0) return;
        if(isPercentage)
            health.TakeDamage(health.maxHealth * (damageTickAmount / 100));
        else
            health.TakeDamage(damageTickAmount);
    }


    void VisualEffect()
    {
        //clamp the distance between the players to a normal.
        float normalDistance = Mathf.Clamp01(currentDistance / maxDistance);
        //make it exponential. 
        float exponentialNDistance = MathF.Pow(normalDistance, exponent);
        float vectorMultiplier = Mathf.SmoothStep(minColorMultiplier, maxColorMultiplier, exponentialNDistance);
        bondBreakMaterial.SetVector("_CurrentColorVector", new Vector3(currentDistance * vectorMultiplier, baseColor,baseColor));
        //clamp the tick rate value to a normal.
        float normalTickRate = Mathf.Clamp01(time / currentTickRate);
        
        if (currentDistance >= maxDistance)
        {
            newVignettePower = Mathf.Lerp(currentVignettePower, maxVignettePower, normalTickRate);
            bondBreakMaterial.SetFloat("_VignettePower", newVignettePower);
            print(newVignettePower);
            if (bondBreaking) return;
            bondBreakMaterial.SetFloat("_VignetteMultiplier", maxVignetteMultiplier);
            bondBreaking = true;
        }
        else if (currentDistance < maxDistance)
        {
            newVignettePower = Mathf.Lerp(currentVignettePower, minVignettePower, normalTickRate);
            bondBreakMaterial.SetFloat("_VignettePower", newVignettePower);
            print(newVignettePower);
            if (!bondBreaking) return;
            bondBreakMaterial.SetFloat("_VignetteMultiplier", minVignetteMultiplier);
            bondBreaking = false;
        }
    }

    void DrawBond()
    {
        bond.SetPosition(0, players[0].transform.position);
        bond.SetPosition(1, players[1].transform.position);
    }
    
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(playersMidpointGO.transform.position, maxDistance/2); //get radius.
    }
}
