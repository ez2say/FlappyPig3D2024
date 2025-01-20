using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingDroneSpawner : BaseSpawner
{
    [Header("StandingDrone Spawner Settings")]
    [SerializeField] private BoxCollider _groundCollider;

    [SerializeField] private float _spawnYOffsetMin = 5f;

    [SerializeField] private float _spawnYOffsetMax = 10f;

    [SerializeField] protected float _minDistanceBetweenObjects = 2f;

    protected override IEnumerator SpawnObjects()
    {
        while(true)
        {
            if(_spawnedObjectPositions.Count < _maxObjectCount)
            {
                Vector3 spawnPosition = GetRandomPositionOnGround();
                if(IsPositionValid(spawnPosition))
                {
                    SpawnDrone(spawnPosition);
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

    private void SpawnDrone(Vector3 basePosition)
    {
        InstantiateObject(basePosition);
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
