using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : BaseSpawner
{
    [Header("Wall Spawner Settings")]
    [SerializeField] private float _spawnYOffset = 0f;
    [SerializeField] private float _spawnZOffset = 0f;

    [SerializeField] private float _spawnXStartOffset = 0f;

    [SerializeField] private float _spawnXEndOffset = 0f;

    protected override IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (_spawnedObjectPositions.Count < _maxObjectCount)
            {
                Vector3 spawnPosition = GetRandomPositionOnWall();

                if (IsPositionValid(spawnPosition))
                {
                    InstantiateObject(spawnPosition);
                }
            }

            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private Vector3 GetRandomPositionOnWall()
    {
        Vector3 extents = _spawnArea.size / 2f;

        float randomX = Random.Range(_spawnXStartOffset, _spawnXEndOffset);
        float randomY = Random.Range(-extents.y, extents.y) + _spawnYOffset;
        float randomZ = Random.Range(-extents.z, extents.z) + _spawnZOffset;

        Vector3 point = new Vector3(randomX, randomY, randomZ);
        point = _spawnArea.transform.TransformPoint(point);

        return point;
    }
}