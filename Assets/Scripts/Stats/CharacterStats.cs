using System.Collections;
using UnityEngine;

public enum StatType
{
    strength,//力量影响伤害
    agility,//敏捷影响闪避
    intelligence,//魔法伤害，魔法抵抗
    vitality,//生命力
    damage,//攻击力
    critChance,//暴击率
    critPower,//暴击伤害默认150%
    maxHealth,//生命上限
    armor,//护甲
    evasion,//闪避
    magicResistance,//法抗
    fireDamage,//火焰伤害
    iceDamage,//冰冻伤害
    lightingDamage//雷电伤害
}

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major stats")]
    public Stat strength;//力量影响伤害
    public Stat agility;//敏捷影响闪避
    public Stat intelligence;//魔法伤害，魔法抵抗
    public Stat vitality;//生命力

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;//暴击伤害默认150%

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;//护甲
    public Stat evasion;//闪避
    public Stat magicResistance;//法抗

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;

    public bool isIgnited;//灼烧持续伤害
    public bool isChilled;//冰冻降低护甲
    public bool isShocked;//导电降低命中

    [SerializeField] private float ailmentsDuration = 4;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float ignitedCooldown = 0.3f;
    private float ignitedDamageTimer;
    private int igniteDamage;
    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDamage;

    public int currentHealth;

    public System.Action onHealthChanged;//血量变动事件
    public bool isDead { get; private set; }
    public bool isInvincible { get; private set; }//无敌时间
    public bool isVulnerable;//易伤状态，多受10%伤害


    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();    
        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        ignitedDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;
        if (chilledTimer < 0)
            isChilled = false;
        if (shockedTimer < 0)
            isShocked = false;
        if(isIgnited)
            ApplyIgniteDamagae();
    }

    public void MakeVulnerableFor(float _duration) => StartCoroutine(VulnerableForCoroutine(_duration));


    private IEnumerator VulnerableForCoroutine(float _duration)
    {
        isVulnerable = true;

        yield return new WaitForSeconds(_duration);

        isVulnerable = false;
    }

    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statToModify)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));
    }

    private IEnumerator StatModCoroutine(int _modifier, float _duration, Stat _statToModify)
    {
        _statToModify.AddModifier(_modifier);

        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);
    }

    public virtual void Dodamage(CharacterStats _targetStats)
    {
        bool criticalStrike = false;

        if (_targetStats.isInvincible)
            return;

        if (TargetCanAvoidAttack(_targetStats))
            return;

        _targetStats.GetComponent<Entity>().SetupKonckblackDir(transform);

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        { 
            totalDamage = CalculateCriticalDamage(totalDamage);
            criticalStrike = true;
        }

        fx.CreateHitFX(_targetStats.transform, criticalStrike);//添加攻击视觉效果

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage);
        DoMagicalDamage(_targetStats);//普攻魔法效果
    }

    #region 魔法伤害及异常状态
    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();
        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);

        _targetStats.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;

        AttemptyToApplyAilements(_targetStats, _fireDamage, _iceDamage, _lightingDamage);

    }
    //添加异常状态
    private void AttemptyToApplyAilements(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightingDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < 0.5f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                break;
            }
            if (Random.value < 0.5f && _iceDamage > 0)
            {
                canApplyChill = true;
                break;
            }
            if (Random.value < 0.5f && _lightingDamage > 0)
            {
                canApplyShock = true;
                break;
            }
        }

        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * 0.2f));

        if (canApplyShock)
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightingDamage * 0.1f));

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }
    //异常
    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;

            fx.IgniteFXFor(ailmentsDuration);   
        }

        if (_chill && canApplyChill)
        {
            isChilled = _chill;
            chilledTimer = ailmentsDuration;

            float slowPercentage = 0.4f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);
            fx.ChillFXFor(ailmentsDuration);
        }

        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                if (GetComponent<Player>() != null)
                    return;
                HitClosestTargetWithShockStrike();
            }
        }
    }
    //导电
    public void ApplyShock(bool _shock)
    {
        if (isShocked)
            return;

        isShocked = _shock;
        shockedTimer = ailmentsDuration;

        fx.ShockFXFor(ailmentsDuration);
    }
    //电击最近目标
    private void HitClosestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)
                closestEnemy = transform;
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);

            newShockStrike.GetComponent<ShockStrikeController>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }
    //灼烧伤害
    private void ApplyIgniteDamagae()
    {
        if (ignitedDamageTimer < 0)
        {
            DecreaseHealthyBy(igniteDamage);

            if (currentHealth < 0 && !isDead)
                Die();

            ignitedDamageTimer = ignitedCooldown;
        }
    }
    //灼烧
    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
    //导电
    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;
    #endregion

    public virtual void TakeDamage(int _damage)
    {
        if (isInvincible)
            return;

        DecreaseHealthyBy(_damage);

        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");

        if (currentHealth < 0 && !isDead)
        {
            Die();
        }
    }

    //治疗
    public virtual void IncreaseHealthyBy(int _healAmount)
    {
        currentHealth += _healAmount;
        if (currentHealth > GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();

        if (onHealthChanged != null)
            onHealthChanged();
    }

    //扣血
    protected virtual void DecreaseHealthyBy(int _damage)
    {
        if (isVulnerable)
            _damage = Mathf.RoundToInt(_damage * 1.1f);

        currentHealth -= _damage;

        if (_damage > 0)
            fx.CreatePopUpText(_damage.ToString());

        if (onHealthChanged != null)
            onHealthChanged();

    }

    //死亡
    protected virtual void Die()
    {
        isDead = true;
    }

    public void KillEntity()
    {
        if (!isDead)
            Die();
    } 

    public void MakeInvincible(bool _invincible) => isInvincible = _invincible;

    public virtual void OnEvasion()
    {

    }

    #region 数值计算
    //躲避计算
    protected bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
            totalEvasion += 20;

        if (Random.Range(0, 100) <= totalEvasion)
        {
            _targetStats.OnEvasion();
            return true;
        }
        return false;
    }
    //防御计算
    protected int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
        {
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f);
        }
        else
            totalDamage -= _targetStats.armor.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    //魔抗计算
    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }
    //暴击计算
    protected bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if(Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }
    //暴击伤害计算
    protected int CalculateCriticalDamage(int _damage)
    {
        float totalCritDamage = (critPower.GetValue() * strength.GetValue()) * 0.01f;
        float critDamage = _damage * totalCritDamage;
        return Mathf.RoundToInt(critDamage);
    }
    //获取最大生命值
    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
    #endregion

    public Stat GetStat(StatType _statType)
    {
        switch (_statType)
        {
            case StatType.strength: return strength;//力量影响伤害
            case StatType.agility: return agility;//敏捷影响闪避
            case StatType.intelligence: return intelligence;//魔法伤害，魔法抵抗
            case StatType.vitality: return vitality;//生命力
            case StatType.damage: return damage;//攻击力
            case StatType.critChance: return critChance;//暴击率
            case StatType.critPower: return critPower;//暴击伤害默认150%
            case StatType.maxHealth: return maxHealth;//生命上限
            case StatType.armor: return armor;//护甲
            case StatType.evasion: return evasion;//闪避
            case StatType.magicResistance: return magicResistance;//法抗
            case StatType.fireDamage: return fireDamage;//火焰伤害
            case StatType.iceDamage: return iceDamage;//冰冻伤害
            case StatType.lightingDamage: return lightingDamage;//雷电伤害
            default: return null;
        }
    }
}
