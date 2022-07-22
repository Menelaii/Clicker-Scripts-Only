using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class BodyPartInventoryView : BodyPartUIView
{
    [SerializeField] private TMP_Text _price;
    [SerializeField] private TMP_Text _upgradePrice;
    [SerializeField] private TMP_Text _stats;
    [SerializeField] private Button _equipButton;
    [SerializeField] private Button _sellButton;
    [SerializeField] private Button _upgradeButton;

    public event Action<BodyPart, BodyPartInventoryView> EquipButtonClick;
    public event Action<BodyPart, BodyPartInventoryView> SellButtonClick;
    public event Action<BodyPart, BodyPartInventoryView> UpgradeButtonClick;

    private void Awake()
    {
        _equipButton?.onClick.AddListener(() => EquipButtonClick?.Invoke(BodyPart, this));
        _sellButton?.onClick.AddListener(() => SellButtonClick?.Invoke(BodyPart, this));
        _upgradeButton?.onClick.AddListener(() => UpgradeButtonClick?.Invoke(BodyPart, this));
    }

    public override void Init(BodyPart bodyPart)
    {
        base.Init(bodyPart);
        _price.text = bodyPart.Price.ToString();
        _upgradePrice.text = bodyPart.UpgradePrice.ToString();
        _stats.text = bodyPart.GetStatsLine();
    }

    public void DisableEquipButton() => _equipButton.interactable = false;

    public void DisableSellButton() => _sellButton.interactable = false;

    public void EnableEquipButton() => _equipButton.interactable = true;

    public void EnableSellButton() => _sellButton.interactable = true;
}
