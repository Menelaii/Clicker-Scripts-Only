using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private CooldownPanel _cooldownPanel;
    [SerializeField] private Image _notAvailablePanel;

    private Mana _userMana;

    public Spell Ability { get; private set; }

    public event Action<SpellView> Clicked;

    private void Awake()
    {
        _cooldownPanel.Closed += OnCooldownPanelClosed;
    }

    private void OnDestroy()
    {
        _cooldownPanel.Closed -= OnCooldownPanelClosed;
    }

    public void Init(Spell ability, Mana userMana)
    {
        Ability = ability;
        _icon.sprite = ability.Icon;
        _userMana = userMana;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_notAvailablePanel.gameObject.activeSelf == false)
        {
            Clicked?.Invoke(this);
        }
    }

    public void SetCooldown()
    {
        _cooldownPanel.Open(Ability.Cooldown);
    }

    public void OnManaChanged(int mana, int maxMana)
    {
        if (_cooldownPanel.gameObject.activeSelf)
            return;

        TrySetAvailable(mana, maxMana);
    }

    private void TrySetAvailable(int mana, int maxMana)
    {
        if (Mana.IsEnoughFor(Ability.ManaCostInPercent, mana, maxMana) == false)
        {
            SetNotAvailable();
        }
        else
        {
            SetAvailable();
        }
    }

    private void SetNotAvailable()
    {
        _notAvailablePanel.gameObject.SetActive(true);
    }

    private void SetAvailable()
    {
        _notAvailablePanel.gameObject.SetActive(false);
    }

    private void OnCooldownPanelClosed()
    {
        TrySetAvailable(_userMana.Value, _userMana.MaxValue);
    }
}