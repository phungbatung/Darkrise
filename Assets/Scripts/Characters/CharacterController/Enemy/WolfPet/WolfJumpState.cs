using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfJumpState : CharacterState
{
    private Wolf wolf;
    public WolfJumpState(Character _character, StateMachine _stateMachine, string _animBoolName) : base(_character, _stateMachine, _animBoolName)
    {
        wolf = _character as Wolf;
    }

    public override void Enter()
    {
        base.Enter();
        wolf.rb.velocity = new Vector2(0, wolf.jumpForce);
        Debug.Log("jump");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {

        if (wolf.IsWallDetected() || wolf.HorizontalDistanceToPlayer() < wolf.maxDistanceXToPlayer)
            wolf.SetVelocity(0, wolf.rb.velocity.y);
        else
            wolf.SetVelocity(wolf.facingDir * wolf.moveSpeed, wolf.rb.velocity.y);
        if (triggerCalled)
            stateMachine.ChangeState(wolf.fallState);
        if (wolf.IsDetectEnenmy())
            stateMachine.ChangeState(wolf.battleState);
    }
}
