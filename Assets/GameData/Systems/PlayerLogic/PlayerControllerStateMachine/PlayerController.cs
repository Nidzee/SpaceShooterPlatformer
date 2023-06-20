using LivingBeings.Player.CharacterMovement.MovementStateMachine;
using LivingBeings.Player.CharacterMovement.MovementStateMachine.States;
using UI;
using UnityEngine;

public class PlayerController : AliveUnit
{

    [Header("Player Health-Armour config")]
    [SerializeField] float _healthAmount;
    [SerializeField] float _armourAmount;
    

    [Header("Controll Buttons")]
    [SerializeField] CustomButton _leftMoveButton = null;
    [SerializeField] CustomButton _rightMoveButton = null;
    [SerializeField] CustomButton _upMoveButton = null;



    public Transform VisualsContainer;

    
    [Header("Weapon")]
    [SerializeField] Weapon _weapon;

    [Header("Handlers")]
    [SerializeField] GrenadeHandler _grenadeHandler;
    public GrenadeHandler GrenadeHandler => _grenadeHandler;
    [SerializeField] PlayerItemCollectorHandler _itemCollectHandler;
    public PlayerItemCollectorHandler ItemCollectorHandler => _itemCollectHandler;
    [SerializeField] PlayerInteractionHandler _interractionHandler;
    public PlayerInteractionHandler InterractionHandler => _interractionHandler;
    [SerializeField] PlayerEffectsHandler _playerEffectsHandler;
    public PlayerEffectsHandler PlayerEffectsHandler => _playerEffectsHandler;



    [Header("Horizontal movement config")]
    [SerializeField] float _horizontalSpeed = 0f;                     // Character`s horizontal speed.
    [Range(0f, 0.4f)] [SerializeField] float _movementSmoothing = 0f;                   // Coefficient of character`s horizontal movement smoothing.
    
    

    [Header("Jump control")]
    [SerializeField] float _jumpHeight = 0f;                          // Character`s jump height.
    [SerializeField] float _afterGroundTouchJumpTime = 0f;            // Time during which character still can jump after he touch ground last time.
    [SerializeField] float _pressBeforeGroundTime = 0f;               // Time during which character can perform jump after he press jump button last time.
    [Range(0f, 1f), SerializeField] float _cutJumpHeight = 0;         // Coefficient that determine how much jump will be cut, after player relesase jump button.


    [Header("Ladder movement control")]
    [SerializeField] float _climbUpSpeed = 0f;                       // Climb up speed.
    [SerializeField] float _climbDownSpeed = 0f;                      // Climb down speed.


    [Header("Is on ground control")]
    [SerializeField] SurfaceCheck _surfaceCheck = null;               // Script that check if character is on ground.



    // Fall information.
    Vector2 _startFallingPosition = Vector2.zero;                     // Used to calculate last fall height, that`s needed to play appropriate landing dust animation.
    Vector2 _landingPosition = Vector2.zero;                          // Used to calculate last fall height, that`s needed to play appropriate landing dust animation.
    float _lastFallHeight = 0f;                                       // Used to play appropriate landing dust animation.
    

    // Movement states.
    StateMachine _stateMachine = null;                                // Script that control transition between states.
    JumpingState _jumpingState = null;
    IdleState _idleState = null;
    RunningState _runningState = null;
    FallingState _fallingState = null;
    LandingState _landingState = null;
    ClimbingState _climbingState = null;


    //Character`s components.
    Transform _transform = null;                                      // Characters Transform component.
    Rigidbody2D _rigidBody = null;                                    // Characters Rigidbody2D component.
    bool _isFacingRight = false;                                      // Determine to which side the character is facing now. 

    // Timers.
    float _afterGroundTouchTimer = 0;                                 // Timer that controls time during which character still can jump after he touch ground last time.
    float _pressButtonTimer = 0;                                      // Timer that controls time during which character can perform jump after he press jump button last time.



    #region Properties
    public float HorizontalSpeed => _horizontalSpeed;

    public float MovementSmoothing => _movementSmoothing;

    public CustomButton LeftMoveButton => _leftMoveButton;

    public CustomButton RightMoveButton => _rightMoveButton;

    public bool IsFacingRight
    {
        get => _isFacingRight;
        set => _isFacingRight = value;
    }


    public float JumpHeight => _jumpHeight;

    public float AfterGroundTouchJumpTime => _afterGroundTouchJumpTime;

    public float PressBeforeGroundTime => _pressBeforeGroundTime;

    public float CutJumpHeight => _cutJumpHeight;

    public CustomButton UpMoveButton => _upMoveButton;

    public float  ClimbUpSpeed => _climbUpSpeed;

    public float ClimbDownSpeed => _climbDownSpeed;

    public SurfaceCheck SurfaceCheck => _surfaceCheck;


    // States references
    public StateMachine StateMachine => _stateMachine;
    public JumpingState Jumping => _jumpingState;
    public IdleState Idle => _idleState;
    public RunningState Running => _runningState;
    public FallingState Falling => _fallingState;
    public LandingState Landing => _landingState;
    public ClimbingState Climbing => _climbingState;



    public Transform Transform => _transform;
    public Rigidbody2D RigidBody => _rigidBody;

    public float AfterGroundTouchTimer
    {
        get => _afterGroundTouchTimer;

        set => _afterGroundTouchTimer = value;
    }

    public float PressButtonTimer
    {
        get => _pressButtonTimer;
        set => _pressButtonTimer = value;
    }

    public Vector2 StartFallingPosition
    {
        get => _startFallingPosition;
        set => _startFallingPosition = value;
    }

    public Vector2 LandingPosition
    {
        get => _landingPosition;
        set => _landingPosition = value;
    }

    public float LastFallHeight
    {
        get => _lastFallHeight;
        set => _lastFallHeight = value;
    }

    #endregion Properties





    
    private void Awake()
    {
        // Initialize character`s components.
        _transform = transform;
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();


        _weapon.SetGunStats();
        _playerEffectsHandler.Reset();
        _itemCollectHandler.Reset();
        _interractionHandler.Reset();

        Health = _healthAmount;
        Armour = _armourAmount;


        // Initialize state machine and states.
        _stateMachine = new StateMachine();
        _jumpingState = new JumpingState(this, _stateMachine);
        _idleState = new IdleState(this, _stateMachine);    
        _runningState = new RunningState(this, _stateMachine);
        _fallingState = new FallingState(this, _stateMachine);
        _landingState = new LandingState(this, _stateMachine);
        _climbingState = new ClimbingState(this, _stateMachine);
    }


    private void Start()
    {
        _isFacingRight = Mathf.Approximately(transform.rotation.y, 0f) ? true : false;
        
        // Set first state.
        _stateMachine.Initialization(Idle);
    }


    public bool IsRightButtonPressed;
    public bool IsLeftButtonPressed;
    public bool IsJumpButtonPressed;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _weapon.StartShootingContinuesly();
        } 
        
        else if (Input.GetMouseButtonUp(0))
        {
            _weapon.StopShootingContinuesly();
        }

        if (Input.GetKey(KeyCode.D))
        {
            IsRightButtonPressed = true;
        } 
        else if (Input.GetKeyUp(KeyCode.D))
        {
            IsRightButtonPressed = false;
        }
        

        if (Input.GetKey(KeyCode.A))
        {
            IsLeftButtonPressed = true;
        } 
        else if (Input.GetKeyUp(KeyCode.A))
        {
            IsLeftButtonPressed = false;
        }

        
        if (Input.GetKey(KeyCode.Space))
        {
            IsJumpButtonPressed = true;
        } 
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            IsJumpButtonPressed = false;
        }


        if (Input.GetKeyDown(KeyCode.G))
        {
            _grenadeHandler.StartForceSet();
        } 
        
        else if (Input.GetKeyUp(KeyCode.G))
        {
            _grenadeHandler.ReleaseForceSet();
        }


        // Try to interract
        if (Input.GetKeyDown(KeyCode.E))
        {
            _interractionHandler.TryToInteract();
        }
    }




    // Update physics
    private void FixedUpdate()
    {
        _stateMachine.CurrentState.PhysicsUpdate();
    }




    
    // Collision logic
    private void OnTriggerEnter2D(Collider2D other)
    {
        _stateMachine.CurrentState.OnTriggerEnter2D(other);    
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _stateMachine.CurrentState.OnTriggerExit2D(other);
    }


    public override void TakeDamage(float damagePoints)
    {
        Debug.Log("[###] PLAYER TAKE DAMAGE: " + damagePoints);

        if (Armour > 0)
        {
            if (damagePoints > Armour)
            {
                float healthReduce = damagePoints - Armour;
                Armour = 0;
                Health -= healthReduce;
            }
            else
            {
                Armour -= damagePoints;
            }
        }
        else
        {
            Health -= damagePoints;
        }

        
        Debug.Log("[###] PLAYER HEALTH: " + Health);
        Debug.Log("[###] PLAYER ARMOUR: " + Armour);

        if (Health <= 0)
        {
            Die();
        }

    }

    public override void Die()
    {
        base.Die();
        Debug.Log("[### PLAYER IS DEAD");
    }
}