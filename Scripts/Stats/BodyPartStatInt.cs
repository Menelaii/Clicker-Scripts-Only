using UnityEngine;

[System.Serializable]
public class BodyPartStatInt : BodyPartStat
{
    [SerializeField] private UpgradableStatInt _stat;

    public override UpgradableStat Stat => _stat;

    public UpgradableStatInt StatInt => _stat;

    public override string ToString() => _stat.ToString();
}
