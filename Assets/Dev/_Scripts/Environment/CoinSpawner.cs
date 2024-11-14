using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [Header("Coin Settings")]
    [SerializeField] private GameObject _coinPrefab;

    [SerializeField] private BoxCollider _spawnArea;

    [SerializeField] private float _spawnInterval = 2f;

    [SerializeField] private float _minDistanceBetweenCoins = 2f;

    [SerializeField] private float _minDistanceFromObstacles = 2f;

    [SerializeField] private int _maxCoinCount = 10;


    private List<Vector3> _spawnedCoinPositions = new List<Vector3>();

    private List<Vector3> _obstaclePositions;

    public void SetObstaclePositions(List<Vector3> obstaclePositions)
    {
        _obstaclePositions = obstaclePositions;
    }

    void Start()
    {
        StartCoroutine(SpawnCoins());
    }

    IEnumerator SpawnCoins()
    {
        while (true)
        {
            if (CheckCoinCount())
            {
                Vector3 spawnPosition = GenerateRandomPosition();
                if (IsValidSpawnPosition(spawnPosition))
                {
                    InstantiateCoin(spawnPosition);
                }
            }
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private bool CheckCoinCount()
    {
        return _spawnedCoinPositions.Count < _maxCoinCount;
    }

    private Vector3 GenerateRandomPosition()
    {
        return GetRandomPositionInArea(_spawnArea);
    }

    private Vector3 GetRandomPositionInArea(BoxCollider area)
    {
        Vector3 extents = CalculateExtents(area);

        Vector3 localPoint = GenerateRandomLocalPoint(extents);

        Vector3 worldPoint = TransformToWorldPoint(area, localPoint);

        return worldPoint;
    }

    private Vector3 CalculateExtents(BoxCollider area)
    {
        return area.size / 2f;
    }

    private Vector3 GenerateRandomLocalPoint(Vector3 extents)
    {
        return new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(5.5f, 80f),
            Random.Range(-extents.z, extents.z)
        );
    }

    private Vector3 TransformToWorldPoint(BoxCollider area, Vector3 localPoint)
    {
        return area.transform.TransformPoint(localPoint);
    }

    private bool IsValidSpawnPosition(Vector3 position)
    {
        if (!CheckDistanceToCoins(position))
        {
            return false;
        }

        if (!CheckDistanceToObstacles(position))
        {
            return false;
        }

        return true;
    }

    private bool CheckDistanceToCoins(Vector3 position)
    {
        foreach (Vector3 coinPosition in _spawnedCoinPositions)
        {
            if (Vector3.Distance(coinPosition, position) < _minDistanceBetweenCoins)
            {
                return false;
            }
        }
        return true;
    }

    private bool CheckDistanceToObstacles(Vector3 position)
    {
        if (_obstaclePositions != null)
        {
            foreach (Vector3 obstaclePosition in _obstaclePositions)
            {
                if (Vector3.Distance(obstaclePosition, position) < _minDistanceFromObstacles)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void InstantiateCoin(Vector3 spawnPosition)
    {
        GameObject coin = Instantiate(_coinPrefab, spawnPosition, Quaternion.identity);
        
        _spawnedCoinPositions.Add(spawnPosition);
    }
}