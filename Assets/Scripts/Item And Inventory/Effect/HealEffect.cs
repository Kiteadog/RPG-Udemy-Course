using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item effect/Heal  Effect")]
public class HealEffect : ItemEffect
{
    [Range(0f, 1f)]
    [SerializeField] private float healPercent;

    public override void ExecuteEffect(Transform _enemyPosition)
    {
        //获取角色状态
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        //治疗量
        int healAmount = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPercent);

        //治疗
        playerStats.IncreaseHealthyBy(healAmount);
    }
}
