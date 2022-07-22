using UnityEngine;

[CreateAssetMenu(fileName = "New Loot", menuName = "Custom/Loot")]
public class Loot : ScriptableObject
{
    public int TotalCoinsValue;
    public CoinDrop[] CoinDrops;
    [Range(0, 100)] public int LootBoxDropChance;
    public BodyPartContainer[] BodyParts;
    public LootBox LootBoxPrefab;

    public void OnValidate()
    {
        if (CoinDrops == null)
            return;

        TotalCoinsValue = 0;
        foreach (var coinDrop in CoinDrops)
        {
            if (coinDrop.Prefab != null)
            {
                TotalCoinsValue += coinDrop.Prefab.Value * coinDrop.Amount;
            }
        }
    }

    public BodyPart GetRandomBodyPart()
    {
        return BodyParts[Random.Range(0, BodyParts.Length - 1)].GetClone();
    }
}