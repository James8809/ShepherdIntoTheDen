using System.Collections;
using System.Collections.Generic;
using Crafting;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using Image = UnityEngine.UI.Image;

public class PlayerController : MonoBehaviour
{
    // state machines
    [HideInInspector] public PlayerState currentState;
    [HideInInspector] public RunningState runningState;
    [HideInInspector] public DashState dashState;
    [HideInInspector] public AttackState attackState;
    [HideInInspector] public AbilityState abilityState;
    [HideInInspector] public ConsumableState currConsumable;
    [HideInInspector] public DeathState deathState;

    [HideInInspector] public CharacterController _characterController;
    public PlayerReferenceManager ReferenceManager;
    [HideInInspector] public PlayerInput _playerInput;
    [HideInInspector] public Animator _animator;
    [HideInInspector] public HerdController herdController;
    
    [HideInInspector] public int speedHash, isDashingHash, attackTriggerHash, walkCancelHash, stompHash;

    [SerializeField] public float speed = 5.0f;
    public float gravity = 9.82f;
    [HideInInspector] public float downwardsMovement = 0.0f;
    [Range(0, 1)][SerializeField] public float attackMovementModifier = .273f;
    [SerializeField] public float dashDuration = .11f;
    [SerializeField] public float dashDistance = 1.8f;
    [SerializeField] private float dashCooldown = .1f;
    [Range(0, 1)][SerializeField] public float dashSlerpFactor = .2f;
    [Range(0, 90)] public float maxTotalDashYRotation = 90.0f;
    public float rotationsPerSecond = 20.0f;
    [HideInInspector] public float speedMultiplier = 1.0f;

    // attack values
    [Range(0, 1)] public float meleeMoveFactor = .273f;
    [Range(0, 1)] public float meleeRotationFactor = 0.0f;
    
    [Range(0, 1)] public float rangedMoveFactor = .0f;
    [Range(0, 1)] public float rangedRotationFactor = 0.8f;

    // variables to be used by states (weird design due to interrupt input)
    [HideInInspector] public Vector3 currentMovement;   // normalized world dir
    [HideInInspector] public float timeSinceLastDash = 100.0f;  // in seconds (set high for first dash)

    // isometric directions
    [HideInInspector] public Vector3 forward, right;

    // Bow combat data
    public GameObject arrowObject;  // needs to be set in inspector, prefab will handle variety + diff vfx
    public float shootForce = 500.0f;
    public Transform arrowSpawn;
    
    // singleton pattern for enemies :/
    public static PlayerController Instance;
    [HideInInspector] public PlayerHealthSystem playerHealthSystem;
    [HideInInspector] public PlayerStats playerStats;
    [HideInInspector] public PlayerManaSystem playerManaSystem;
    
    // James need this for changing weapon type
    [SerializeField] public GameObject abilityCollider;
    [SerializeField] public GameObject stompVfx;
    [SerializeField] public GameObject firePrefab;
    [SerializeField] public GameObject waterParticle;
    [HideInInspector] public bool isWaterGunning = false;
    public bool isInputing = false;
    public Collider hurtBox;

    public Image FadeImage;

    public LayerMask terrainForClickPos;
    public bool canAttack = false;

    public void PlayWakeUpAnimation()
    {
        DisableInput();
        _animator.SetTrigger("sleeping");
        DOVirtual.DelayedCall(4.0f, () => _animator.SetTrigger("wakeUp"));
        DOVirtual.DelayedCall(10.1f, () => EnableInput());
        
        // fade in
        FadeImage.gameObject.SetActive(true);
        FadeImage.color = Color.black;
        DOVirtual.DelayedCall(2.0f, () =>
        {
            FadeImage.DOColor(Color.clear, 5.0f);
            DOVirtual.DelayedCall(6.0f, () => FadeImage.gameObject.SetActive(false));
        });
    }


    void Awake()
    {
        _animator = GetComponent<Animator>();    // it is important that this comes first for initialization of classes
        herdController = GetComponent<HerdController>();
        InitializeRefrences();
        canAttack = true;

        // initialize all states that the player starts with
        attackState = new AttackState(this);
        runningState = new RunningState(this);
        dashState = new DashState(this);
        deathState = new DeathState(this);
        abilityState = new AbilityState(this);
        abilityState.InitializeAbility();

        waterParticle.SetActive(false);
        // initialize starting state
        currentState = runningState;
        
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        
        speedHash = Animator.StringToHash("speed");
        isDashingHash = Animator.StringToHash("isDashing");
        attackTriggerHash = Animator.StringToHash("attackTrigger");
        walkCancelHash = Animator.StringToHash("walkCancel");
        stompHash = Animator.StringToHash("stompTrigger");

        // set directions for isometric camera
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

        if (Instance == null)
        {
            Instance = this;
        }


    }

    public void InitializeRefrences()
    {
        playerHealthSystem = FindObjectOfType<PlayerHealthSystem>();
        playerManaSystem = FindObjectOfType<PlayerManaSystem>();
        playerStats = FindObjectOfType<PlayerStats>();
    }

    public void Die()
    {
        // set state to death state
        currentState.TransitionState(deathState);
    }

    public void Respawn()
    {
        // Restore refrences and values to default state
        InitializeRefrences();
        playerHealthSystem.ResetHealth();
        playerManaSystem.ResetMana();

        // set state to running state
        currentState.TransitionState(runningState);
        PlayWakeUpAnimation();
    }

    // Input 

    void OnCommandHerd(InputAction.CallbackContext context)
    {
        if (herdController == null)
        {
            Debug.Log("Herd Controller Component is missing, cannot command herd.");
            return;
        }
        herdController.StartCharge();
    }

    public static Vector3 DirectionToMouseFromPlayer()
    {
        Camera mainCam = Camera.main;
        Vector3 playerPos = Instance.transform.position;
        Vector3 mousePosition = Instance._playerInput.CharacterControls.MousePosition.ReadValue<Vector2>();
        float zdist = (mainCam.transform.position - playerPos).magnitude;   // distance from cam to player
        
        Vector3 worldPos = mainCam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, zdist));
        var diff = worldPos - playerPos;
        diff.y = 0;
        return diff.normalized;
    }

    public Vector3 WorldPositionToMouseFromPlayer()
    {
        Vector3 worldPos = Vector3.zero;
        Camera mainCam = Camera.main;
        Vector3 mousePosition = Instance._playerInput.CharacterControls.MousePosition.ReadValue<Vector2>();
        Ray ray = mainCam.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if(Physics.Raycast (ray, out hit, 100000f, terrainForClickPos)) {
            worldPos = hit.point;
        }
        return worldPos;
        //return target.transform.position;
    }

    void OnClickMelee(InputAction.CallbackContext context)
    {
        if(canAttack){
            attackState.SetAttackState(meleeMoveFactor, meleeRotationFactor,
                Quaternion.LookRotation(DirectionToMouseFromPlayer()));
            currentState.OnAttack();
            
        }
    }
    
    void OnMelee(InputAction.CallbackContext context)
    {
        if(canAttack){
            attackState.SetAttackState(meleeMoveFactor, meleeRotationFactor, transform.rotation);
            currentState.OnAttack();

        }
    }
    
    void OnDash(InputAction.CallbackContext context)
    {
        if (timeSinceLastDash > dashCooldown)
        {
            currentState.OnDash();
        }
    }
    
    // Consumable

    void OnConsumableStarted(InputAction.CallbackContext context)
    {
        currentState.OnConsumableTriggered(); // Trigger throw
    }

    void OnConsumableEnded(InputAction.CallbackContext context)
    {
        currentState.OnConsumableEnded();
    }
    
    // Consumables
    public void TriggerConsumable(Throwable.ThrowableType type, GameObject prefab)
    {
        switch (type)
        {
            case Throwable.ThrowableType.Bomb:
                currConsumable = new BombThrow(this, prefab);
                break;
            case Throwable.ThrowableType.Dagger:
                currConsumable = new DaggerThrow(this, prefab);
                (currConsumable as DaggerThrow).SetDaggerThrowState(meleeMoveFactor, meleeRotationFactor,
                    Quaternion.LookRotation(DirectionToMouseFromPlayer()));
                break;
        }
        currentState.OnConsumableStarted();
    }

    // all attack animation events should be moved to a different class
    public void AttackAnimationEnded()
    {
        // this should work but it doesn't :/ figure out some way of slowing the char down whilst attacking
        attackState.attackAnimationFinished = true;
    }

    public void EnableDash()
    {
        attackState.canDash = true;
    }

    public void EnableAnotherAttack()
    {
        attackState.canAttackAgain = true;
    }

    public void WalkCancelEnabled()
    {
        attackState.walkCancel = true;
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        Vector2 currentMovementInput = context.ReadValue<Vector2>();
        currentMovement = currentMovementInput.x * right +
                          currentMovementInput.y * forward;
        currentMovement.y = 0;    // only move on the xz plane
        currentMovement = Vector3.Normalize(currentMovement);   // removes subtlety from input direction
        isInputing = false;
    }

    void PostStateUpdate()
    {
        timeSinceLastDash += Time.deltaTime;
    }

    void PreStateUpdate()
    {
        _animator.SetFloat(speedHash, currentMovement.magnitude);
    }

    private void HandleGravity()
    {
        if (!_characterController.isGrounded)
        {
            // move downwards
            downwardsMovement += gravity * Time.deltaTime;
            _characterController.Move(Vector3.down * downwardsMovement * Time.deltaTime);
        }
        else
        {
            downwardsMovement = 0.0f;
        }
    }

    void Update()
    {
        // update the speed data in the animator since it needs to be done every frame
        PreStateUpdate();
        currentState.Execute();
        PostStateUpdate();
        // always handle gravity
        HandleGravity();
    }

    void OnEnable()
    {
        EnableInput();
    }

    void OnDisable()
    {
        DisableInput();
    }

    public void EnableInput()
    {
        _playerInput.CharacterControls.Enable();
        // setup callbacks for input system
        _playerInput.CharacterControls.Move.started += OnMovementInput;
        _playerInput.CharacterControls.Move.canceled += OnMovementInput;
        _playerInput.CharacterControls.Move.performed += OnMovementInput;
        _playerInput.CharacterControls.Dash.started += OnDash;
         _playerInput.CharacterControls.Melee.started += OnMelee;
        _playerInput.CharacterControls.ClickMelee.started += OnClickMelee;
        //_playerInput.CharacterControls.CommandHerd.started += OnCommandHerd;
        
        // ability controls
        _playerInput.CharacterControls.Consumable.started += OnConsumableStarted;
        _playerInput.CharacterControls.Consumable.started += OnConsumableEnded;
        // _playerInput.CharacterControls.WaterGun.started += OnWaterGunAbilityStarted;
        // _playerInput.CharacterControls.WaterGun.started += OnWaterGunAbilityEnded;
        // _playerInput.CharacterControls.WaterGunAttack.started += OnWaterGunAttackHold;
        // _playerInput.CharacterControls.WaterGunAttack.performed += OnWaterGunAttackHold;
        // _playerInput.CharacterControls.WaterGunAttack.canceled += OnWaterGunAttackReleased;

    }

    public void DisableInput()
    {
        // setup callbacks for input system
        _playerInput.CharacterControls.Move.started -= OnMovementInput;
        _playerInput.CharacterControls.Move.canceled -= OnMovementInput;
        _playerInput.CharacterControls.Move.performed -= OnMovementInput;
        _playerInput.CharacterControls.Dash.started -= OnDash;
        Debug.Log("disable attack");
        _playerInput.CharacterControls.Melee.started -= OnMelee;
        _playerInput.CharacterControls.ClickMelee.started -= OnClickMelee;
        //_playerInput.CharacterControls.CommandHerd.started -= OnCommandHerd;
        
        // ability controls
        _playerInput.CharacterControls.Consumable.started -= OnConsumableStarted;
        _playerInput.CharacterControls.Consumable.started -= OnConsumableEnded;
        // _playerInput.CharacterControls.WaterGun.started -= OnWaterGunAbilityStarted;
        // _playerInput.CharacterControls.WaterGun.started -= OnWaterGunAbilityEnded;
        // _playerInput.CharacterControls.WaterGunAttack.started += OnWaterGunAttackHold;
        // _playerInput.CharacterControls.WaterGunAttack.performed += OnWaterGunAttackHold;
        // _playerInput.CharacterControls.WaterGunAttack.canceled += OnWaterGunAttackReleased;
        _playerInput.CharacterControls.Disable();
        
        // Stop movement
        currentMovement = Vector3.zero;
    }

    public void AddAttackFirstTime()
    {
        canAttack = true;
        _playerInput.CharacterControls.Melee.started += OnMelee;
        _playerInput.CharacterControls.ClickMelee.started += OnClickMelee;

    }

    public void SetPlayerWeapon(WeaponObject weapon){
        playerStats.weapon = weapon;
        var weaponScript = GetComponentInChildren<Weapon>(true);
        weaponScript.SwitchWeapon(weapon);
        Debug.Log("switching player weapon" + weapon);
    }

    public void CheckIfWeaponEquipped(){
        if(playerStats.weapon == null){
            Debug.Log("theres no weapon currently");
            canAttack = false;
            _playerInput.CharacterControls.Melee.started -= OnMelee;
            _playerInput.CharacterControls.ClickMelee.started -= OnClickMelee;

        } else{
            canAttack = true;
            _playerInput.CharacterControls.Melee.started += OnMelee;
            _playerInput.CharacterControls.ClickMelee.started += OnClickMelee;
        }
    }
    
}
