using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_meleeAttackState : MeleeAttackState
{
    private Enemy1 enemy;

    public E1_meleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_meleeAttackState stateData, Enemy1 enemy) : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.enemy = enemy;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isAnimationFinished)
        {
            if (isPlayerInMinAgroRange)
            {
                stateMachine.ChangeState(enemy.playerDetectedState);
            }
            else
            {
                stateMachine.ChangeState(enemy.lookForPlayerState);
            }
        }
    }

    public override void PhsyicsUpdate()
    {
        base.PhsyicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
    }
}
