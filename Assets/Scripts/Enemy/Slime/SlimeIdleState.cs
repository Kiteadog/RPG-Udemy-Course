using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeIdleState : SlimeGroundedState
{
    public SlimeIdleState(EnemyStateMachine _stateMachine, Enemy _enemyBase, string _animBoolName, Enemy_Slime _enemy) : base(_stateMachine, _enemyBase, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.PlaySFX(24, enemy.transform);
    }

    public override void UPdate()
    {
        base.UPdate();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.moveState);
    }
}
