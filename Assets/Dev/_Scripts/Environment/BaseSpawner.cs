using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] protected GameObject[] _prefabs;
    [SerializeField] protected BoxCollider _spawnArea;
    [SerializeField] protected float _spawnInterval = 2f;
    [SerializeField] protected float _minDistanceBetweenObjects = 2f;
    [SerializeField] protected int _maxObjectCount = 10;

    protected List<Vector3> _spawnedObjectPositions = new List<Vector3>();

    protected virtual void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    protected abstract IEnumerator SpawnObjects();

    protected Vector3 GetRandomPositionInArea(BoxCollider area)
    {
        Vector3 extents = area.size / 2f;

        Vector3 point = new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y),
            Random.Range(-extents.z, extents.z)
        );

        point = area.transform.TransformPoint(point);

        return point;
    }

    protected void InstantiateObject(Vector3 spawnPosition)
    {
        int randomIndex = GetRandomObjectIndex();

        GameObject obj = CreateObject(randomIndex, spawnPosition);

        AddObjectPosition(spawnPosition);

        obj.transform.SetParent(transform);
    }

    protected int GetRandomObjectIndex()
    {
        return Random.Range(0, _prefabs.Length);
    }

    protected GameObject CreateObject(int index, Vector3 position)
    {
        return Instantiate(_prefabs[index], position, Quaternion.identity);
    }

    protected void AddObjectPosition(Vector3 position)
    {
        _spawnedObjectPositions.Add(position);
    }

    protected bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 spawnedPosition in _spawnedObjectPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < _minDistanceBetweenObjects)
            {
                return false;
            }
        }

        Collider[] hitColliders = Physics.OverlapSphere(position, _minDistanceBetweenObjects);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Obstacle"))
            {
                return false;
            }
        }

        return true;
    }

    public List<Vector3> GetSpawnedObjectPositions()
    {
        return _spawnedObjectPositions;
    }
}