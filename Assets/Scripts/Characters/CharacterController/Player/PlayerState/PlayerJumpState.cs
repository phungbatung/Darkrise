using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(Character _character, StateMachine _stateMachine, string _animBoolName) : base(_character, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(player.rb.velocity.x, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (player.rb.velocity.y <= 0)
            stateMachine.ChangeState(player.fallState);
        
    }
    public override void StateEvent()
    {
        player.anim.Play("Player_Jump");
    }
}
