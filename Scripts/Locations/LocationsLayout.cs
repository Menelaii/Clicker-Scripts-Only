using System;
using UnityEngine;

public class LocationsLayout : MonoBehaviour
{
    [SerializeField] private LocationView _template;
    [SerializeField] private Transform _itemContainer;
    [SerializeField] private SelectedWaveBox _messageBoxTemplate;

    private Player _player;
    private LocationsStaticData _locationsStaticData;
    private SelectedWaveBox _currentBox;
    private WaveButton _selected;

    private void OnDestroy()
    {
        _player.Traveler.Traveled -= OnTraveled;
    }

    public void Init(Player player, LocationsStaticData locationsStaticData)
    {
        _player = player;
        _locationsStaticData = locationsStaticData;

        CreateViews();

        _player.Traveler.Traveled += OnTraveled;
    }

    public void OnWaveButtonClick(LocationView locationView, WaveButton waveButton)
    {
        if (_currentBox != null)
            Destroy(_currentBox.gameObject);

        if (_player.Traveler.CurrentLocationIndex == locationView.LocationIndex
            && _player.Traveler.CurrentWaveIndex == waveButton.WaveIndex)
            return;

        ShowBox(locationView, waveButton);
    }

    private void CreateViews()
    {
        int wavesBought = _player.Traveler.WavesBought;
        for (int i = 0; i < _locationsStaticData.Locations.Length; i++)
        {
            LocationView view = Instantiate(_template, _itemContainer.transform);

            view.Init(_locationsStaticData.Locations[i], ref wavesBought);
            view.ButtonClick += OnWaveButtonClick;
        }
    }

    private void ShowBox(LocationView locationView, WaveButton waveButton)
    {
        _currentBox = Instantiate(_messageBoxTemplate, locationView.ContextMenuParent);

        var priceGenerator = new StatGenerator(locationView.LocationIndex, waveButton.WaveIndex);
        int price = priceGenerator.GenerateWavePrice(_locationsStaticData.StartPriceForWave, _locationsStaticData.PriceIncreasePercent);

        bool isWaveBought = _player.Traveler.WavesBought > priceGenerator.CurrentWaveGlobalIndex;
        bool isPreviousWaveBought = _player.Traveler.WavesBought > priceGenerator.CurrentWaveGlobalIndex - 1;

        _currentBox.Init(isPreviousWaveBought, isWaveBought, _locationsStaticData.Locations[locationView.LocationIndex], waveButton.WaveIndex, _player, price);
        _currentBox.Bought += OnWaveBought;
    }

    private void OnWaveBought(int locationIndex, int waveIndex)
    {
        GetWaveButton(locationIndex, waveIndex)
            .SetBought();
    }

    private void OnTraveled(int locationIndex, int waveIndex)
    {
        if (_selected != null)
            _selected.SetSelected(false);

        _selected = GetWaveButton(locationIndex, waveIndex);
        _selected.SetSelected(true);
    }

    private WaveButton GetWaveButton(int locationIndex, int waveIndex)
    {
        return _itemContainer
            .GetChild(locationIndex)
            .GetComponent<LocationView>()
            .Buttons[waveIndex];
    }
}
