using System;

public class Player : IBodyChangesHandler
{
    public Health Health { get; private set; }
    public HealthRegeneration HealthRegeneration { get; private set; }
    public Combatant Combatant { get; private set; }
    public Wallet Wallet { get; private set; }
    public Traveler Traveler { get; private set; }
    public Inventory Inventory { get; private set; }
    public Body Body { get; private set; }
    public int BossesDefeated { get; private set; }

    public Player(PlayerProgress progress, Combatant combatant, float autoAttackDamageCut)
    {
        Body = progress.Body;
        Traveler = progress.Traveler;
        Wallet = progress.Wallet;
        Inventory = progress.Inventory;
        Health = new Health(Body.Corpus.MaxHealth, progress.Health);
        BossesDefeated = progress.BossesDefeated;
        HealthRegeneration = new HealthRegeneration(Health, Body.Corpus.HealthRegeneration);
        HealthRegeneration.StartRegeneration();

        Mana mana = new Mana(progress.Mana, Body.Head.MaxMana);
        SpellUser abilityUser = new SpellUser(mana, Body.Head.ManaRegeneration);
        AutoAttacker autoAttacker = new AutoAttacker(Body.Hands.AutoAttackCooldown, 0, autoAttackDamageCut);

        Combatant = combatant;
        Combatant.Init(Body, Health, autoAttacker, abilityUser);
    }

    public void Sell(BodyPart bodyPart)
    {
        Inventory.Remove(bodyPart);
        Wallet.Add(bodyPart.Price);
    }

    public void AddBoughtWave()
    {
        Traveler.OnWaveBought();
    }

    public void Equip(BodyPart bodyPart)
    {
        Inventory.Remove(bodyPart);
        var replaced = Body.Replace(bodyPart);
        Inventory.Add(replaced);
    }

    public void OnCoinDestroyed(Coin coin)
    {
        coin.Destroyed -= OnCoinDestroyed;
        Wallet.Add(coin.Value);
    }

    public void OnBodyChanged(Body body)
    {
        Combatant.OnBodyChanged(body);
        HealthRegeneration.OnBodyChanged(body);
    }

    public void OnBossDefeated(Health boss)
    {
        boss.Died -= OnBossDefeated;

        BossesDefeated++;
    }

    public PlayerProgress GetProgress()
    {
        return new PlayerProgress()
        {
            Health = Combatant.Health.Value,
            Mana = Combatant.SpellUser.Mana.Value,
            Wallet = Wallet,
            Inventory = Inventory,
            Body = Body,
            Traveler = Traveler,
            BossesDefeated = BossesDefeated
        };
    }
}
