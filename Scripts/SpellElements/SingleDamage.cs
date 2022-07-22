using UnityEngine;

[System.Serializable]
public class SingleDamage : SpellElement
{
    [SerializeField] private RelativeStat _damage;
    [SerializeField] private DamageType _damageType;

    public override void Upgrade()
    {
        _damage.Upgrade();
    }

    public override void Apply()
    {
        Target.TakeDamage(_damage.GetValue(Target, Owner), _damageType);
    }
}