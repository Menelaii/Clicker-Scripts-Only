using System;
using UnityEngine;
using UnityEngine.UI;

public class BodyPartCategoryButton : MonoBehaviour
{
    [SerializeField] private BodyPartType _category;
    [SerializeField] private Sprite _selectedIcon;
    [SerializeField] private Sprite _normalIcon;
    [SerializeField] private Button _button;
    [SerializeField] private Image _icon;

    public event Action<BodyPartType, BodyPartCategoryButton> Clicked;

    private void Awake() =>
        _button.onClick.AddListener(() => Clicked?.Invoke(_category, this));

    public void SetSelectedIcon() =>
        _icon.sprite = _selectedIcon;

    public void SetNormalIcon() =>
        _icon.sprite = _normalIcon;
}
