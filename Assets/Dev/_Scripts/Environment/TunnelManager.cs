using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelManager : MonoBehaviour
{
    [Header("Tunnel Settings")]
    [SerializeField] private GameObject[] _tunnelSegments;

    [SerializeField] private float _segmentLength = 10f;

    [SerializeField] private float _spawnInterval = 2f;

    [SerializeField] private Transform _player;

    [SerializeField] private float _segmentDestroyDelay = 5f;

    private int _currentSegmentIndex = 0;

    private float _timer = 0f;

    private float _destroyTimer = 0f;

    private List<GameObject> _activeSegments = new List<GameObject>();

    private List<Vector3> _obstaclePositions = new List<Vector3>();

    [Header("Coin Spawner")]
    [SerializeField] private CoinSpawner _coinSpawner;

    void Update()
    {
        UpdateTimers();

        if (ShouldSpawnNewSegment())
        {
            SpawnNewSegment();

            ResetTimer();
        }

        RecycleSegments();
    }

    private void UpdateTimers()
    {
        _timer += Time.deltaTime;

        _destroyTimer += Time.deltaTime;
    }

    private bool ShouldSpawnNewSegment()
    {
        return _timer >= _spawnInterval;
    }

    private void ResetTimer()
    {
        _timer = 0f;
    }

    private void SpawnNewSegment()
    {
        GameObject newSegment = CreateNewSegment();

        PositionNewSegment(newSegment);

        _activeSegments.Add(newSegment);

        IncrementSegmentIndex();

        UpdateObstaclePositions();

        _coinSpawner.SetObstaclePositions(_obstaclePositions);
    }

    private GameObject CreateNewSegment()
    {
        return Instantiate(_tunnelSegments[_currentSegmentIndex], transform);
    }

    private void PositionNewSegment(GameObject newSegment)
    {
        newSegment.transform.position = _activeSegments.Count > 0 ? GetLastSegmentPosition() + Vector3.forward * _segmentLength : Vector3.zero;
    }

    private Vector3 GetLastSegmentPosition()
    {
        return _activeSegments[_activeSegments.Count - 1].transform.position;
    }

    private void IncrementSegmentIndex()
    {
        _currentSegmentIndex = (_currentSegmentIndex + 1) % _tunnelSegments.Length;
    }

    private void RecycleSegments()
    {
        if (ShouldRecycleFirstSegment())
        {
            DestroyFirstSegment();

            RemoveFirstSegmentFromList();

            ResetDestroyTimer();

            UpdateObstaclePositions();

            _coinSpawner.SetObstaclePositions(_obstaclePositions);
        }
    }

    private bool ShouldRecycleFirstSegment()
    {
        return _activeSegments.Count > 0 && IsFirstSegmentOutOfView() && _destroyTimer >= _segmentDestroyDelay;
    }

    private bool IsFirstSegmentOutOfView()
    {
        return _activeSegments[0].transform.position.z < _player.position.z - _segmentLength;
    }

    private void DestroyFirstSegment()
    {
        Destroy(_activeSegments[0]);
    }

    private void RemoveFirstSegmentFromList()
    {
        _activeSegments.RemoveAt(0);
    }

    private void ResetDestroyTimer()
    {
        _destroyTimer = 0f;
    }

    private void UpdateObstaclePositions()
    {
        _obstaclePositions.Clear();
        foreach (GameObject segment in _activeSegments)
        {
            CollectObstaclePositionsFromSegment(segment);
        }
    }

    private void CollectObstaclePositionsFromSegment(GameObject segment)
    {
        BaseSpawner[] spawners = segment.GetComponentsInChildren<BaseSpawner>();
        foreach (BaseSpawner spawner in spawners)
        {
            _obstaclePositions.AddRange(spawner.GetSpawnedObjectPositions());
        }
    }
}