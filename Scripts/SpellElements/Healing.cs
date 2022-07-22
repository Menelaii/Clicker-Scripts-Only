using UnityEngine;

[System.Serializable]
public class Healing : SpellElement
{
    [Header("To owner health")]
    [SerializeField] private RelativeStat _amount;

    public override void Upgrade()
    {
        _amount.Upgrade();
    }

    public override void Apply()
    {
        Owner.Health.Heal(_amount.GetValue(Target, Owner));
    }
}