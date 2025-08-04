using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wolf : Enemy
{
    public LayerMask enemyLayer;
    private float lifeTime;
    //public WolfIdleState idleState { get; private set; }
    //public WolfJumpState jumpState { get; private set; }
    //public WolfAttackState attackState { get; private set; }
    public WolfMoveState moveState { get; private set; }
    public WolfFallState fallState { get; private set; }
    public WolfBattleState battleState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        idleState = new WolfIdleState(this, stateMachine, "idle");
        moveState = new WolfMoveState(this, stateMachine, "move");
        jumpState = new WolfJumpState(this, stateMachine, "jump");
        fallState = new WolfFallState(this, stateMachine, "fall");
        battleState = new WolfBattleState(this, stateMachine, "move");
        attackState = new WolfAttackState(this, stateMachine, "attack");
        deathState = new EnemyDeathState(this, stateMachine, "death");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.InitialState(jumpState);
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public RaycastHit2D IsDetectEnenmy()
    {
        return  Physics2D.Linecast(transform.position - new Vector3(3, 0, 0), transform.position + new Vector3(3, 0, 0), enemyLayer);
    }

    
    public bool IsEnemyInAttackRange()
    {
        return Physics2D.Raycast(wallCheck.position, facingDir * Vector3.right, attackPoint.position.x + facingDir * attackRadius - wallCheck.position.x, enemyLayer);
    }
    public void SetUpWolf(Player _player, Vector3 _position, SkillLevelData _wolfCallLevelData)
    {
        player = _player;
        int statPercentage = _wolfCallLevelData.GetProperty<int>(SkillLevelData.Key.STAT_PERCENTAGE);
        stats.maxHealth.AddModifier((int)(player.stats.maxHealth.GetValue() * statPercentage / 100f));
        stats.damage.AddModifier((int)(player.stats.damage.GetValue() * statPercentage/100f));
        stats.moveSpeed.AddModifier((int)( player.moveSpeed * (100f +player.stats.moveSpeed.GetValue())/100f * Random.Range(60, 90)/100f));
        stats.armor.AddModifier(player.stats.armor.GetValue());
        stats.attackSpeed.AddModifier(player.stats.attackSpeed.GetValue());
        stats.armorPenetration.AddModifier(player.stats.armorPenetration.GetValue());
        transform.position = _position;
        lifeTime=_wolfCallLevelData.GetProperty<int>(SkillLevelData.Key.LIFESPAN);
        moveSpeed = player.moveSpeed;
        if (player.facingDir == -1)
            Flip();
        Invoke("DestroyGO", lifeTime);
    }

    private void DestroyGO()
    {
        Destroy(gameObject);
    }
}
