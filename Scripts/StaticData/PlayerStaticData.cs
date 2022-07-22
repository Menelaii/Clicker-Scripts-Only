using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStaticData", menuName = "Custom/Single/PlayerStaticData")]
public class PlayerStaticData : ScriptableObject
{
    public int MaxClicksPerSecond;
    public int InventorySize;
    [Range(0, 1)] public float AADamageCutPercentage;
    public float RespawnDelay;
    public BodyContainer DefaultBody;
}
