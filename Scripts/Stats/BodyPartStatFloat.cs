using UnityEngine;

[System.Serializable]
public class BodyPartStatFloat : BodyPartStat
{
    [SerializeField] private UpgradableStatFloat _stat;

    public override UpgradableStat Stat => _stat;

    public UpgradableStatFloat StatFloat => _stat;

    public override string ToString() => _stat.ToString();
}