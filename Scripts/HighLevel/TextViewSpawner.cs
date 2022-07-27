using UnityEngine;

public class TextViewSpawner
{
    private static readonly int DEFAULT_DODGE_MESSAGE_POOL_SIZE = 5;
    private static readonly int DEFAULT_COIN_VALUE_POOL_SIZE = 5;
    private static readonly int DEFAULT_DAMAGE_VIEW_POOL_SIZE = 10;

    private readonly ObjectsPool<FlyingText> _dodgeMessages;
    private readonly ObjectsPool<FlyingText> _coinValueViews;
    private readonly ObjectsPool<DamageView> _damageViews;

    public TextViewSpawner(Prefabs prefabs, Transform objectsParent)
    {
        _dodgeMessages = new ObjectsPool<FlyingText>(
            () => (FlyingText)CreateTextView(objectsParent, prefabs.DodgeView),
            DEFAULT_DODGE_MESSAGE_POOL_SIZE);

        _coinValueViews = new ObjectsPool<FlyingText>(
            () => (FlyingText)CreateTextView(objectsParent, prefabs.TotalValueView),
            DEFAULT_COIN_VALUE_POOL_SIZE);

        _damageViews = new ObjectsPool<DamageView>(
            () => (DamageView)CreateTextView(objectsParent, prefabs.DamageView),
            DEFAULT_DAMAGE_VIEW_POOL_SIZE);
    }

    public FlyingText SpawnDodgedMessage(Vector3 position)
    {
        FlyingText message = _dodgeMessages.GetItem();
        message.Init(() => ReturnToPool(message, _dodgeMessages));
        return PlaceObject(message, position);
    }

    public FlyingText SpawnCoinValueView(int value, Vector3 position)
    {
        FlyingText text = _coinValueViews.GetItem();
        text.Init(value.ToString(), () => ReturnToPool(text, _coinValueViews));
        return PlaceObject(text, position);
    }

    public DamageView SpawnDamageView(int damage, DamageType type, Vector3 position)
    {
        DamageView view = _damageViews.GetItem();
        view.Init(damage.ToString(), type, () => ReturnToPool(view, _damageViews));
        return PlaceObject(view, position);
    }

    private T PlaceObject<T>(T monoBehaviour, Vector3 position) where T : MonoBehaviour
    {
        monoBehaviour.gameObject.SetActive(true);
        monoBehaviour.transform.position = position;

        return monoBehaviour;
    }

    private void ReturnToPool<T>(T monoBehaviour, ObjectsPool<T> objectsPool) where T : MonoBehaviour
    {
        monoBehaviour.gameObject.SetActive(false);
        objectsPool.ReleaseItem(monoBehaviour);
    }

    private TextView CreateTextView(Transform parent, TextView prefab)
    {
        TextView view = Object.Instantiate(prefab, parent);
        view.transform.rotation = Quaternion.identity;
        view.gameObject.SetActive(false);

        return view;
    }
}
