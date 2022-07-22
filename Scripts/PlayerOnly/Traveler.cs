using System;
using UnityEngine;

[Serializable]
public class Traveler
{
    [SerializeField] private int _currentLocationIndex;
    [SerializeField] private int _currentWaveIndex;
    [SerializeField] private int _wavesBought;

    public int CurrentLocationIndex => _currentLocationIndex;
    public int CurrentWaveIndex => _currentWaveIndex;
    public int WavesBought => _wavesBought;
    public int CurrentWaveGlobalIndex => StatGenerator.GetGlobalWaveIndex(_currentLocationIndex, _currentWaveIndex);
    public bool IsOnLastBoughtWave => CurrentWaveGlobalIndex + 1 == _wavesBought;

    public event Action<int, int> Traveled;

    public void Travel(int locationIndex, int waveIndex)
    {
        _currentLocationIndex = locationIndex;
        _currentWaveIndex = waveIndex;
        Traveled?.Invoke(locationIndex, waveIndex);
    }

    public void OnWaveBought()
    {
        _wavesBought++;
    }
}
