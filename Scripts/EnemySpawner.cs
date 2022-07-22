using System.Collections;
using UnityEngine;

public class EnemySpawner
{
    private GameFactory _factory;
    private Vector2 _spawnPoint;
    private Wave _currentWave;
    private float _spawnDelay;
    private Combatant _lastSpawned;

    public EnemySpawner(GameFactory factory, Vector2 spawnPoint, Wave defaultWave, float spawnDelay)
    {
        _factory = factory;
        _spawnPoint = spawnPoint;
        _currentWave = defaultWave;
        _spawnDelay = spawnDelay;
    }

    public void SetWave(Wave wave, Vector2 spawnPoint)
    {
        _currentWave = wave;
        _spawnPoint = spawnPoint;
    }

    public IEnumerator SpawnEnemyWithWaveDelay()
    {
        yield return new WaitForSeconds(_spawnDelay);
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        if (_lastSpawned != null)
            TryDestroyLastSpawned();

        _lastSpawned = _factory.CreateEnemy(_currentWave.GetRandomEnemy(), _spawnPoint);
    }

    public void SpawnBoss(EnemyStaticData enemy)
    {
        if (_lastSpawned != null)
            TryDestroyLastSpawned();

        _lastSpawned = _factory.CreateBoss(enemy, _spawnPoint);
    }

    public void TryDestroyLastSpawned()
    {
        if (_lastSpawned != null)
            Object.Destroy(_lastSpawned.gameObject);
    }
}
