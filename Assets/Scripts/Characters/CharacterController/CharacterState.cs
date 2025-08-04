using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState
{
    public Character character;
    public StateMachine stateMachine;
    public string defaultAnim;

    public float xInput;
    public float yVelocity;
    public bool triggerCalled=false;
    public float stateTimer;
    public CharacterState (Character _character, StateMachine _stateMachine, string _defaultAnimName)
    {
        character = _character;
        stateMachine = _stateMachine;
        defaultAnim = _defaultAnimName;
    }
    public virtual void Enter()
    {
        character.anim.Play(defaultAnim);
        //Debug.Log(GetType().Name);
    }

    public virtual void Exit() 
    {
        triggerCalled = false;
    }

    public virtual void Update()
    {
        xInput = InputManager.Instance.horizontalInput;
        stateTimer -= Time.deltaTime;
    }

    public virtual void TriggerCall() => triggerCalled = true;
    public virtual void StateEvent()
    {

    }
    public virtual void PlaySFX()
    {

    }
}
