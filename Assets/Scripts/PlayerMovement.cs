using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController2D))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController2D cc2d;

    private InputSystem_Actions inputActions;

    private Vector2 moveInput;
    private bool jumpInput;
    private bool isCrouching;

    private void Awake()
    {
        cc2d = GetComponent<CharacterController2D>();

        inputActions = new InputSystem_Actions();

        // Register callbacks
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Jump.performed += OnJump; 
        inputActions.Player.Crouch.started += OnCrouchStart;
        inputActions.Player.Crouch.canceled += OnCrouchEnd;
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
        cc2d.Move(moveInput.x, isCrouching, jumpInput);

        // reset jump since it is set to true whenever space is pressed down
        jumpInput = false;
    }

    // ----------- Action Handlers ------------

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Debug.Log($"Move input: {moveInput}");
        
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump triggered!");
        jumpInput = true;
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

    // ----------- Optional Getter ------------

    public Vector2 GetMoveInput() => moveInput;
    public bool IsCrouching() => isCrouching;
}