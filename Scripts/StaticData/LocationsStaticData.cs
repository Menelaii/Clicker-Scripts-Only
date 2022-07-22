using UnityEngine;

[CreateAssetMenu(fileName = "Locations", menuName = "Custom/Single/LocationsList")]
public class LocationsStaticData : ScriptableObject
{
    public int StartPriceForWave;
    [Range(0, 100)] public int PriceIncreasePercent;
    public Location[] Locations;

    public void Awake()
    {
        for (int i = 0; i < Locations.Length; i++)
        {
            Locations[i].SetIndex(i);
        }
    }
}
