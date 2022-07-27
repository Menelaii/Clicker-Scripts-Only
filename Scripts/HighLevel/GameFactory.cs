using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameFactory
{
    private readonly Prefabs _prefabs;
    private readonly Player _player;
    private readonly ItemThrower _itemThrower;
    private readonly TextViewSpawner _textViewSpawner;
    private readonly BodyPartsGenerationStaticData _generationStaticData;
    private readonly Transform _optionalPanelsParent;

    public readonly AttackResultViewer AttackResultViewer;
    public readonly LootSpawner LootSpawner;

    public event Action<LootBox> LootBoxCreated;
    public event Action<Combatant> EnemyCreated;

    public GameFactory(Prefabs prefabs, Player player, ItemThrower itemThrower,
        BodyPartsGenerationStaticData generationStaticData, Transform optionalPanelsParent,
        Transform poolObjectsParent)
    {
        _prefabs = prefabs;
        _player = player;
        _itemThrower = itemThrower;
        _generationStaticData = generationStaticData;
        AttackResultViewer = new AttackResultViewer(this, itemThrower);
        LootSpawner = new LootSpawner(this, _itemThrower);
        _optionalPanelsParent = optionalPanelsParent;
        _textViewSpawner = new TextViewSpawner(prefabs, poolObjectsParent);
    }

    public Combatant CreateEnemy(EnemyStaticData staticData, Vector3 position)
    {
        Health health = new Health(staticData.MaxHealth);
        AutoAttacker autoAttacker = new AutoAttacker(staticData.AttackCooldown, staticData.AttackCooldownSpread);
        SpellUser abilityUser = new SpellUser(new Mana(staticData.MaxMana), staticData.ManaRegen);

        Combatant combatant = Object.Instantiate(staticData.Prefab, position, Quaternion.identity);
        combatant.Init(abilityUser, autoAttacker, health, staticData.Damage, staticData.Armor, staticData.CritDamageInPercent,
            staticData.CritChance,staticData.DodgeChance, staticData.AttackModifiers);

        LootSpawner.OnEnemyCreated(staticData.Loot, position, _player.Traveler.IsOnLastBoughtWave);
        health.Died += LootSpawner.OnEnemyDied;

        EnemyAnimator animator = combatant.GetComponent<EnemyAnimator>();
        animator.Init(AttackResultViewer);

        combatant.SetTarget(_player.Combatant);
        _player.Combatant.SetTarget(combatant);

        EnemyCreated?.Invoke(combatant);

        return combatant;
    }

    public StormCloud CreateStormCloud(Vector3 position)
    {
        return Object.Instantiate(_prefabs.StormCloud, position, Quaternion.identity);
    }

    public AccumulatorEntity CreateAccumulatorEntity()
    {
        return Object.Instantiate(_prefabs.HealthAccumulator, _optionalPanelsParent);
    }

    public Combatant CreateBoss(EnemyStaticData staticData, Vector3 position)
    {
        var combatant = CreateEnemy(staticData, position);
        combatant.Health.Died += _player.OnBossDefeated;
        return combatant;
    }

    public Coin[] CreateCoins(CoinDrop[] coinDrops, Vector3 position)
    {
        int count = 0;
        foreach (var coinDrop in coinDrops)
        {
            count += coinDrop.Amount;
        }

        List<Coin> coins = new List<Coin>(count);

        for (int i = 0; i < coinDrops.Length; i++)
        {
            for (int j = 0; j < coinDrops[i].Amount; j++)
            {
                var coin = Object.Instantiate(coinDrops[i].Prefab, position, Quaternion.identity);
                coin.Destroyed += _player.OnCoinDestroyed;
                coin.Destroyed += OnCoinDestroyed;
                coins.Add(coin);
            }
        }

        return coins.ToArray();
    }

    public LootBox CreateLootBox(LootBox prefab, BodyPart bodyPartloot, Vector3 position)
    {
        LootBox lootBox = Object.Instantiate(prefab, position, Quaternion.identity);
        bodyPartloot.OnDrop(_generationStaticData.UpgradesPerWave, _player.Traveler.CurrentWaveGlobalIndex, _generationStaticData.StatsMaxSpreadPercent);
        lootBox.Init(bodyPartloot);

        LootBoxCreated?.Invoke(lootBox);

        return lootBox;
    }

    public DamageView SpawnDamageView(int damage, DamageType type, Vector3 position)
    {
        return _textViewSpawner.SpawnDamageView(damage, type, position);
    }

    public FlyingText SpawnDodgedMessage(Vector3 position)
    {
        return _textViewSpawner.SpawnDodgedMessage(position);
    }

    public FlyingText SpawnCoinValueView(int value, Vector3 position)
    {
        return _textViewSpawner.SpawnCoinValueView(value, position);
    }

    public PlayerView CreatePlayerView(Vector3 position)
    {
        var view = Object.Instantiate(_prefabs.PlayerView, position, Quaternion.identity);
        view.GetComponent<PlayerAnimator>().Init(AttackResultViewer);

        return view;
    }

    private void OnCoinDestroyed(Coin coin)
    {
        coin.Destroyed -= OnCoinDestroyed;

        SpawnCoinValueView(coin.Value, coin.transform.position);
    }
}
