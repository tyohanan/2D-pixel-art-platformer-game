using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Entity
{
    public E1_idleState idleState { get; private set; }
    public E1_moveState moveState { get; private set; }
    public E1_playerDetectedState playerDetectedState { get; private set; }
    public E1_lookForPlayerState lookForPlayerState { get; private set; }
    public E1_chargePlayerState chargePlayerState { get; private set; }
    public E1_meleeAttackState meleeAttackState { get; private set; }
    public E1_stunState stunState { get; private set; }
    public E1_DeadState deadState { get; private set; }



    [SerializeField]
    private D_IdleState idleStateData;
    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField]
    private D_playerDetectedState playerDetectedData;
    [SerializeField]
    private D_lookForPlayerState lookForPlayerData;
    [SerializeField]
    private D_chargeState chargeStateData;
    [SerializeField]
    private D_meleeAttackState meleeAttackStateData;
    [SerializeField]
    private D_StunState stunStateData;
    [SerializeField]
    private D_DeadState deadStateData;

    [SerializeField]
    private Transform meleeAttackPosition;

    public override void Start()
    {
        base.Start();

        idleState = new E1_idleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new E1_moveState(this, stateMachine, "move", moveStateData, this);
        playerDetectedState = new E1_playerDetectedState(this, stateMachine, "playerDetected", playerDetectedData, this);
        chargePlayerState = new E1_chargePlayerState(this, stateMachine, "charge", chargeStateData, this);
        lookForPlayerState = new E1_lookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerData, this);
        meleeAttackState = new E1_meleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        stunState = new E1_stunState(this, stateMachine, "stun", stunStateData, this);
        deadState = new E1_DeadState(this, stateMachine, "dead", deadStateData, this);

        stateMachine.Initialize(moveState);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);

        if (isDead)
        {
            stateMachine.ChangeState(deadState);
        }
        else if (isStunned && stateMachine.currentState != stunState)
        {
            stateMachine.ChangeState(stunState);
        }
    }
}
