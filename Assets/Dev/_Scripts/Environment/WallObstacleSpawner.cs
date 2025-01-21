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

    [SerializeField] private int _diagonalSegmentStart = 5;

    private int _wiresSpawnedInCurrentSegment = 0;

    private RoadGenerator _roadGenerator;
    private int _segmentIndexes = 0;

    protected override void Start()
    {
        base.Start();
        _roadGenerator = FindObjectOfType<RoadGenerator>();
        //Я ЗНАЮ ЧТО ЭТА ШТУКА ПЛОХАЯ, Я ПЕРЕДЕЛАЮ , ЭТО РЕСУРСОЗАТРАТНО И ВООБЩЕ ФУ

        if (_roadGenerator != null)
        {
            _segmentIndexes = _roadGenerator.ManageRoads();
            Debug.Log($"fsafsafs{_segmentIndexes}");
        }
        else
        {
            Debug.LogError("RoadGenerator not found on any GameObject!");
        }

        StartCoroutine(SpawnObjects());
    }

    protected override IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (_wiresSpawnedInCurrentSegment < _maxWiresPerSegment)
            {
                int horizontalWireCount = 0;
                int diagonalWireCount = 0;

                if (_segmentIndexes < _diagonalSegmentStart)
                {
                    horizontalWireCount = _maxWiresPerSegment;
                }
                else
                {
                    horizontalWireCount = Random.Range(0, _maxWiresPerSegment + 1);
                    diagonalWireCount = _maxWiresPerSegment - horizontalWireCount;
                }

                for (int i = 0; i < horizontalWireCount; i++)
                {
                    Vector3 spawnPosition1, spawnPosition2;
                    GetRandomSpawnPoints(out spawnPosition1, out spawnPosition2);

                    if (IsPositionValid(spawnPosition1) && IsPositionValid(spawnPosition2) && Mathf.Approximately(spawnPosition1.y, spawnPosition2.y))
                    {
                        SpawnWire(spawnPosition1, spawnPosition2);
                        _wiresSpawnedInCurrentSegment++;
                    }
                }

                for (int i = 0; i < diagonalWireCount; i++)
                {
                    Vector3 spawnPosition1, spawnPosition2;
                    GetRandomSpawnPoints(out spawnPosition1, out spawnPosition2);

                    if (IsPositionValid(spawnPosition1) && IsPositionValid(spawnPosition2) && IsDiagonalPositionValid(spawnPosition1, spawnPosition2))
                    {
                        SpawnWire(spawnPosition1, spawnPosition2);
                        _wiresSpawnedInCurrentSegment++;
                    }
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
    }

    private void SpawnWire(Vector3 startPosition, Vector3 endPosition)
    {
        GameObject wire = Instantiate(_wirePrefab, startPosition, Quaternion.identity, transform);

        LineRenderer lineRenderer = wire.GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);

        Vector3 direction = endPosition - startPosition;

        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);

        wire.transform.rotation = rotation;

        float wireLength = direction.magnitude;
        wire.transform.localScale = new Vector3(wire.transform.localScale.x * 0.5f, wire.transform.localScale.y * 0.5f, wireLength);

        Vector3 midPoint = (startPosition + endPosition) / 2f;
        wire.transform.position = midPoint;

        AddObjectPosition(startPosition);
        AddObjectPosition(endPosition);
    }

    private bool IsPositionValid(Vector3 position)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, _minDistanceBetweenObjects);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Coin"))
            {
                return false;
            }
        }

        return true;
    }

    private bool IsDiagonalPositionValid(Vector3 position1, Vector3 position2)
    {
        foreach (var spawnedPosition in _spawnedObjectPositions)
        {
            if (DoLinesIntersect(position1, position2, spawnedPosition, spawnedPosition + Vector3.right * _minDistanceBetweenObjects))
            {
                return false;
            }
        }

        return true;
    }

    private bool DoLinesIntersect(Vector3 p1, Vector3 p2, Vector3 q1, Vector3 q2)
    {
        float d1 = Direction(q1, q2, p1);
        float d2 = Direction(q1, q2, p2);
        float d3 = Direction(p1, p2, q1);
        float d4 = Direction(p1, p2, q2);

        if (((d1 > 0 && d2 < 0) || (d1 < 0 && d2 > 0)) && ((d3 > 0 && d4 < 0) || (d3 < 0 && d4 > 0)))
        {
            return true;
        }
        else if (d1 == 0 && OnSegment(q1, q2, p1))
        {
            return true;
        }
        else if (d2 == 0 && OnSegment(q1, q2, p2))
        {
            return true;
        }
        else if (d3 == 0 && OnSegment(p1, p2, q1))
        {
            return true;
        }
        else if (d4 == 0 && OnSegment(p1, p2, q2))
        {
            return true;
        }

        return false;
    }

    private float Direction(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return (p3.x - p1.x) * (p2.y - p1.y) - (p2.x - p1.x) * (p3.y - p1.y);
    }

    private bool OnSegment(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return p3.x <= Mathf.Max(p1.x, p2.x) && p3.x >= Mathf.Min(p1.x, p2.x) &&
               p3.y <= Mathf.Max(p1.y, p2.y) && p3.y >= Mathf.Min(p1.y, p2.y);
    }

    public void MoveToNextSegment()
    {
        _wiresSpawnedInCurrentSegment = 0;
    }
}