using UnityEngine;

public class AttackResultViewer
{
    private GameFactory _gameFactory;
    private ItemThrower _itemThrower;

    public AttackResultViewer(GameFactory gameFactory, ItemThrower itemThrower)
    {
        _gameFactory = gameFactory;
        _itemThrower = itemThrower;
    }

    public void DropDamageView(int damage, DamageType type, Vector3 position)
    {
        DamageView view = _gameFactory.SpawnDamageView(damage, type, position);
        _itemThrower.Throw(view.GetComponent<Rigidbody2D>());
    }

    public void DropDodgeView(Vector3 position)
    {
        _gameFactory.SpawnDodgedMessage(position);
    }
}
