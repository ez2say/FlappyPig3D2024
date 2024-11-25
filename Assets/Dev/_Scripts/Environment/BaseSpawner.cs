using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [SerializeField] protected GameObject[] _prefabs;

    [SerializeField] protected float _spawnInterval = 2f;
    
    [SerializeField] protected int _maxObjectCount = 10;

    protected List<Vector3> _spawnedObjectPositions = new List<Vector3>();

    protected virtual void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    protected abstract IEnumerator SpawnObjects();

    protected virtual GameObject InstantiateObject(Vector3 spawnPosition)
    {
        int randomIndex = GetRandomObjectIndex();

        GameObject obj = CreateObject(randomIndex, spawnPosition);

        AddObjectPosition(spawnPosition);

        obj.transform.SetParent(transform);

        return obj;
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

    public List<Vector3> GetSpawnedObjectPositions()
    {
        return _spawnedObjectPositions;
    }
}
