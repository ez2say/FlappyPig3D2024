using System.Collections.Generic;
using UnityEngine;

public class LevelInitializer
{   
    public List<GameObject> Roads => _roads;
    private RoadSegmentSpawner _spawner;
    private SpecialZoneManager _zoneManager;
    private BuildingPainter _painter;
    private List<GameObject> _roads;
    private int _maxRoadCount;

    public LevelInitializer(RoadSegmentSpawner spawner, SpecialZoneManager zoneManager, BuildingPainter painter, int maxRoadCount, List<GameObject> roads)
    {
        _spawner = spawner;
        _zoneManager = zoneManager;
        _painter = painter;
        _maxRoadCount = maxRoadCount;
        _roads = roads;
    }

    public void InitializeLevel()
    {
        for (int i = 0; i < _maxRoadCount; i++)
        {
            CreateNextRoad();
        }
    }

    public void CreateNextRoad()
    {
        Vector3 pos = _roads.Count > 0 ? _spawner.GetNextPosition(_roads[_roads.Count - 1].transform.position) : Vector3.zero;
        GameObject generate = _spawner.SpawnSegment(pos);

        if (_zoneManager != null)
        {
            GameObject specialSegment = _zoneManager.GetSpecialSegment(_roads.Count, pos);
            if (specialSegment != null)
            {
                UnityEngine.Object.Destroy(generate);
                generate = specialSegment;
            }
        }

        if (_painter != null)
        {
            _painter.PaintBuildings(generate);
        }

        _roads.Add(generate);

        _zoneManager.CheckStateTransition(_roads.Count);
    }
}