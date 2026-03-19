using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
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

    public PlayerAttack playerAttackState { get; private set; }
    #endregion

    [SerializeField]
    private CharacterData characterData;
    public CharacterData CharData { get => characterData; private set => characterData = value; }
    private bool dead;

    public List<GameObject> touchList { get; private set; }

    void Awake()
    {
        dead = false;
        core = GetComponentInChildren<Core>();
        anim = GetComponent<Animator>();
        touchList = new List<GameObject>();

        stateMachine = new PlayerFiniteStateMachine();
        playerIdleState = new PlayerIdle(this, stateMachine, CharData, "idle");
        playerRunningState = new PlayerMoving(this, stateMachine, CharData, "run");
        playerJumpingState = new PlayerJump(this, stateMachine, CharData, "jump");
        playerLandingState = new PlayerLanding(this, stateMachine, CharData, "landed");
        playerInAirState = new PlayerInAir(this, stateMachine, CharData, "inAir");
        playerAttackState = new PlayerAttack(this, stateMachine, CharData, "attack");
    }

    void Start()
    {
        core.Movement.SetPlayerGravity(CharData.defaultGravity);
        playerInput = GetComponent<PlayerInputManager>();

        stateMachine.ChangeState(playerIdleState);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!touchList.Contains(collision.gameObject))
        {
            touchList.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (touchList.Contains(collision.gameObject))
        {
            touchList.Remove(collision.gameObject);
        }
    }
}
