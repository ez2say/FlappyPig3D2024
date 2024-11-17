using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _roadPrefab;
    private List<GameObject> _roads = new List<GameObject>();
    [SerializeField] private int _maxRoadCount = 5;
    [SerializeField] private float _segmentLength = 100f; // Длина сегмента туннеля
    [SerializeField] private Transform _cameraTransform;

    private void Start()
    {
        ResetLevel();
    }

    private void Update()
    {
        if (_roads.Count == 0) return;

        if (_roads[0].transform.position.z < _cameraTransform.position.z - _segmentLength)
        {
            Destroy(_roads[0]);
            _roads.RemoveAt(0);
            CreateNextRoad();
        }

        if (_roads[_roads.Count - 1].transform.position.z < _cameraTransform.position.z + _segmentLength * (_maxRoadCount - 1))
        {
            CreateNextRoad();
        }
    }

    private void CreateNextRoad()
    {
        Vector3 pos = Vector3.zero;

        if (_roads.Count > 0)
        {
            pos = _roads[_roads.Count - 1].transform.position + new Vector3(0, 0, _segmentLength);
        }

        GameObject generate = Instantiate(_roadPrefab, pos, Quaternion.identity);
        generate.transform.SetParent(transform);
        _roads.Add(generate);
    }

    public void ResetLevel()
    {
        while (_roads.Count > 0)
        {
            Destroy(_roads[0]);
            _roads.RemoveAt(0);
        }
        for (int i = 0; i < _maxRoadCount; i++)
        {
            CreateNextRoad();
        }
    }
}