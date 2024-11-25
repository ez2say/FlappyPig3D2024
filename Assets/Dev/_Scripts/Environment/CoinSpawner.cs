using System.Collections;
using UnityEngine;

public class CoinSpawner : BaseSpawner
{
    [Header("Coin Spawner Settings")]
    [SerializeField] private BoxCollider _groundCollider;

    [SerializeField] private float _spawnYOffsetMin = 5f;

    [SerializeField] private float _spawnYOffsetMax = 10f;

    [SerializeField] protected float _minDistanceBetweenObjects = 2f;

    [SerializeField] private int _coinsInRow = 3;

    [SerializeField] private float _rowSpacing = 1f;

    [SerializeField] private float _triangleSpacing = 1f;

    [SerializeField] private float _crossSpacing = 1f;

    [SerializeField] private float _rowSpawnChance = 0.5f;

    [SerializeField] private float _triangleSpawnChance = 0.25f;
    
    [SerializeField] private float _crossSpawnChance = 0.25f;

    protected override IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (_spawnedObjectPositions.Count < _maxObjectCount)
            {
                Vector3 spawnPosition = GetRandomPositionOnGround();

                if (IsPositionValid(spawnPosition))
                {
                    SpawnCoins(spawnPosition);
                }
            }

            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private Vector3 GetRandomPositionOnGround()
    {
        Vector3 extents = _groundCollider.size / 2f;

        float randomX = Random.Range(-extents.x, extents.x);
        float randomY = Random.Range(_spawnYOffsetMin, _spawnYOffsetMax);
        float randomZ = Random.Range(-extents.z, extents.z);

        Vector3 point = new Vector3(randomX, randomY, randomZ);
        point = _groundCollider.transform.TransformPoint(point);

        return point;
    }

    private void SpawnCoins(Vector3 basePosition)
    {
        float randomValue = Random.value;

        if (randomValue < _rowSpawnChance)
        {
            SpawnCoinsInRow(basePosition);
        }
        else if (randomValue < _rowSpawnChance + _triangleSpawnChance)
        {
            SpawnCoinsInTriangle(basePosition);
        }
        else if (randomValue < _rowSpawnChance + _crossSpawnChance)
        {
            SpawnCoinsInCross(basePosition);
        }
    }

    private void SpawnCoinsInRow(Vector3 basePosition)
    {
        for (int i = 0; i < _coinsInRow; i++)
        {
            Vector3 spawnPosition = basePosition + new Vector3(0, 0, i * _rowSpacing);
            if (IsPositionValid(spawnPosition))
            {
                InstantiateObject(spawnPosition);
            }
        }
    }

    private void SpawnCoinsInTriangle(Vector3 basePosition)
    {
        for (int i = 0; i < _coinsInRow; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                Vector3 spawnPosition = basePosition + new Vector3(i * _triangleSpacing, j * _triangleSpacing, 0);
                if (IsPositionValid(spawnPosition))
                {
                    InstantiateObject(spawnPosition);
                }
            }
        }
    }

    private void SpawnCoinsInCross(Vector3 basePosition)
    {
        Vector3[] crossPositions = new Vector3[]
        {
            basePosition + new Vector3(0, _crossSpacing, 0), // Верх
            basePosition + new Vector3(0, -_crossSpacing, 0), // Низ
            basePosition + new Vector3(-_crossSpacing, 0, 0), // Лево
            basePosition + new Vector3(_crossSpacing, 0, 0) // Право
        };

        foreach (Vector3 spawnPosition in crossPositions)
        {
            if (IsPositionValid(spawnPosition))
            {
                InstantiateObject(spawnPosition);
            }
        }
    }

    private bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 spawnedPosition in _spawnedObjectPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < _minDistanceBetweenObjects)
            {
                return false;
            }
        }

        if (Physics.CheckSphere(position, _minDistanceBetweenObjects))
        {
            return false;
        }

        return true;
    }
}