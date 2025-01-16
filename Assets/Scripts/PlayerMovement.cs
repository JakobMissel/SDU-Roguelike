using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;
    Vector3 move;
    Camera mainCamera;
    Animator animator;
    [SerializeField] [Tooltip("If this player is the Tamer, check this box.")]bool isTamer;
    [SerializeField] [Tooltip("The base speed of this player's movement. Default: 8")] public float movementSpeed = 8;
    Vector3 targetDirection;
    [SerializeField] [Tooltip("How fast the player will rotate towards their target direction. Default: 450")] public float rotationSpeed = 450;
    [SerializeField] [Tooltip("**Value only required for non-Tamer Player** Sets the sensitivity requirement before the player rotates player rotation. Default: 0.1")] float gamepadSensitivity = 0.1f;
    [Header("Dash")] 
    public bool isDashing; //check if the player is dashing, grant damage reduction or something maybe.
    [SerializeField] [Tooltip("The base speed of this player's dash. Default: 30")] public float dashSpeed = 30;
    [SerializeField] [Tooltip("How long the dash lasts. Default: 0.15")] public float dashDuration = 0.15f;
    [SerializeField] [Tooltip("How long the player must wait before they can dash again. Default: 4")] public float dashCooldown = 4;
    float currentDashDuration;
    float lastDashTime;
    
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        isDashing = false;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isDashing)
        {
            Move();
            RotatePlayer();
        }
        Dash();
    }

    void Move()
    {
        if (move != Vector3.zero)
        {
            //feed input values to character controller.
            characterController.Move(move * (movementSpeed * Time.deltaTime));
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    //is invoked by Player Input.
    public void OnMove(InputAction.CallbackContext context)
    {
        //get the players input values
        Vector2 movement = context.ReadValue<Vector2>();
        move = new Vector3(movement.x, 0, movement.y);
    }

    //is invoked by Player Input to move the player.
    public void OnLook(InputAction.CallbackContext context)
    {
        if(isTamer) return;
        
        //get the players input values
        Vector2 direction = context.ReadValue<Vector2>();
        targetDirection = new Vector3(direction.x, 0, direction.y);
        
        // sensitivity check for gamepad if needed
        if (targetDirection.magnitude > gamepadSensitivity)
            targetDirection.Normalize();
    }
    
    
    void RotatePlayer()
    {
        // rotate player depending on if they are "tamer" or not
        if (isTamer)
        {
            RotateKBM();
        }
        // if the player is not "tamer", the OnLook function will be invoked by Player Input
        
        //initialize the rotation only when input.
        if (targetDirection == Vector3.zero) return;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    void RotateKBM()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            targetDirection = hit.point - transform.position;
            targetDirection.y = 0;
        }
    }

    // is invoked by Player Input to make the player dash on input
    public void OnDash(InputAction.CallbackContext context)
    {
        //check if the cooldown is ready
        if (Time.time - lastDashTime >= dashCooldown)
        {
            BeginDash();
        }
    }

    
    void Dash()
    {
        if (!isDashing) return;
        currentDashDuration -= Time.deltaTime;
        if (currentDashDuration <= 0f)
        {
            EndDash();
        }
        else
        {
            characterController.Move(transform.forward * (dashSpeed * Time.deltaTime));
        }

    }
    
    void BeginDash()
    {
        isDashing = true;
        currentDashDuration = dashDuration;
        lastDashTime = Time.time;
    }

    void EndDash()
    {
        isDashing = false;
    }
}
