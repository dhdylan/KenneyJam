using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MovementController2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerCharacter player;

    private MovementController2D playerMovementController;

    private InputSystem_Actions inputActions;

    private Vector2 moveInput;
    private bool jumpInput;
    private bool _jumpPressedThisFrame;
    private bool isCrouching;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();

        // Register callbacks
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Jump.started += OnJump;
        inputActions.Player.Jump.canceled += OnJump; 
        inputActions.Player.Crouch.started += OnCrouchStart;
        inputActions.Player.Crouch.canceled += OnCrouchEnd;

        inputActions.Player.Attack.started += OnAttack;
    }

    private void Start()
    {
        playerMovementController = player.movementController2D;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    // -------- Call things on update ---------

    private void FixedUpdate()
    {
        playerMovementController.Move(moveInput.x, isCrouching, _jumpPressedThisFrame, jumpInput);
        _jumpPressedThisFrame = false;
    }

    // ----------- Action Handlers ------------

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log($"Move input: {moveInput}");
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log($"Jump triggered with {context.phase}");

        if(context.started)
        {
            _jumpPressedThisFrame = true;
            jumpInput = true;
        }

        if(context.canceled)
        {
            jumpInput = false;
        }
    }

    private void OnCrouchStart(InputAction.CallbackContext context)
    {
        isCrouching = true;
        Debug.Log("Crouch started");
    }

    private void OnCrouchEnd(InputAction.CallbackContext context)
    {
        isCrouching = false;
        Debug.Log("Crouch ended");
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        player.BasicAttack();
    }

    // ----------- Optional Getter ------------

    public Vector2 GetMoveInput() => moveInput;
    public bool IsCrouching() => isCrouching;
}