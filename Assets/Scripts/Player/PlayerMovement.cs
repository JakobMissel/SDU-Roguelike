using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    [HideInInspector] public Vector3 move;
    PlayerInput playerInput;
    Camera mainCamera;
    Animator animator;
    [SerializeField] [Tooltip("If this player uses keyboard, check this box.")] bool usesKeyboard;
    
    [Header("Movement")]
    [SerializeField] [Tooltip("The acceleration of this player's movement. \nDefault: 100")] public float acceleration = 100;
    [SerializeField] [Tooltip("The max speed of this player's movement. \nDefault: 4")] public float maxMovementSpeed = 4;

    [Header("Rotation")]
    Vector3 targetDirection;
    [SerializeField] [Tooltip("Check this box to rotate this player towards the mouse position, instead of the walking direction")] bool rotateToMouse;
    [SerializeField] [Tooltip("How fast the player will rotate towards their target direction. \nDefault: 750 for walking direction, 2000 for mouse position")] public float rotationSpeed = 750;
    [SerializeField] [Tooltip("**Value only required for non-Tamer Player** Sets the sensitivity requirement before the player rotates player rotation. \nDefault: 0.2")] float gamepadSensitivity = 0.2f;
    
    DashAbility DashAbility;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        mainCamera = Camera.main;
        animator = GetComponentInChildren<Animator>();
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
        playerInput.actions["Dash"].canceled -= OnDash;
    }

    void Update()
    {
        if (!DashAbility.isDashing)
        {
            RotatePlayer();
        }
    }

    void FixedUpdate()
    {
        if (!DashAbility.isDashing)
        { 
            Move();
        }
    }

    /// <summary>
    /// Moves the player in the direction of the input values.
    /// </summary>
    void Move()
    {
        if (move != Vector3.zero)
        {
            //feed input values to character controller.
            rb.AddForce(move * (acceleration * Time.deltaTime), ForceMode.VelocityChange);
            if(!DashAbility.isDashing)
                rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxMovementSpeed);
            else
                rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, DashAbility.speed);
            //rigidbody.MovePosition(transform.position + move * (movementSpeed * Time.deltaTime));
            animator.SetBool("isWalking", true);
        }
        else
        {
            rb.linearVelocity = new(0, rb.linearVelocity.y, 0);
            animator.SetBool("isWalking", false);
        }
    }

    /// <summary>
    /// Gets the players input values from the left joystick or WASD keys.
    /// </summary>
    /// <param name="context"></param>
    void OnMove(InputAction.CallbackContext context)
    {
        //get the players input values
        Vector2 movement = context.ReadValue<Vector2>();
        move = new Vector3(movement.x, 0, movement.y).normalized;
    }

    /// <summary>
    /// Makes the player look towards the direction of the right joystick.
    /// </summary>
    /// <param name="context"></param>
    void OnLook(InputAction.CallbackContext context)
    {
        if(usesKeyboard) return;
        //get the players input values
        Vector2 direction = context.ReadValue<Vector2>();
        targetDirection = new Vector3(direction.x, 0, direction.y).normalized;
        // sensitivity check for gamepad if needed
    }

    /// <summary>
    /// Rotates the player towards the direction they are moving or towards the mouse position.
    /// </summary>
    void RotatePlayer()
    {
        if (!usesKeyboard && targetDirection.magnitude > gamepadSensitivity)
        {
            Rotate(targetDirection);
        }
        // initialize the rotation only when input from movement.
        if (move != Vector3.zero)
        {
            //Rotate(move);
        }
        // when the player is not moving, the player rotates towards the mouse position.
        if (rotateToMouse && usesKeyboard)
        { 
            RotateTowardsMousePosition();
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
        if(DashAbility != null)
            DashAbility.BeginDash();
    }
}
