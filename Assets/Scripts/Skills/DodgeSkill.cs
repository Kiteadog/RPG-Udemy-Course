using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//闪避（加回避率和允许回避时生成分身的）
public class DodgeSkill : Skill
{
    [Header("Dodge")]
    [SerializeField] UI_SkillTreeSlot unlockDodgeButton;
    [SerializeField] private int evasionAmount;//被动增加的躲避率
    public bool dodgeUnlocked { get; private set; }

    [Header("Dodge Mirage")]
    [SerializeField] UI_SkillTreeSlot unlockMirageDodgeButton;
    public bool mirageDodgeUnlocked { get; private set; }

    protected override void Start()
    {
        base.Start();

        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }

    protected override void CheckUnlock()
    {
        UnlockDodge();
        UnlockMirageDodge();
    }

    private void UnlockDodge()
    {
        if (unlockDodgeButton.unlocked && !dodgeUnlocked)
        {
            player.stats.evasion.AddModifier(evasionAmount);
            Inventory.instance.UpdateStatsUI();
            dodgeUnlocked = true;
        }
    }

    private void UnlockMirageDodge()
    {
        if (unlockMirageDodgeButton.unlocked)
            mirageDodgeUnlocked = true;
    }

    public void CreateMirageOnDodge()
    {
        if (mirageDodgeUnlocked)
            SkillManager.instance.clone.CreateClone(player.transform, new Vector3(2 * player.facingDir, 0));
    }
}
