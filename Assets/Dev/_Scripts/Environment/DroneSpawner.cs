using System.Collections;
using UnityEngine;

public class DroneSpawner : BaseSpawner
{
    [Header("Drone Spawner Settings")]
    [SerializeField] private float _spawnYOffsetMin = 5f;
    [SerializeField] private float _spawnYOffsetMax = 10f;

    protected override IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (_spawnedObjectPositions.Count < _maxObjectCount)
            {
                Vector3 spawnPosition = GetRandomPositionOnDrone();

                if (IsPositionValid(spawnPosition))
                {
                    InstantiateObject(spawnPosition);
                }
            }

            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private Vector3 GetRandomPositionOnDrone()
    {
        Vector3 extents = _spawnArea.size / 2f;

        float randomX = Random.Range(-extents.x, extents.x);
        float randomY = Random.Range(_spawnYOffsetMin, _spawnYOffsetMax);
        float randomZ = Random.Range(-extents.z, extents.z);

        Vector3 point = new Vector3(randomX, randomY, randomZ);
        point = _spawnArea.transform.TransformPoint(point);

        return point;
    }
}