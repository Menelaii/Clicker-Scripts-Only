using System;
using UnityEngine;
using UnityEngine.UI;

public class WaveButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _flagImage;
    [SerializeField] private int _waveIndex;
    [SerializeField] private Sprite _selected;
    [SerializeField] private Sprite _normal;

    private bool bought = false;

    public event Action<WaveButton> ButtonClick;

    public int WaveIndex => _waveIndex;

    private void Awake()
    {
        _button.onClick.AddListener(() => ButtonClick?.Invoke(this));
    }

    public void SetSelected(bool selected)
    {
        if (bought == false && selected)
            _flagImage.enabled = true;

        if (bought == false && selected == false)
            _flagImage.enabled = false;

        _flagImage.sprite = selected ? _selected : _normal;
    }

    public void SetBought()
    {
        bought = true;
        _flagImage.enabled = true;
    }
}
