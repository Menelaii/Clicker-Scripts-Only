using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationView : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Transform _contextMenuParent;
    [SerializeField] private WaveButton[] _buttons;

    public event Action<LocationView, WaveButton> ButtonClick;

    public int LocationIndex { get; private set; }
    public Transform ContextMenuParent => _contextMenuParent;
    public IReadOnlyList<WaveButton> Buttons => _buttons;

    private void Awake()
    {
        foreach (WaveButton button in _buttons)
        {
            button.ButtonClick += OnButtonClick;
        }
    }

    public void Init(Location location, ref int wavesBought)
    {
        _icon.sprite = location.Icon;
        LocationIndex = location.Index;

        int i = 0;
        while(wavesBought > 0)
        {
            _buttons[i].SetBought();
            wavesBought--;
            i++;
        }
    }

    private void OnButtonClick(WaveButton waveButton)
    {
        ButtonClick?.Invoke(this, waveButton);
    }
}
