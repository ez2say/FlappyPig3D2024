using System.Collections;
using UnityEngine;

public class CeilingSpawner : BaseSpawner
{
    [Header("Ceiling Spawner Settings")]
    [SerializeField] private float _spawnYOffset = 0f;

    protected override IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (_spawnedObjectPositions.Count < _maxObjectCount)
            {
                Vector3 spawnPosition = GetRandomPositionOnCeiling();

                if (IsPositionValid(spawnPosition))
                {
                    InstantiateObject(spawnPosition);
                }
            }

            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private Vector3 GetRandomPositionOnCeiling()
    {
        Vector3 extents = _spawnArea.size / 2f;

        float randomX = Random.Range(-extents.x, extents.x);
        float randomY = extents.y + _spawnYOffset;
        float randomZ = Random.Range(-extents.z, extents.z);

        Vector3 point = new Vector3(randomX, randomY, randomZ);
        point = _spawnArea.transform.TransformPoint(point);

        return point;
    }
}
    
