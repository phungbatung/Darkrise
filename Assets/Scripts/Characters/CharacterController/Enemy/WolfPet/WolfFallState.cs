using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfFallState : CharacterState
{
    private Wolf wolf;
    public WolfFallState(Character _character, StateMachine _stateMachine, string _animBoolName) : base(_character, _stateMachine, _animBoolName)
    {
        wolf = _character as Wolf;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("fall state");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (wolf.IsWallDetected() || wolf.HorizontalDistanceToPlayer() < wolf.maxDistanceXToPlayer)
            wolf.SetVelocity(0, wolf.rb.velocity.y);
        else
            wolf.SetVelocity(wolf.facingDir * wolf.moveSpeed, wolf.rb.velocity.y);
        if (wolf.IsGrounded())
            stateMachine.ChangeState(wolf.idleState);
    }
}
