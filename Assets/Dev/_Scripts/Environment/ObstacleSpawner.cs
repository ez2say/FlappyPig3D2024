using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Settings")]
    [SerializeField] private GameObject[] _obstaclePrefabs; // Список префабов препятствий

    [SerializeField] private BoxCollider _spawnArea; // Область спауна

    [SerializeField] private float _spawnInterval = 2f; // Интервал спауна препятствий

    [SerializeField] private float _minDistanceBetweenObstacles = 2f; // Минимальное расстояние между препятствиями

    private List<Vector3> _spawnedObstaclePositions = new List<Vector3>();

    private float _lastSpawnZ; // Позиция Z последнего спауна

    void Start()
    {
        _lastSpawnZ = _spawnArea.bounds.min.z;
        StartCoroutine(SpawnObstacles());
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            Vector3 spawnPosition = GetRandomPositionInArea(_spawnArea);
            yield return new WaitForSeconds(_spawnInterval);
            InstantiateObstacle(spawnPosition);
        }
    }

    private Vector3 GetRandomPositionInArea(BoxCollider area)
    {
        Vector3 extents = area.size / 2f;

        Vector3 point = new Vector3(
            Random.Range(-extents.x, extents.x),
            5.5f, // Генерируем y в пределах области спауна
            Random.Range(-extents.z, extents.z)
        );

        point = area.transform.TransformPoint(point);

        return point;
    }

    private void InstantiateObstacle(Vector3 spawnPosition)
    {
        int randomIndex = GetRandomObstacleIndex();

        GameObject obstacle = CreateObstacleObject(randomIndex, spawnPosition);

        // Проверка и корректировка позиции препятствия
        AdjustObstaclePosition(obstacle);

        AddObstaclePosition(spawnPosition);
    }

    private int GetRandomObstacleIndex()
    {
        return Random.Range(0, _obstaclePrefabs.Length);
    }

    private GameObject CreateObstacleObject(int index, Vector3 position)
    {
        return Instantiate(_obstaclePrefabs[index], position, Quaternion.identity);
    }

    private void AddObstaclePosition(Vector3 position)
    {
        _spawnedObstaclePositions.Add(position);
    }

    private void AdjustObstaclePosition(GameObject obstacle)
    {
        Collider obstacleCollider = obstacle.GetComponent<Collider>();
        if (obstacleCollider != null)
        {
            Vector3 obstaclePosition = obstacle.transform.position;
            float obstacleHeight = obstacleCollider.bounds.size.y;

            // Корректировка позиции, чтобы препятствие было на земле
            obstaclePosition.y = 10f + obstacleHeight / 2f;
            obstacle.transform.position = obstaclePosition;
        }
    }

    // Метод для получения списка позиций спаунов препятствий
    public List<Vector3> GetSpawnedObstaclePositions()
    {
        return _spawnedObstaclePositions;
    }
}