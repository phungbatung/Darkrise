using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfIdleState : CharacterState
{
    private Wolf wolf;
    public WolfIdleState(Character _character, StateMachine _stateMachine, string _animBoolName) : base(_character, _stateMachine, _animBoolName)
    {
        wolf = _character as Wolf;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("idle");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        wolf.SetZeroVelocity();
        if (!wolf.IsGrounded())
            stateMachine.ChangeState(wolf.fallState);
        if (wolf.HorizontalDistanceToPlayer() > wolf.maxDistanceXToPlayer)
            stateMachine.ChangeState(wolf.moveState);
        if (wolf.IsDetectEnenmy())
            stateMachine.ChangeState(wolf.battleState);
    }
}
