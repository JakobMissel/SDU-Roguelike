using System.Collections;
using UnityEngine;

public class DashAbility : MonoBehaviour
{
    CharacterController characterController;
    
    Vector3 move;

    [Header("Dash")]
    [HideInInspector] public bool isDashing; //check if the player is dashing, grant damage reduction or something maybe.
    [SerializeField][Tooltip("The base speed of the entity's dash. \nDefault: 20")] public float dashSpeed = 20;
    [SerializeField][Tooltip("How long the dash lasts. \nDefault: 0.15")] public float dashDuration = 0.15f;
    [SerializeField][Tooltip("How long the entity must wait before they can dash again. \nDefault: 4")] public float dashCooldown = 4;
    [SerializeField][Tooltip("How many charges of dash this entity has. \nDefault: 1")] public int maxDashes = 1;
    [HideInInspector] public int availableDashes;
    float currentDashDuration;
    float currentDashCooldown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        availableDashes = maxDashes;
        if(gameObject.GetComponent<CharacterController>() != null)
            characterController = gameObject.GetComponent<CharacterController>();
        
    }

    public void CheckCooldown()
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

    public void BeginDash()
    {
        //check if there are dashes left in the charge.
        if (availableDashes > 0 && !isDashing)
        {
            isDashing = true;
            currentDashDuration = dashDuration;
            availableDashes--;
            print(availableDashes);
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        //performs the dash.
        while (isDashing)
        {
            currentDashDuration -= Time.deltaTime;
            print("is dashing");
            if (currentDashDuration <= 0f)
            {
                EndDash();
            }
            else
            {
                //makes the player dash in the direction they are moving
                //if no movement, dash in the direction they are facing.
                if (move != Vector3.zero)
                    characterController.Move(move * (dashSpeed * Time.deltaTime));
                else
                    characterController.Move(transform.forward * (dashSpeed * Time.deltaTime));
            }
            yield return null;
        }
    }

    void EndDash()
    {
        isDashing = false;
        StopCoroutine(Dash());
    }
}
