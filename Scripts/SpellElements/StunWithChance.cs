using UnityEngine;

[System.Serializable]
public class StunWithChance : SpellElement
{
    [SerializeField] private UpgradableStatFloat _time;
    [SerializeField] private UpgradableStatInt _chance;

    public override void Upgrade()
    {
        _time.Upgrade();
        _chance.Upgrade();
    }

    public override void Apply()
    {
        if (Random.Range(1, 100) < _chance.Value)
        {
            Target.Stun();
            CoroutineMaster.Instance.InvokeAfterDelay(() => Target.UnStun(), _time.Value);
        }
    }
}
