using System;
using UnityEngine;

public class PlayerDistanceManager : MonoBehaviour
{
    Health health;
    [SerializeField] [Tooltip("A reference to the flashing screen that serves as a visual warning for players to mind the distance between them.")] GameObject distanceWarning;
    [SerializeField] [Tooltip("A reference to the two players' GameObjects.")] GameObject[] players;
    [SerializeField] [Tooltip("The distance at which damage will begin to be taken. \nDefault: 12")] float maxDistance = 12f;
    [SerializeField] [Tooltip("The amount of damage taken upon distancing to greatly. \nDefault: 5")] float damageTickAmount = 5f;
    [SerializeField] [Tooltip("The rate of which damage is taken in seconds. \nDefault: 2")] float tickRateInterval = 2f;
    [Header("NOT IMPLEMENTED")]
    [SerializeField] [Tooltip("The MINIMUM rate of which damage is taken in seconds. \nDefault: 0.5")] float minTickRateInterval = 0.5f;
    [SerializeField] [Tooltip("The rate of which the TICK RATE increases in seconds. \nDefault: 0.1")] float tickRateIntervalIncrease = 0.1f;

    float time;

    void Awake()
    {
        health = GetComponent<Health>();
        distanceWarning.SetActive(false);
    }

    void Update()
    {
        float distance = Vector3.Distance(players[0].transform.position, players[1].transform.position);
        print(distance);

        if (distance >= maxDistance)
        {
            distanceWarning.SetActive(true);
            time += Time.deltaTime;
            if (!(time >= tickRateInterval)) return;
            health.TakeDamage(damageTickAmount);
            time = 0;
        }
        else
        {
            distanceWarning.SetActive(false);
        }
    }
}
