using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : CharacterState
{

    protected Player player;
    public PlayerAirState(Character _character, StateMachine _stateMachine, string _animBoolName) : base(_character, _stateMachine, _animBoolName)
    {
        player = _character as Player;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.anim.SetFloat("yVelocity", player.rb.velocity.y);
        player.SetVelocity(xInput * player.moveSpeed * (100f + player.stats.moveSpeed.GetValue())/100f, player.rb.velocity.y);
        if (SkillManager.Instance.dash.CanBeUse())
            SkillManager.Instance.dash.Called();
        if (SkillManager.Instance.baseAttack.CanBeUse())
            SkillManager.Instance.baseAttack.Called();
    }
}
