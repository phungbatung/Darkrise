using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyDeathState : CharacterState
{
    protected Enemy enemyBase;
    protected bool destroyCooldownActivated;
    protected float delayBeforeDestroy = 2f;
    public EnemyDeathState(Character _character, StateMachine _stateMachine, string _animBoolName) : base(_character, _stateMachine, _animBoolName)
    {
        enemyBase = _character as Enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemyBase.itemDroper?.Drop();
        destroyCooldownActivated = false;
        enemyBase.stats.isImmortal = true;
        enemyBase.SetZeroVelocity();
        
    }

    public override void Exit()
    {
        base.Exit();
        enemyBase.stats.Revive();
        enemyBase.stats.isImmortal = false;
    }

    public override void Update()
    {
        base.Update();

        if (destroyCooldownActivated)
        {
            stateTimer -= Time.deltaTime;
            if(stateTimer <= 0) 
                enemyBase.Despawn();
        }
        if (triggerCalled)
        {
            triggerCalled = false;
            stateTimer = delayBeforeDestroy;
            destroyCooldownActivated = true;
        }  
        
    }

    private void DestroyGO()
    {
        Object.Destroy(enemyBase.gameObject);
    }


}
