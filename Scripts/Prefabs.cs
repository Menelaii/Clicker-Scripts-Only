using UnityEngine;

[CreateAssetMenu(fileName = "PrefabList", menuName = "Custom/Single/PrefabList")]
public class Prefabs : ScriptableObject
{
    public PlayerView PlayerView;
    public DamageView DamageView;
    public LootBox Lootbox;
    public FlyingText DodgeView;
    public FlyingText TotalValueView;
    public StormCloud StormCloud;
    public AccumulatorEntity HealthAccumulator;
}
