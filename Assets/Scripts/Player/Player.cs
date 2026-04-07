using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public PlayerInputManager playerInput { get; private set; }
    public Core core { get; private set; }
    public Animator anim { get; private set; }
    public Collider2D col { get; private set; }
    public PlayerFiniteStateMachine stateMachine { get; private set; }

    #region States
    public PlayerIdle playerIdleState { get; private set; }
    public PlayerMoving playerRunningState { get; private set; }
    public PlayerJump playerJumpingState { get; private set; }
    public PlayerLanding playerLandingState { get; private set; }
    public PlayerInAir playerInAirState { get; private set; }
    public PlayerAttack playerAttackState { get; private set; }
    public PlayerPull playerPullState { get; private set; }
    public PlayerWallJumpState playerWallJumpState { get; private set; }
    public PlayerLedgeClimbState playerLedgeClimbState { get; private set; }
    public PlayerWallSlideState playerWallSlideState { get; private set; }
    #endregion

    [SerializeField]
    private CharacterData characterData;
    public CharacterData CharData { get => characterData; private set => characterData = value; }
    private bool dead;

    public List<GameObject> touchList { get; private set; }

    private bool DropInput;
    private Collider2D currentPlatform;

    public LineRenderer lineRenderer { get; private set; }
    public LineRenderer slashLineupLine;

    void Awake()
    {
        dead = false;
        core = GetComponentInChildren<Core>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        lineRenderer = GetComponent<LineRenderer>();
        touchList = new List<GameObject>();

        stateMachine = new PlayerFiniteStateMachine();
        playerIdleState = new PlayerIdle(this, stateMachine, CharData, "idle");
        playerRunningState = new PlayerMoving(this, stateMachine, CharData, "run");
        playerJumpingState = new PlayerJump(this, stateMachine, CharData, "jump");
        playerLandingState = new PlayerLanding(this, stateMachine, CharData, "landed");
        playerInAirState = new PlayerInAir(this, stateMachine, CharData, "inAir");
        playerAttackState = new PlayerAttack(this, stateMachine, CharData, "attack");
        playerPullState = new PlayerPull(this, stateMachine, CharData, "pull");
        playerWallJumpState = new PlayerWallJumpState(this, stateMachine, CharData, "inAir");
        playerLedgeClimbState = new PlayerLedgeClimbState(this, stateMachine, CharData, "ledgeClimb");
        playerWallSlideState = new PlayerWallSlideState(this, stateMachine, CharData, "wallSlide");
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

        DropInput = playerInput.dropInput;
        currentPlatform = core.CollisionSenses.Platform;

        if (DropInput && currentPlatform)
        {
            StartCoroutine(DisableCollision());
        }
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
        GetComponent<SpriteRenderer>().color = Color.red;
        core.Movement.FreezePosition();

        lineRenderer.enabled = false;
        slashLineupLine.enabled = false;

        // Auto load scene for now when you die
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // For interactables
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!touchList.Contains(collision.gameObject))
        {
            touchList.Add(collision.gameObject);
        }
    }
    // For interactables
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (touchList.Contains(collision.gameObject))
        {
            touchList.Remove(collision.gameObject);
        }
    }

    private IEnumerator DisableCollision()
    {
        Collider2D platformCollider = currentPlatform.GetComponent<Collider2D>();

        if (platformCollider == null)
        {
            yield return null;
        }

        // Ignore collision between player and this specific platform
        Physics2D.IgnoreCollision(col, platformCollider, true);

        // Wait for a short duration
        yield return new WaitForSeconds(0.5f);

        // Re-enable collision
        Physics2D.IgnoreCollision(col, platformCollider, false);
    }
}
