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
            currentCooldown -= Time.deltaTime;
            if (cooldownImage != null)
            {
                cooldownImage.fillAmount = Mathf.Clamp01(currentCooldown / cooldown);
            }
            if (currentCooldown >= cooldown)
            {
                availableDashes++;
                UpdateDashChargeVisual(false);
                currentCooldown = 0f;
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
                if (maxCharges.transform.GetChild(i).gameObject.GetComponent<Image>().color == Color.white)
                {
                    maxCharges.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.clear;
                    break;
                }
            }
            else if (!removeCharge)
            {
                if (maxCharges.transform.GetChild(i).gameObject.GetComponent<Image>().color == Color.clear)
                {
                    maxCharges.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.white;
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
        //check if there are dashes left in the charge.
        if (availableDashes > 0 && !isDashing)
        {
            isDashing = true;
            currentDuration = duration;
            currentCooldown = cooldown;
            availableDashes--;
            UpdateDashChargeVisual(true);
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        print($"{gameObject.name} is dashing");
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
                        rb.MovePosition(transform.position + playerMovement.move * (speed * Time.deltaTime));
                    }
                    else
                        rb.MovePosition(transform.position + transform.forward * (speed * Time.deltaTime));
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
