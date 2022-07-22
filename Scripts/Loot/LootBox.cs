using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
public class LootBox : MonoBehaviour, IPointerDownHandler
{
    private BodyPart _loot;

    public event Action<LootBox, BodyPart> Opened;

    public void Init(BodyPart loot)
    {
        _loot = loot;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Open();
    }

    private void Open()
    {
        Opened?.Invoke(this, _loot);
        Destroy(gameObject);
    }
}
