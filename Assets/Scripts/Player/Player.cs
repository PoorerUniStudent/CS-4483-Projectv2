using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInputManager playerInput { get; private set; }
    public Core core { get; private set; }
    public Animator anim { get; private set; }
    public PlayerFiniteStateMachine stateMachine { get; private set; }

    #region States
    public PlayerIdle playerIdleState { get; private set; }
    public PlayerMoving playerRunningState { get; private set; }
    public PlayerJump playerJumpingState { get; private set; }
    public PlayerLanding playerLandingState { get; private set; }
    public PlayerInAir playerInAirState { get; private set; }
    #endregion

    [SerializeField]
    private CharacterData characterData;
    public CharacterData CharData { get => characterData; private set => characterData = value; }
    private bool dead;

    void Awake()
    {
        dead = false;
        core = GetComponentInChildren<Core>();
        anim = GetComponent<Animator>();

        stateMachine = new PlayerFiniteStateMachine();
        playerIdleState = new PlayerIdle(this, stateMachine, CharData, "idle");
        playerRunningState = new PlayerMoving(this, stateMachine, CharData, "run");
        playerJumpingState = new PlayerJump(this, stateMachine, CharData, "jump");
        playerLandingState = new PlayerLanding(this, stateMachine, CharData, "landed");
        playerInAirState = new PlayerInAir(this, stateMachine, CharData, "inAir");

        stateMachine.ChangeState(playerIdleState);
    }

    void Start()
    {
        core.Movement.SetPlayerGravity(CharData.defaultGravity);
        playerInput = GetComponent<PlayerInputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            return;
        }

        core.LogicUpdate();
        stateMachine.currentState.LogicUpdate();
    }

    public PlayerState GetCurrentState()
    {
        return stateMachine.currentState;
    }

    // Call these from the animation to call the AnimationTrigger in the states
    public void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();
    public void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public void Die()
    {
        Debug.Log("PLayer ded");
        dead = true;
    }
}
