using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargePlayerState : State
{
    protected D_chargeState stateData;

    protected bool isPlayerInMinAgroRange;
    protected bool isDectectingLedge;
    protected bool isDetectingWall;
    protected bool isChargeTimeOver;
    protected bool performCloseRangeAction;

    public ChargePlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_chargeState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();


        isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        isDectectingLedge = entity.CheckLedge();
        isDetectingWall = entity.CheckWall();

        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
    }

    public override void Enter()
    {
        base.Enter();

        isChargeTimeOver = false;
        entity.SetVelocity(stateData.chargeSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + stateData.chargeTime)
        {
            isChargeTimeOver = true;
        }
    }

    public override void PhsyicsUpdate()
    {
        base.PhsyicsUpdate();
    }
}
