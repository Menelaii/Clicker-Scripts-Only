using UnityEngine;

[System.Serializable]
public class SetBonus
{
    [SerializeField] private SetId _setId;
    [SerializeField] private Bonus[] _bonuses;

    public SetId SetId => _setId;

    public void EnableOn(Body body)
    {
        foreach (var bonus in _bonuses)
        {
            if (bonus == null)
                continue;

            bonus.EnableOn(body);
        }
    }

    public void DisableOn(Body body)
    {
        foreach (var bonus in _bonuses)
        {
            if (bonus == null)
                continue;

            bonus.DisableOn(body);
        }
    }
}
