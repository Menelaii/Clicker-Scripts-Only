using UnityEngine;

[CreateAssetMenu(fileName = "new Generation Settings", menuName = "Custom/Single/BPGenerationSettings")]
public class BodyPartsGenerationStaticData : ScriptableObject
{
    [Range(0, 400)] public int StatsMaxSpreadPercent;
    public int UpgradesPerWave;
}
