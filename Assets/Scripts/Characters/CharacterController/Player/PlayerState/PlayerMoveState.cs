using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    private AnimState _animState;
    public PlayerMoveState(Character _character, StateMachine _stateMachine, string _animBoolName) : base(_character, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.anim.speed = (100f + player.stats.moveSpeed.GetValue()) / 100f;
        _animState = AnimState.Start;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (xInput == 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
        player.SetVelocity(xInput * player.moveSpeed * (100f + player.stats.moveSpeed.GetValue()) / 100f, player.rb.velocity.y);
    }
    public override void StateEvent()
    {
        if(_animState == AnimState.Start)
        {
            player.anim.Play("Player_Run");
            _animState = AnimState.Loop;
        }
        else if(_animState == AnimState.End)
        {
            stateMachine.ChangeState(player.idleState);
            _animState = AnimState.Start;
        }
    }
}
