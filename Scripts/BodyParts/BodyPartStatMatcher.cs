using System.Collections.Generic;

static class BodyPartStatMatcher
{
    private static Dictionary<string, BodyPartStatType> _map = new Dictionary<string, BodyPartStatType>
    {
        [nameof(BodyPartStatType.MaxMana)] = BodyPartStatType.MaxMana,
        [nameof(BodyPartStatType.ManaRegeneration)] = BodyPartStatType.ManaRegeneration,
        [nameof(BodyPartStatType.MaxHealth)] = BodyPartStatType.MaxHealth,
        [nameof(BodyPartStatType.Armor)] = BodyPartStatType.Armor,
        [nameof(BodyPartStatType.HealthRegeneration)] = BodyPartStatType.HealthRegeneration,
        [nameof(BodyPartStatType.Damage)] = BodyPartStatType.Damage,
        [nameof(BodyPartStatType.AutoAttackCooldown)] = BodyPartStatType.AutoAttackCooldown,
        [nameof(BodyPartStatType.CritChance)] = BodyPartStatType.CritChance,
        [nameof(BodyPartStatType.CritDamageInPercent)] = BodyPartStatType.CritDamageInPercent,
        [nameof(BodyPartStatType.DodgeChance)] = BodyPartStatType.DodgeChance,
        [nameof(BodyPartStatType.Price)] = BodyPartStatType.Price,
        [nameof(BodyPartStatType.UpgradePrice)] = BodyPartStatType.UpgradePrice
    };

    public static BodyPartStatType GetMatch(string fieldName)
    {
        return _map[ToPascalCase(fieldName)];
    }

    private static string ToPascalCase(string fieldName)
    {
        char[] chars = fieldName.ToCharArray(1, fieldName.Length - 1);
        chars[0] = char.ToUpper(chars[0]);

        return new string(chars);
    }
}
