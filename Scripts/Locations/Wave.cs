using UnityEngine;

[System.Serializable]
public class Wave
{
    [SerializeField] private EnemyStaticData[] _templates;

    public EnemyStaticData GetRandomEnemy()
    {
        return _templates[Random.Range(0, _templates.Length)];
    }
}
