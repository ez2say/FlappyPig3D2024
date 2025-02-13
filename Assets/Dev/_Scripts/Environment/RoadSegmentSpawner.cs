using UnityEngine;
using System;

public class RoadSegmentSpawner 
{
    private GameObject _roadPrefab;
    private float _segmentLength;

    public RoadSegmentSpawner(GameObject roadPrefab, float segmentLength)
    {
        _roadPrefab = roadPrefab;

        _segmentLength = segmentLength;
    }

    public GameObject SpawnSegment(Vector3 position)
    {
        return UnityEngine.Object.Instantiate(_roadPrefab, position, Quaternion.identity);
    }

    public Vector3 GetNextPosition(Vector3 currentPosition)
    {
        return currentPosition + new Vector3(0,0, _segmentLength);
    }
}
