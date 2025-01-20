using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;
    Vector3 move;
    Camera mainCamera;
    Animator animator;
    [SerializeField] [Tooltip("If this player is the Tamer, check this box.")]bool isTamer;
    
    [Header("Movement")]
    [SerializeField] [Tooltip("The base speed of this player's movement. \nDefault: 8")] public float movementSpeed = 8;
    
    [Header("Rotation")]
    Vector3 targetDirection;
    [SerializeField] [Tooltip("Check this box to rotate this player towards the mouse position, instead of the walking direction")] bool rotateToMouse;
    [SerializeField] [Tooltip("How fast the player will rotate towards their target direction. \nDefault: 750 for walking direction, 2000 for mouse position")] public float rotationSpeed = 750;
    [SerializeField] [Tooltip("**Value only required for non-Tamer Player** Sets the sensitivity requirement before the player rotates player rotation. \nDefault: 0.1")] float gamepadSensitivity = 0.1f;
    
    [Header("Dash")] 
    [HideInInspector] public bool isDashing; //check if the player is dashing, grant damage reduction or something maybe.
    [SerializeField] [Tooltip("The base speed of this player's dash. \nDefault: 20")] public float dashSpeed = 20;
    [SerializeField] [Tooltip("How long the dash lasts. \nDefault: 0.15")] public float dashDuration = 0.15f;
    [SerializeField] [Tooltip("How long the player must wait before they can dash again. \nDefault: 4")] public float dashCooldown = 4;
    [SerializeField] [Tooltip("How many charges can this player perform. \nDefault: 1")] public int maxDashes = 1;
    [HideInInspector] public int availableDashes;
    float currentDashDuration;
    float currentDashCooldown;
    
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        isDashing = false;
        animator = GetComponent<Animator>();
        availableDashes = maxDashes;
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

    //is invoked by Player Input to move the player.
    public void OnMove(InputAction.CallbackContext context)
    {
        //get the players input values
        Vector2 movement = context.ReadValue<Vector2>();
        move = new Vector3(movement.x, 0, movement.y);
    }

    //is invoked by Player Input to rotate the player with the right stick on gamepad.
    public void OnLook(InputAction.CallbackContext context)
    {
        if(isTamer || move != Vector3.zero) return;
        print("look here");
        //get the players input values
        Vector2 direction = context.ReadValue<Vector2>();
        targetDirection = new Vector3(direction.x, 0, direction.y);
        // sensitivity check for gamepad if needed
    }
    
    
    void RotatePlayer()
    {
        // initialize the rotation only when input from movement.
        if (move != Vector3.zero)
        {
            Rotate(move);
        }
        // when the player is not moving, the player rotates towards the mouse position.
        else if (rotateToMouse && isTamer)
        { 
            RotateTowardsMousePosition();
        }
        else if (!isTamer)
        {
            if (targetDirection.magnitude > gamepadSensitivity)
                Rotate(targetDirection);
        }
    }

    void Rotate(Vector3 direction)
    {
        if(direction == Vector3.zero) return;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void RotateTowardsMousePosition()
    {
        // cast ray from mouse position.
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // subtract the position of the player from the raycast of the mouse.
            targetDirection = hit.point - transform.position;
            targetDirection.y = 0;
        }
        Rotate(targetDirection);
    }

    // is invoked by Player Input to make the player dash on input.
    public void OnDash(InputAction.CallbackContext context)
    {
        //check if there are dashes left in the charge.
        if (availableDashes > 0 && !isDashing)
        {
            BeginDash();
        }
    }
    
    void Dash()
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
        
        //performs the dash.
        if (!isDashing) return;
        currentDashDuration -= Time.deltaTime;
        if (currentDashDuration <= 0f)
        {
            EndDash();
        }
        else
        {
            //makes the player dash in the direction they are moving
            //if no movement, dash in the direction they are facing.
            if(move != Vector3.zero)
                characterController.Move(move * (dashSpeed * Time.deltaTime));
            else
                characterController.Move(transform.forward * (dashSpeed * Time.deltaTime));
                
        }
    }
    
    void BeginDash()
    {
        isDashing = true;
        currentDashDuration = dashDuration;
        availableDashes--;
    }

    void EndDash()
    {
        isDashing = false;
    }
}
