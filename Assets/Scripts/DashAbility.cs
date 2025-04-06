using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DashAbility : MonoBehaviour
{
    //Player stuff.
    Rigidbody rb;
    PlayerMovement playerMovement;

    //AI stuff.
    NavMeshAgent navMeshAgent;
    float initialAcceleration;
    float initialSpeed;

    [Header("Dash")]
    [HideInInspector] public bool isDashing; //check if the player is dashing, grant damage reduction or something maybe.
    [SerializeField][Tooltip("The base speed of the entity's dash. \nDefault: 20")] public float dashSpeed = 20;
    [SerializeField][Tooltip("How long the dash lasts. \nDefault: 0.15")] public float dashDuration = 0.15f;
    [SerializeField][Tooltip("How long the entity must wait before they can dash again. \nDefault: 4")] public float dashCooldown = 4;
    [SerializeField][Tooltip("How many charges of dash this entity has. \nDefault: 1")] public int maxDashes = 1;
    [SerializeField][Tooltip("Is this attached to a player entity?")] public bool isPlayer;
    [HideInInspector] public int availableDashes;
    float currentDashDuration;
    float currentDashCooldown;
    
    void Awake()
    {
        availableDashes = maxDashes;
        if (GetComponent<Rigidbody>() != null)
            rb = GetComponent<Rigidbody>();
        if(GetComponent<PlayerMovement>() != null)
            playerMovement = GetComponent<PlayerMovement>();
        if (GetComponent<NavMeshAgent>() != null)
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            initialAcceleration = navMeshAgent.acceleration;
            initialSpeed = navMeshAgent.speed;
        }

    }

    void Update()
    {
        CheckCooldown();
    }

    void CheckCooldown()
    {
        //checking and resetting dash cooldown.
        if (availableDashes < maxDashes)
        {
            currentDashCooldown += Time.deltaTime;
            if (currentDashCooldown >= dashCooldown)
            {
                availableDashes++;
                currentDashCooldown = 0f;
            }
        }
    }

    public void GainCharge()
    {
        availableDashes++;
    }

    public void BeginDash()
    {
        //check if there are dashes left in the charge.
        if (availableDashes > 0 && !isDashing)
        {
            isDashing = true;
            currentDashDuration = dashDuration;
            availableDashes--;
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        print($"{gameObject.name} is dashing");
        //performs the dash.
        while (isDashing)
        {
            currentDashDuration -= Time.deltaTime;
            if (currentDashDuration <= 0f)
            {
                EndDash();
            }
            else
            {
                //makes the player dash in the direction they are moving
                //if no movement, dash in the direction they are facing.
                if(isPlayer)
                {
                    if (playerMovement.move != Vector3.zero)
                    {
                        rb.MovePosition(transform.position + playerMovement.move * (dashSpeed * Time.deltaTime));
                    }
                    else
                        rb.MovePosition(transform.position + transform.forward * (dashSpeed * Time.deltaTime));
                }
                else
                {
                    navMeshAgent.Move(transform.forward * (dashSpeed * Time.deltaTime));
                }
            }
            yield return null;
        }
    }

    void EndDash()
    {
        isDashing = false;
        if (navMeshAgent)
        {
            navMeshAgent.acceleration = initialAcceleration;
            navMeshAgent.speed = initialSpeed;
        }
        StopCoroutine(Dash());
    }
}
