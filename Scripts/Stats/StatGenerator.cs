using UnityEngine;

public class StatGenerator
{
    public int CurrentWaveGlobalIndex { get; private set; }

    public StatGenerator(int locationIndex, int waveIndex)
    {
        CurrentWaveGlobalIndex = GetGlobalWaveIndex(locationIndex, waveIndex);
    }

    public StatGenerator(int currentWaveGlobalIndex)
    {
        CurrentWaveGlobalIndex = currentWaveGlobalIndex;
    }

    public static int GetGlobalWaveIndex(int locationIndex, int waveIndex)
    {
        return locationIndex * Location.WavesPerLocation + waveIndex;
    }

    public int GenerateStat(int startValue, int increasePercent)
    {
        return Mathf.RoundToInt(Mathf.Pow(1 + increasePercent / 100f, CurrentWaveGlobalIndex) * startValue);
    }

    public int GenerateWavePrice(int startValue, int increasePercent)
    {
        return Mathf.RoundToInt(Mathf.Pow(1 + increasePercent / 100f, CurrentWaveGlobalIndex - 1) * startValue);
    }
}
