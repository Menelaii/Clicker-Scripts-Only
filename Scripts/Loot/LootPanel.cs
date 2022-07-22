using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LootPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private TMP_Text _stats;
    [SerializeField] private Image _icon;
    [SerializeField] private Button _sellButton;
    [SerializeField] private Button _takeButton;
    [SerializeField] private Button _inventoryButton;

    private BodyPart _bodyPart;

    public event Action<BodyPart> SellButtonClick;
    public event Action<BodyPart> TakeButtonClick;

    private void Awake()
    {
        _sellButton.onClick.AddListener(OnSellButtonClick);
        _takeButton.onClick.AddListener(OnTakeButtonClick);
    }

    private void OnDestroy()
    {
        _sellButton.onClick.RemoveListener(OnSellButtonClick);
        _takeButton.onClick.RemoveListener(OnTakeButtonClick);
    }

    public void Init(BodyPart bodyPart, bool isInventoryFull)
    {
        _bodyPart = bodyPart;

        _icon.sprite = bodyPart.Icon;
        _stats.text = bodyPart.GetStatsLine();
        _label.text = bodyPart.Label;
        _price.text = bodyPart.Price.ToString();

        OnInventoryItemsCountChanged(isInventoryFull);
    }

    public void OnInventoryItemsCountChanged(bool isInventoryFull)
    {
        _inventoryButton.gameObject.SetActive(isInventoryFull);
        _takeButton.gameObject.SetActive(isInventoryFull == false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void OnSellButtonClick()
    {
        SellButtonClick?.Invoke(_bodyPart);
    }

    private void OnTakeButtonClick()
    {
        TakeButtonClick?.Invoke(_bodyPart);
    }
}
