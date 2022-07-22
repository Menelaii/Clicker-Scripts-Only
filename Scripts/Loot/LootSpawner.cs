using UnityEngine;

public class LootSpawner
{
    private Loot _loot;
    private GameFactory _gameFactory;
    private ItemThrower _itemThrower;
    private bool _isLootBoxCanDrop;
    private Vector3 _spawnPosition;

    public LootSpawner(GameFactory gameFactory, ItemThrower itemThrower)
    {
        _gameFactory = gameFactory;
        _itemThrower = itemThrower;
    }

    public void OnEnemyCreated(Loot loot, Vector3 position, bool isLootBoxCanDrop)
    {
        _loot = loot;
        _spawnPosition = position;
        _isLootBoxCanDrop = isLootBoxCanDrop;
    }

    public void OnEnemyDied(Health enemy)
    {
        enemy.Died -= OnEnemyDied;

        DropCoins(_spawnPosition);

        TryDropLootBox(_spawnPosition);
    }

    private void TryDropLootBox(Vector3 position)
    {
        if (_isLootBoxCanDrop && Random.Range(1, 100) < _loot.LootBoxDropChance)
        {
            BodyPart bodyPart = _loot.GetRandomBodyPart();
            LootBox lootBox = _gameFactory.CreateLootBox(_loot.LootBoxPrefab, bodyPart, position);

            _itemThrower.Throw(lootBox.GetComponent<Rigidbody2D>());
        }
    }

    private void DropCoins(Vector3 position)
    {
        Coin[] coins = _gameFactory.CreateCoins(_loot.CoinDrops, position);
        Rigidbody2D[] bodies = GetRigidBodiesFrom(coins);

        _itemThrower.Throw(bodies);
    }

    private static Rigidbody2D[] GetRigidBodiesFrom(Coin[] coins)
    {
        Rigidbody2D[] bodies = new Rigidbody2D[coins.Length];
        for (int i = 0; i < coins.Length; i++)
        {
            bodies[i] = coins[i].GetComponent<Rigidbody2D>();
        }

        return bodies;
    }
}
