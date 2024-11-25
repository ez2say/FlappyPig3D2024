using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{   
    public int SpawnedSegmentCount { get; private set; } = 0;

    public delegate void NewSegmentAddedHandler(GameObject newSegment);
    
    public event NewSegmentAddedHandler OnNewSegmentAdded;

    [SerializeField] private GameObject _roadPrefab;

    [SerializeField] private GameObject _specialRoadPrefab;

    private List<GameObject> _roads = new List<GameObject>();

    [SerializeField] private int _maxRoadCount = 5;

    [SerializeField] private float _segmentLength = 100f;

    [SerializeField] private int _specialZoneLength = 3;

    private int _normalSegmentCount = 0;

    private bool _specialZoneSpawned = false;

    private int _specialZoneSegmentIndex = 0;

    

    private void Start()
    {
        ResetLevel();
    }

    private void Update()
    {
        if (_roads.Count == 0) return;

        if (_roads[0].transform.position.z < Camera.main.transform.position.z - _segmentLength)
        {
            DestroyRoadSegment();
        }

        if (_roads[_roads.Count - 1].transform.position.z < Camera.main.transform.position.z + _segmentLength * (_maxRoadCount - 1))
        {
            CreateNextRoad();
        }
    }

    private void DestroyRoadSegment()
    {
        Destroy(_roads[0]);

        _roads.RemoveAt(0);

        CreateNextRoad();
    }

    private void CreateNextRoad()
    {
        Vector3 pos = Vector3.zero;

        if (_roads.Count > 0)
        {
            pos = _roads[_roads.Count - 1].transform.position + new Vector3(0, 0, _segmentLength);
        }

        GameObject generate = InstantiateRoadSegment(pos);

        generate.transform.SetParent(transform);

        _roads.Add(generate);

        OnNewSegmentAdded?.Invoke(generate);
    }

    private GameObject InstantiateRoadSegment(Vector3 pos)
    {
        GameObject generate;

        if (_specialZoneSpawned)
        {
            generate = Instantiate(_specialRoadPrefab, pos, Quaternion.identity);
            
            ManageSpecialZoneSegment(generate);
        }
        else
        {
            generate = Instantiate(_roadPrefab, pos, Quaternion.identity);

            SpawnedSegmentCount++;
            
            _normalSegmentCount++;

            Debug.Log($" Сегментов - {SpawnedSegmentCount}");

            if (_normalSegmentCount >= 6)
            {
                _normalSegmentCount = 0;

                _specialZoneSpawned = true;
            }
        }

        return generate;
    }

    private void ManageSpecialZoneSegment(GameObject generate)
    {
        _specialZoneSegmentIndex++;

        SpecialZoneSegment specialZoneSegment = generate.GetComponent<SpecialZoneSegment>();
        if (specialZoneSegment != null)
        {
            if (_specialZoneSegmentIndex == 1)
            {
                specialZoneSegment.enterCollider.gameObject.SetActive(true);

                specialZoneSegment.exitCollider.gameObject.SetActive(false);
            }
            else if (_specialZoneSegmentIndex == _specialZoneLength)
            {
                specialZoneSegment.enterCollider.gameObject.SetActive(true);

                specialZoneSegment.exitCollider.gameObject.SetActive(true);

                _specialZoneSpawned = false;

                _specialZoneSegmentIndex = 0;
            }
            else
            {
                specialZoneSegment.enterCollider.gameObject.SetActive(true);

                specialZoneSegment.exitCollider.gameObject.SetActive(false);
            }
        }
    }

    public void ResetLevel()
    {
        ClearRoads();

        InitializeLevel();
    }

    private void ClearRoads()
    {
        while (_roads.Count > 0)
        {
            Destroy(_roads[0]);

            _roads.RemoveAt(0);
        }
    }

    private void InitializeLevel()
    {
        SpawnedSegmentCount = 0;

        _normalSegmentCount = 0;

        _specialZoneSpawned = false;

        _specialZoneSegmentIndex = 0;

        for (int i = 0; i < _maxRoadCount; i++)
        {
            CreateNextRoad();
        }
    }

    public int GetMaxRoadCount()
    {
        return _maxRoadCount;
    }
}