using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//¸ñµ²
public class ParrySkill : Skill
{
    [Header("Parry")]
    [SerializeField] private UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked { get; private set; }

    [Header("Parry restore")]
    [SerializeField] private UI_SkillTreeSlot restoreUnlockButton;
    [Range(0f, 1f)]
    [SerializeField] private float restoreHealthPercentage;
    public bool restoreUnlocked { get; private set; }

    [Header("Parry with mirage")]
    [SerializeField] private UI_SkillTreeSlot mirageParryUnlockButton;
    public bool mirageParryUnlocked { get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();

        if (restoreUnlocked)
        {
            player.stats.IncreaseHealthyBy(Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restoreHealthPercentage));

        }
    }

    protected override void Start()
    {
        base.Start();

        parryUnlockButton.GetComponent<Button>().onClick.AddListener(unlockParry);
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(unlockParryRestore);
        mirageParryUnlockButton.GetComponent<Button>().onClick.AddListener(unlockParryWithMirage);
    }

    protected override void CheckUnlock()
    {
        unlockParry();
        unlockParryRestore();
        unlockParryWithMirage();
    }

    private void unlockParry()
    {
        if(parryUnlockButton.unlocked) 
            parryUnlocked = true;
    }

    private void unlockParryRestore()
    {
        if (restoreUnlockButton.unlocked)
            restoreUnlocked = true;
    }

    private void unlockParryWithMirage()
    {
        if (mirageParryUnlockButton.unlocked)
            mirageParryUnlocked = true;
    }

    public void MakeMirageOnParry(Transform _respawnTransform)
    {
        if (mirageParryUnlocked)
            SkillManager.instance.clone.CreateCloneWithDelay(_respawnTransform); 
    }
}
