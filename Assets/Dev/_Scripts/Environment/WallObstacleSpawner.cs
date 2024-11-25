using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObstacleSpawner : BaseSpawner
{
    [Header("Wall Spawner Settings")]
    [SerializeField] private List<Transform> _spawnPointsHouse1 = new List<Transform>();

    [SerializeField] private List<Transform> _spawnPointsHouse2 = new List<Transform>();

    [SerializeField] private int _maxWiresPerSegment = 3;

    [SerializeField] private GameObject _wirePrefab;

    [SerializeField] protected float _minDistanceBetweenObjects = 2f;

    private int _currentSegmentIndex = 0;
    
    private int _wiresSpawnedInCurrentSegment = 0;

    protected override IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (_wiresSpawnedInCurrentSegment < _maxWiresPerSegment)
            {
                Vector3 spawnPosition1, spawnPosition2;

                GetRandomSpawnPoints(out spawnPosition1, out spawnPosition2);

                if (IsPositionValid(spawnPosition1) && IsPositionValid(spawnPosition2))
                {
                    SpawnWire(spawnPosition1, spawnPosition2);

                    _wiresSpawnedInCurrentSegment++;
                }
            }

            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private void GetRandomSpawnPoints(out Vector3 spawnPosition1, out Vector3 spawnPosition2)
    {
        if (_spawnPointsHouse1.Count == 0 || _spawnPointsHouse2.Count == 0)
        {
            Debug.LogError("Не найдены точки");

            spawnPosition1 = Vector3.zero;

            spawnPosition2 = Vector3.zero;
            return;
        }

        int randomIndex1 = Random.Range(0, _spawnPointsHouse1.Count);

        int randomIndex2 = Random.Range(0, _spawnPointsHouse2.Count);

        spawnPosition1 = _spawnPointsHouse1[randomIndex1].position;

        spawnPosition2 = _spawnPointsHouse2[randomIndex2].position;

        Debug.Log($"Выбрана точка спауна : {spawnPosition1} и {spawnPosition2}");
    }

    private void SpawnWire(Vector3 startPosition, Vector3 endPosition)
    {

        GameObject wire = Instantiate(_wirePrefab, startPosition, Quaternion.identity);

        LineRenderer lineRenderer = wire.GetComponent<LineRenderer>();

        if (lineRenderer == null)
        {
            Debug.LogError("Нет Line Renderere");
            return;
        }

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPosition);

        lineRenderer.SetPosition(1, endPosition);

        AddObjectPosition(startPosition);

        AddObjectPosition(endPosition);

        Debug.Log($"Провод между {startPosition} и {endPosition}");
    }

    private bool IsPositionValid(Vector3 position)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, _minDistanceBetweenObjects);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Coin") || hitCollider.CompareTag("Drone"))
            {
                return false;
            }
        }

        return true;
    }

    public void MoveToNextSegment()
    {
        _currentSegmentIndex++;

        _wiresSpawnedInCurrentSegment = 0;
    }
}