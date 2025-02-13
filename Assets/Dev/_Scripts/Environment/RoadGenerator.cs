using UnityEngine;
using System.Collections.Generic;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _roadPrefab;
    [SerializeField] private GameObject _specialRoadPrefab;
    [SerializeField] private GameObject _balkaRoadPrefab;
    [SerializeField] private GameObject _craneRoadPrefab;
    [SerializeField] private List<Material> _buildingMaterials;
    [SerializeField] private int _maxRoadCount = 5;
    [SerializeField] private float _segmentLength = 100f;

    private RoadSegmentSpawner _spawner;
    private SpecialZoneManager _zoneManager;
    private BuildingPainter _painter;
    private RoadCleaner _cleaner;
    private LevelInitializer _initializer;

    private bool _isGenerating = false;

    private void InitializeRoadSettings()
    {
        _spawner = new RoadSegmentSpawner(_roadPrefab, _segmentLength);
        _zoneManager = new SpecialZoneManager(_specialRoadPrefab, _craneRoadPrefab, _balkaRoadPrefab);
        _painter = new BuildingPainter(_buildingMaterials);
        List<GameObject> roads = new List<GameObject>();
        _cleaner = new RoadCleaner(roads, _segmentLength);
        _initializer = new LevelInitializer(_spawner,_zoneManager, _painter, _maxRoadCount, roads);

    }

    public void StartRoadGeneration()
    {
        if(!_isGenerating)
        {
            InitializeRoadSettings();

            _initializer.InitializeLevel();

            _isGenerating = true; 
        }
        
    }

    private void Update()
    {
        if(_isGenerating)
        {
            _cleaner.CleanOldSegments(Camera.main);
            if (_initializer.Roads.Count > 0 && 
                _initializer.Roads[_initializer.Roads.Count - 1].transform.position.z < 
                Camera.main.transform.position.z + _segmentLength * (_maxRoadCount - 1))
            {
                _initializer.CreateNextRoad();
            }
        }
    }

    public int ManageRoads()
    {
        return _initializer.Roads.Count;
    }

    public void AddRoad(GameObject road)
    {
        if (_cleaner != null)
        {
            _cleaner.Roads.Add(road);
        }
    }

    public List<GameObject> GetRoads()
    {
        if (_cleaner != null)
        {
            return _cleaner.Roads;
        }
        return new List<GameObject>();
    }
}