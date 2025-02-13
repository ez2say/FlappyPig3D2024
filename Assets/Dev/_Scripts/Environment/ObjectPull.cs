using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private Queue<GameObject> _pool = new Queue<GameObject>();
    private GameObject _prefab;
    private int _poolSize;
    private Transform _parent;

    public ObjectPool(GameObject prefab, int poolSize, Transform parent = null)
    {
        _prefab = prefab;
        _poolSize = poolSize;
        _parent = parent;

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = (GameObject)UnityEngine.Object.Instantiate(_prefab);
            obj.SetActive(false);
            if (_parent != null) obj.transform.SetParent(_parent);
            _pool.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        if (_pool.Count > 0)
        {
            GameObject obj = _pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject newObj = (GameObject)UnityEngine.Object.Instantiate(_prefab);
            if (_parent != null) newObj.transform.SetParent(_parent);
            return newObj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        _pool.Enqueue(obj);
    }
}