using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BodyPartsLayout : MonoBehaviour
{
    [SerializeField] private BodyPartInventoryView _template;
    [SerializeField] private GameObject _itemContainer;
    [SerializeField] private BodyPartCategoryButton[] _categoryButtons;

    private List<BodyPartInventoryView> _views = new List<BodyPartInventoryView>();
    private BodyPartInventoryView _equiped;
    private BodyPartCategoryButton _selected;
    private Player _player;
    private List<BodyPart> _buffer;

    private void Awake()
    {
        foreach (var button in _categoryButtons)
        {
            button.Clicked += OnCategoryButtonClicked;
        }

        _buffer = new List<BodyPart>();
    }

    private void OnDisable()
    {
        _selected?.SetNormalIcon();
        DestroyViews();
    }

    private void OnDestroy()
    {
        foreach (var button in _categoryButtons)
        {
            button.Clicked -= OnCategoryButtonClicked;
        }
    }

    public void Init(Player player)
    {
        _player = player;
    }

    private void OnCategoryButtonClicked(BodyPartType type, BodyPartCategoryButton button)
    {
        _selected?.SetNormalIcon();
        _selected = button;
        _selected.SetSelectedIcon();

        DestroyViews();
        CreateViewsWith(type);
        EnableViews();
    }

    private void DestroyViews()
    {
        for (int i = 0; i < _views.Count; i++)
        {
            DestroyView(_views[i]);
        }

        _views.Clear();
    }

    private void CreateViewsWith(BodyPartType type)
    {
        _buffer.Clear();

        switch (type)
        {
            case BodyPartType.Head:
                _buffer.Add(_player.Body.Head);
                _buffer.AddRange(_player.Inventory.BodyParts.Where(part => part is Head));
                break;

            case BodyPartType.Corpus:
                _buffer.Add(_player.Body.Corpus);
                _buffer.AddRange(_player.Inventory.BodyParts.Where(part => part is Corpus));
                break;

            case BodyPartType.Hands:
                _buffer.Add(_player.Body.Hands);
                _buffer.AddRange(_player.Inventory.BodyParts.Where(part => part is Hands));
                break;

            case BodyPartType.Legs:
                _buffer.Add(_player.Body.Legs);
                _buffer.AddRange(_player.Inventory.BodyParts.Where(part => part is Legs));
                break;

            default:
                throw new ArgumentNullException();
        }

        CreateViewsList(_buffer);
    }

    private void CreateViewsList(IReadOnlyList<BodyPart> bodyParts)
    {
        for (int i = 0; i < bodyParts.Count; i++)
        {
            CreateView(bodyParts[i]);
        }

        SetViewAsEquiped(_views[0]);
    }

    private void SetViewAsEquiped(BodyPartInventoryView view)
    {
        view.DisableEquipButton();
        view.DisableSellButton();

        _equiped = view;
    }

    private void SetViewAsNotEquiped(BodyPartInventoryView view)
    {
        view.EnableEquipButton();
        view.EnableSellButton();
    }

    private void OnEquipButtonClick(BodyPart bodyPart, BodyPartInventoryView view)
    {
        SetViewAsNotEquiped(_equiped);
        SetViewAsEquiped(view);

        _player.Equip(bodyPart);
    }

    private void OnSellButtonClick(BodyPart bodyPart, BodyPartInventoryView view)
    {
        _views.Remove(view);
        DestroyView(view);
        _player.Sell(bodyPart);
    }

    private void OnUpgradeButtonClick(BodyPart bodyPart, BodyPartInventoryView view)
    {
        if (_player.Wallet.TryBuy(bodyPart.UpgradePrice))
        {
            bodyPart.Upgrade();
            _player.Body.OnBodyPartUpgraded();
            view.Init(bodyPart);
        }
    }

    private void SubscribeHandlers(BodyPartInventoryView view)
    {
        view.EquipButtonClick += OnEquipButtonClick;
        view.SellButtonClick += OnSellButtonClick;
        view.UpgradeButtonClick += OnUpgradeButtonClick;
    }

    private void UnsubscribeHandlers(BodyPartInventoryView view)
    {
        view.EquipButtonClick -= OnEquipButtonClick;
        view.SellButtonClick -= OnSellButtonClick;
        view.UpgradeButtonClick -= OnUpgradeButtonClick;
    }

    private void DestroyView(BodyPartInventoryView view)
    {
        UnsubscribeHandlers(view);
        Destroy(view.gameObject);
    }

    private void CreateView(BodyPart bodyPart)
    {
        var view = Instantiate(_template, _itemContainer.transform);

        view.Init(bodyPart);
        SubscribeHandlers(view);

        _views.Add(view);
    }

    private void EnableViews()
    {
        foreach (var view in _views)
        {
            view.gameObject.SetActive(true);
        }
    }
}
