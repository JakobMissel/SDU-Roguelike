using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;
    Vector3 move;
    PlayerInput playerInput;
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

    DashAbility DashAbility;
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        DashAbility = GetComponent<DashAbility>();
    }

    void OnEnable()
    {
        //subscribe to the player input events.
        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;
        playerInput.actions["Look"].performed += OnLook;
        playerInput.actions["Dash"].started += OnDash;
    }

    void OnDisable()
    {
        //unsubscribe from the player input events.
        playerInput.actions["Move"].performed -= OnMove;
        playerInput.actions["Move"].canceled += OnMove;
        playerInput.actions["Look"].performed -= OnLook;
        playerInput.actions["Dash"].started -= OnDash;
    }

    void Update()
    {
        if (!DashAbility.isDashing)
        {
            Move();
            RotatePlayer();
        }
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

    void OnMove(InputAction.CallbackContext context)
    {
        //get the players input values
        Vector2 movement = context.ReadValue<Vector2>();
        move = new Vector3(movement.x, 0, movement.y);
    }

    /// <summary>
    /// Makes the player look towards the direction of the right joystick.
    /// </summary>
    /// <param name="context"></param>
    void OnLook(InputAction.CallbackContext context)
    {
        if(isTamer || move != Vector3.zero) return;
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

    /// <summary>
    /// Rotates the player towards the direction they are moving.
    /// </summary>
    /// <param name="direction"></param>
    void Rotate(Vector3 direction)
    {
        if(direction == Vector3.zero) return;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Rotates the player towards the mouse position.
    /// </summary>
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

    void OnDash(InputAction.CallbackContext context)
    {
        DashAbility.BeginDash();
        CameraShake.Instance.ShakeCamera();
    }
}
