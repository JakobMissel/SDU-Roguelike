using System.Collections;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField][Tooltip("The base speed of the entity's dash. \nDefault: 20")] public float speed = 20;
    [SerializeField][Tooltip("How long the dash lasts. \nDefault: 0.15")] public float duration = 0.15f;
    [SerializeField][Tooltip("UI Image that visualizes the remaining cooldown.")] public Image cooldownImage;
    [SerializeField][Tooltip("How long the entity must wait before they can dash again. \nDefault: 4")] public float cooldown = 4;
    [SerializeField][Tooltip("Dash charge image prefab that will be instantiated as charges are gained.")] public GameObject dashChargeImagePrefab;
    [SerializeField][Tooltip("Dash charge visualizer parent.")] public GridLayoutGroup maxCharges;
    [SerializeField][Tooltip("How many charges of dash this entity has. \nDefault: 1")] public int maxDashes = 1;
    [SerializeField][Tooltip("Is this attached to a player entity?")] public bool isPlayer;
    [HideInInspector] public int availableDashes;
    float currentDuration;
    float currentCooldown;
    
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
        for (int i = 0; i < maxDashes; i++)
        {
            SpawnVisualDashCharge();
        }
        if (cooldownImage != null)
        {
            cooldownImage.fillAmount = Mathf.Clamp01(currentCooldown / cooldown);
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
            if(currentCooldown == 0)
                currentCooldown = cooldown;
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0)
            {
                availableDashes++;
                UpdateDashChargeVisual(false);
                currentCooldown = 0f;
            }
            if (cooldownImage != null)
            {
                cooldownImage.fillAmount = Mathf.Clamp01(currentCooldown / cooldown);
            }
        }
    }

    public void UpdateDashChargeVisual(bool removeCharge)
    {
        if(maxCharges == null) return;
        for (int i = 0; i < maxDashes; i++)
        {
            if (removeCharge)
            {
                var charge = maxCharges.transform.GetChild(i).transform.GetChild(0).GetComponent<Image>();
                if (charge.color == Color.white)
                {
                    charge.color = Color.clear;
                    break;
                }
            }
            else
            {
                var charge = maxCharges.transform.GetChild((maxCharges.transform.childCount - 1) - i).transform.GetChild(0).GetComponent<Image>();
                if (charge.color == Color.clear)
                {
                    charge.color = Color.white;
                    break;
                }
            }
        }
    }
    public void SpawnVisualDashCharge()
    {
        if (maxCharges == null) return;
        var dashChargeImage = Instantiate(dashChargeImagePrefab, maxCharges.transform);
    }

    public void GainCharge()
    {
        availableDashes++;
        SpawnVisualDashCharge();
    }

    public void BeginDash()
    {
        if (GameEvents.GameIsOver) return;
        //check if there are dashes left in the charge.
        if (availableDashes > 0 && !isDashing)
        {
            isDashing = true;
            currentDuration = duration;
            availableDashes--;
            UpdateDashChargeVisual(true);
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        //performs the dash.
        while (isDashing)
        {
            currentDuration -= Time.deltaTime;
            if (currentDuration <= 0f)
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
                        rb.AddForce(playerMovement.move * (10 * speed * Time.deltaTime), ForceMode.VelocityChange);
                    }
                    else
                        rb.AddForce(transform.forward * (10 * speed * Time.deltaTime), ForceMode.VelocityChange);
                }
                else
                {
                    navMeshAgent.Move(transform.forward * (speed * Time.deltaTime));
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
