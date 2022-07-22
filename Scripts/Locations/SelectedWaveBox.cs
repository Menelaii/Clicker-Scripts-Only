using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class SelectedWaveBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _buyPreviousWavesMessage;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _travelButton;

    private Player _player;
    private Location _location;
    private int _waveIndex;
    private int _price;
    private bool _pointerOutOfBox;

    public event Action<int, int> Bought;

    private void OnEnable()
    {
        _buyButton.onClick.AddListener(OnBuyButtonClick);
        _travelButton.onClick.AddListener(OnTravelButtonClick);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && _pointerOutOfBox)
            Destroy(gameObject);
    }

    private void OnDisable()
    {
        _buyButton.onClick.RemoveListener(OnBuyButtonClick);
        _travelButton.onClick.RemoveListener(OnTravelButtonClick);

        Destroy(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _pointerOutOfBox = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _pointerOutOfBox = true;
    }

    public void Init(bool isPreviousWaveBought, bool isWaveBought, Location location, int waveIndex, Player player, int price)
    {
        _player = player;
        _location = location;
        _waveIndex = waveIndex;
        _price = price;
        _pointerOutOfBox = true;

        _buyPreviousWavesMessage.gameObject.SetActive(!isPreviousWaveBought);
        _travelButton.gameObject.SetActive(isWaveBought);

        if(isPreviousWaveBought && isWaveBought == false)
        {
            _text.text = "Buy: " + _price;
            _text.gameObject.SetActive(true);
            _buyButton.gameObject.SetActive(true);
        }
    }

    private void OnBuyButtonClick()
    {
        if (_player.Wallet.TryBuy(_price))
        {
            _player.AddBoughtWave();

            _travelButton.gameObject.SetActive(true);
            _text.gameObject.SetActive(false);
            _buyButton.gameObject.SetActive(false);

            Bought?.Invoke(_location.Index, _waveIndex);
        }
    }

    private void OnTravelButtonClick()
    {
        _player.Traveler.Travel(_location.Index, _waveIndex);
        Destroy(gameObject);
    }
}
