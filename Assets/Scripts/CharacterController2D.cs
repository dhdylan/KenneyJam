using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{

    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _movingThreshold = 1f;
    [SerializeField] private float _jumpForce = 400f;                           // Amount of force added when the player jumps.
    [SerializeField] private float _maxFallingSpeed = 10f;
	[SerializeField] private float _fallingGravityScale = 2f; 
	[SerializeField] private float _jumpApexGravityScale = 0.5f;
	[Tooltip(
		"This determines what counts as the \"Apex\" of a jump " +
		"if the character's Y-axis speed is within +/- of this value " +
		"then the character is considered to be at the \"Apex\" of the jump. " +
		"This is used for applying a different level of gravity when the " +
		"character is at the peak of their jump.")]
	[SerializeField] private float _jumpApexSpeedThreshold = 0.5f;
	[SerializeField] private float _standardGravityScale = 1f;
	[Range(0, 1)] [SerializeField] private float _crouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, 1)][SerializeField] private float _airSpeed = 0.1f;
    [Range(0, .3f)] [SerializeField] private float _movementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool _airControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask _whatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform _groundCheckLocation;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform _ceilingCheckLocation;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D _crouchDisableCollider;				// A collider that will be disabled when crouching

	const float GROUNDED_RADIUS = .1f; // Radius of the overlap circle to determine if grounded
	private bool _grounded;            // Whether or not the player is grounded.
	private bool _isFalling;
	private bool _isHoldingJump;
	const float CEILING_RADIUS = .1f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D _rigidbody2D;
	private bool _facingRight = true;  // For determining which way the player is currently facing.
	private Vector3 _velocity = Vector3.zero;

    private Animator m_CharacterAnimator;

    [Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool _wasCrouching = false;

    private void Awake()
	{
		_rigidbody2D = GetComponent<Rigidbody2D>();
        m_CharacterAnimator = GetComponent<Animator>();

        if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = _grounded;
		_grounded = false;

		// DO GROUND CHECK
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheckLocation.position, GROUNDED_RADIUS, _whatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				_grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}

		// if we're not grounded, then we're in the air
		if(!_grounded)
		{
			_isFalling = _rigidbody2D.linearVelocityY < 0;

			// if jump is still being held, do all this extra math bullshit
			// to make the jump feel good
			if(_isHoldingJump)
            {
                // check if we are near the apex of the jump (y velocity is getting close to 0)
                // remember, if this is true, then the following two blocks of code are not being evaluated
                if (Mathf.Abs(_rigidbody2D.linearVelocityY) < _jumpApexSpeedThreshold)
                {
                    _rigidbody2D.gravityScale = _jumpApexGravityScale;
                }
                // are we falling?
                else if (_isFalling)
                {
                    _rigidbody2D.gravityScale = _fallingGravityScale;
                }
                else // otherwise, we are moving up in the air
                {
                    _rigidbody2D.gravityScale = _standardGravityScale;
                }
            }
			else
			{
				_rigidbody2D.gravityScale = _fallingGravityScale;
			}
		}
		else // otherwise, we're grounded
		{
			_rigidbody2D.gravityScale = _standardGravityScale;
		}

		// limit falling speed if necessary
		if(_rigidbody2D.linearVelocityY < -_maxFallingSpeed)
		{
			_rigidbody2D.linearVelocityY = -_maxFallingSpeed;
		}
	}

    public bool IsGrounded() { return _grounded; }

	/// <summary>
	/// This is intended to be called every frame (fixed update) by some kind of controller object.
	/// Remember: this component is meant to just be a *component* that is utilized
	/// by some other component that *controls* it and tells it what to do.
	/// </summary>
	/// <param name="move">Left/Right movement</param>
	/// <param name="crouch">whether or not the character should be trying to crouch</param>
	/// <param name="jump">whether or not the character should be trying to jump</param>
	public void Move(float move, bool crouch, bool jump, bool isHoldingJump)
	{
		_isHoldingJump = isHoldingJump;

		// If the character is trying to "not crouch", see if they can
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(_ceilingCheckLocation.position, CEILING_RADIUS, _whatIsGround))
			{
				crouch = true;
			}
		}

		// Only control the player if grounded or airControl is turned on
		if (_grounded || _airControl)
		{

			// If crouching
			if (crouch)
			{
				if (!_wasCrouching)
				{
					_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Disable one of the colliders when crouching
				if (_crouchDisableCollider != null)
					_crouchDisableCollider.enabled = false;
			} else
			{
				// Enable the collider when not crouching
				if (_crouchDisableCollider != null)
					_crouchDisableCollider.enabled = true;

				if (_wasCrouching)
				{
					_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

            // Reduce the speed by the crouchSpeed multiplier
			// if we're grounded and we're crouched, take the movement value down to crouching scale
			// if we're grounded and not crouching, leave movement value as is
			// if we're not grounded, then scale the movement value to air speed
            move *= _grounded
				? (_wasCrouching ? _crouchSpeed : 1.0f)
				:  _airSpeed;

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * _moveSpeed, _rigidbody2D.linearVelocity.y);
			// And then smoothing it out and applying it to the character
			_rigidbody2D.linearVelocity = Vector3.SmoothDamp(_rigidbody2D.linearVelocity, targetVelocity, ref _velocity, _movementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !_facingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && _facingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (_grounded && jump)
		{
			// Add a vertical force to the player.
			_grounded = false;
			_rigidbody2D.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Force);
		}

        UpdateAnimatorParameters();
    }

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		_facingRight = !_facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	private void UpdateAnimatorParameters()
	{
        m_CharacterAnimator.SetBool("crouching", _wasCrouching);
        m_CharacterAnimator.SetBool("grounded", _grounded);
        m_CharacterAnimator.SetBool("moving", _rigidbody2D.linearVelocity.magnitude > _movingThreshold);
    }
}
