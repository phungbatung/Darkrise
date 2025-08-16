using BlitzyUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public Detector detector;
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerFallState fallState { get; private set; }
    public PlayerPrimaryAttack attackState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerSlashState slashState { get; private set; }
    public PlayerHealingState healState { get; private set; }
    public PlayerLightCutState lightCut { get; private set; }
    public PlayerWolfCallState wolfCall { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        detector = GetComponentInChildren<Detector>();

        idleState = new PlayerIdleState(this, stateMachine, "Player_Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Player_Run");
        jumpState = new PlayerJumpState(this, stateMachine, "Player_StartJump");
        fallState = new PlayerFallState(this, stateMachine, "Player_JumpToFall");
        attackState = new PlayerPrimaryAttack(this, stateMachine, "Player_Attack");
        dashState = new PlayerDashState(this, stateMachine, "Player_Dash");
        slashState = new PlayerSlashState(this, stateMachine, "Player_Slash");
        healState = new PlayerHealingState(this, stateMachine, "Player_Healing");
        lightCut = new PlayerLightCutState(this, stateMachine, "Player_LightCut");
        wolfCall = new PlayerWolfCallState(this, stateMachine, "Player_WolvesCall");
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.InitialState(idleState);
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    protected override void Die()
    {
        UIManager.Instance.QueuePush(GameManager.revivePopup, null);
        Time.timeScale = 0f;
    }
    
    public void Revive()
    {
        stats.Revive();
        stateMachine.ChangeState(idleState);
        Time.timeScale = 1f;
    }    
}
