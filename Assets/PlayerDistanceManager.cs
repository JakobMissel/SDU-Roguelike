using System;
using UnityEngine;

public class PlayerDistanceManager : MonoBehaviour
{
    public static PlayerDistanceManager Instance;
    Health health;
    [SerializeField] [Tooltip("A reference to the midpoint of the two players' position for the camera to look at.")]public GameObject playersMidpointGO;
    [SerializeField] [Tooltip("A reference to the flashing screen that serves as a visual warning for players to mind the distance between them.")] GameObject distanceWarning;
    [SerializeField] [Tooltip("A reference to the two players' GameObjects.")] GameObject[] players;
    [SerializeField] [Tooltip("The distance at which damage will begin to be taken. \nDefault: 12")] float maxDistance = 12f;
    [SerializeField] [Tooltip("Check this box if the damage taken value should be in percentages.")] bool isPercentage;
    [SerializeField] [Tooltip("The amount of damage taken upon distancing to greatly. \nDefault: 5")] float damageTickAmount = 5f;
    [SerializeField] [Tooltip("The rate of which damage is taken in seconds. \nDefault: 2")] float tickRateInterval = 2f;
    [SerializeField] [Tooltip("The MINIMUM rate of which damage is taken in seconds. \nDefault: 0.5")] float minTickRateInterval = 0.5f;
    [SerializeField] [Tooltip("The rate of which the TICK RATE increases in seconds. \nDefault: 0.1")] float tickRateIntervalIncrease = 0.1f;
    [HideInInspector] public float currentDistance;
    float time;
    float maxTickRateInterval;
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        health = GetComponent<Health>();
        distanceWarning.SetActive(false);
        maxTickRateInterval = tickRateInterval;
    }

    void Update()
    {
        currentDistance = Vector3.Distance(players[0].transform.position, players[1].transform.position);
        print("distance between players: " + currentDistance);
        //find the midpoint find the midpoint of the players. t = 0.5f since 0 is the pos of value a, and 1 is the pos of value b. 
        Vector3 playersMidpoint = Vector3.Lerp(players[0].transform.position, players[1].transform.position, 0.5f);
        playersMidpointGO.transform.position = playersMidpoint;
        CheckDistance();
    }

    void CheckDistance()
    {
        if (currentDistance >= maxDistance)
        {
            distanceWarning.SetActive(true);
            time += Time.deltaTime;
            if (!(time >= tickRateInterval)) return;
            time = 0;
            TakeDamage();
            //every time the players are too far away from each other increase the frequency of taken damage 
            //do this until the tick rate is the same as the minimum value.
            if(tickRateInterval <= minTickRateInterval) return;
            tickRateInterval = tickRateInterval < minTickRateInterval ? minTickRateInterval : Mathf.Min(tickRateInterval, tickRateInterval - tickRateIntervalIncrease);
        }
        else
        {
            distanceWarning.SetActive(false);
            time += Time.deltaTime;
            if (!(time >= tickRateInterval)) return;
            time = 0;
            //when the players are close, decrease the frequency of damage taken when far away from each other (evening it out)
            //do that until the tick rate is the same as it was initially.
            if(tickRateInterval >= maxTickRateInterval) return;
            tickRateInterval = tickRateInterval > maxTickRateInterval ? maxTickRateInterval : Mathf.Max(tickRateInterval, tickRateInterval + tickRateIntervalIncrease);
        }
    }

    void TakeDamage()
    {
        if(isPercentage)
            health.TakeDamage(health.maxHealth * (damageTickAmount / 100));
        else
            health.TakeDamage(damageTickAmount);
    }
}
