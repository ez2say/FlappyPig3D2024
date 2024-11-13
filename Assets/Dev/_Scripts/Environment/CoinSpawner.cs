using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [Header("Coin Settings")]
    [SerializeField] private GameObject _coinPrefab; // Префаб монетки
    [SerializeField] private BoxCollider _spawnArea; // Область спауна
    [SerializeField] private float _spawnInterval = 2f; // Интервал спауна монеток
    [SerializeField] private float _minDistanceBetweenCoins = 2f; // Минимальное расстояние между монетками
    [SerializeField] private float _minDistanceFromObstacles = 2f; // Минимальное расстояние от препятствий
    [SerializeField] private int _maxCoinCount = 10; // Максимальное количество монеток в сегменте

    private List<Vector3> _spawnedCoinPositions = new List<Vector3>();
    private List<Vector3> _obstaclePositions; // Список позиций препятствий

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
            if (_spawnedCoinPositions.Count < _maxCoinCount)
            {
                Vector3 spawnPosition = GetRandomPositionInArea(_spawnArea);
                if (IsValidSpawnPosition(spawnPosition))
                {
                    InstantiateCoin(spawnPosition);
                }
            }
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private Vector3 GetRandomPositionInArea(BoxCollider area)
    {
        Vector3 extents = area.size / 2f;

        Vector3 point = new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(5.5f, 80f), // Генерируем y в пределах области спауна
            Random.Range(-extents.z, extents.z)
        );

        point = area.transform.TransformPoint(point);

        return point;
    }

    private bool IsValidSpawnPosition(Vector3 position)
    {
        // Проверяем, что позиция находится на достаточном расстоянии от уже созданных монеток
        foreach (Vector3 coinPosition in _spawnedCoinPositions)
        {
            if (Vector3.Distance(coinPosition, position) < _minDistanceBetweenCoins)
            {
                return false;
            }
        }

        // Проверяем, что позиция находится на достаточном расстоянии от препятствий
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