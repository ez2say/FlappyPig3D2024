using UnityEngine;
using System.Collections.Generic;

public class RoadCleaner
{
    public List<GameObject> Roads => _roads;
    private List<GameObject> _roads;
    private float _segmentLength;

    public RoadCleaner(List<GameObject> roads, float segmentLength)
    {
        _roads = roads;
        _segmentLength = segmentLength;
    }

    public void CleanOldSegments(Camera camera)
    {
        if (_roads.Count > 0 && 
            _roads[0].transform.position.z < camera.transform.position.z - _segmentLength)
        {
            GameObject oldestRoad = _roads[0];
            _roads.RemoveAt(0);
            UnityEngine.Object.Destroy(oldestRoad);
        }
    }
}