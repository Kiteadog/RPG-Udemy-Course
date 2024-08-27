using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThunderStrike Effect", menuName = "Data/Item effect/Thunder strike")]
public class ThunderStrikeEffect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePerfab;

    public override void ExecuteEffect(Transform _respawnPosition)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikePerfab, _respawnPosition.position, Quaternion.identity);
        Destroy(newThunderStrike, 0.5f);
    }
}
