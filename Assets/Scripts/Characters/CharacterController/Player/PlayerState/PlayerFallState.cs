using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAirState
{
    private AnimState _animState;
    public PlayerFallState(Character _character, StateMachine _stateMachine, string _animBoolName) : base(_character, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _animState = AnimState.Start;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        
        if (player.IsGrounded())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void StateEvent()
    {
        if (_animState == AnimState.Start)
        {
            player.anim.Play("Player_Fall");
            _animState = AnimState.Loop;
        }
        else if (_animState == AnimState.End)
        {
            
            _animState = AnimState.Start;
        }
    }
}
