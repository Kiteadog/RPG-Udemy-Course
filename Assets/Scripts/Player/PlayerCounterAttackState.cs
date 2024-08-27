using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;

    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        canCreateClone = true;
        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>().CanBeStunned())
            {
                stateTimer = 10;//格挡成功的计时器，大于1即可，因为该动画播放完成后会推出状态。
                player.anim.SetBool("SuccessfulCounterAttack", true);

                player.skill.parry.UseSkill();//格挡恢复

                if (canCreateClone)
                {
                    canCreateClone = false;
                    player.skill.parry.MakeMirageOnParry(hit.transform);
                }
            }
        }

        if(stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
