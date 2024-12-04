using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public int SpawnedSegmentCount { get; private set; } = 0;

    [SerializeField] private GameObject _roadPrefab;

    [SerializeField] private GameObject _specialRoadPrefab;

    [SerializeField] private List<Material> _buildingMaterials;

    private List<GameObject> _roads = new List<GameObject>();

    [SerializeField] private int _maxRoadCount = 5;

    [SerializeField] private float _segmentLength = 100f;

    [SerializeField] private int _specialZoneLength = 3;

    private int _normalSegmentCount = 0;

    private bool _specialZoneSpawned = false;

    private int _specialZoneSegmentIndex = 0;

    private int _zoneSegmentIndex = 0;

    private List<int> _segmentIndexes = new List<int>();

    private bool isGameStarted = false;

    private void Update()
    {
        if (!isGameStarted) return;

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

        _segmentIndexes.RemoveAt(0);

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

        SpawnedSegmentCount++;

        _segmentIndexes.Add(SpawnedSegmentCount);

        Debug.Log($" Сегментов - {SpawnedSegmentCount}");
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

            _normalSegmentCount++;
            _zoneSegmentIndex++;

            Debug.Log($" Сегментов - {SpawnedSegmentCount}");
            Debug.Log($"ManageRoads{_zoneSegmentIndex}");

            if (_normalSegmentCount >= 6)
            {
                _normalSegmentCount = 0;
                _specialZoneSpawned = true;
            }

            PaintBuildings(generate);
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

    private void PaintBuildings(GameObject roadSegment)
    {
        List<Transform> buildings = new List<Transform>();
        FindBuildings(roadSegment.transform, buildings);

        if (buildings.Count == 0)
        {
            Debug.LogWarning($"В сегменте {roadSegment.name} не найдено объектов с тегом 'Building'.");
            return;
        }

        Dictionary<Transform, Material> houseMaterials = new Dictionary<Transform, Material>();

        foreach (Transform building in buildings)
        {
            Renderer renderer = building.GetComponent<Renderer>();
            if (renderer != null && _buildingMaterials.Count > 0)
            {
                if (houseMaterials.ContainsKey(building.parent))
                {
                    renderer.material = houseMaterials[building.parent];
                }
                else
                {
                    Material randomMaterial = _buildingMaterials[Random.Range(0, _buildingMaterials.Count)];

                    renderer.material = randomMaterial;

                    houseMaterials[building.parent] = randomMaterial;

                    Debug.Log($"Дом {building.parent.name} покрашен в цвет: {randomMaterial.name}");
                }
            }
        }
    }

    private void FindBuildings(Transform parent, List<Transform> buildings)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag("Building"))
            {
                buildings.Add(child);
            }
            FindBuildings(child, buildings);
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

            _segmentIndexes.RemoveAt(0);
        }
    }

    private void InitializeLevel()
    {
        SpawnedSegmentCount = 0;

        _normalSegmentCount = 0;

        _specialZoneSpawned = false;

        _specialZoneSegmentIndex = 0;

        _zoneSegmentIndex = 0;

        for (int i = 0; i < _maxRoadCount; i++)
        {
            CreateNextRoad();
        }
    }

    public int GetMaxRoadCount()
    {
        return _maxRoadCount;
    }

    public int ManageRoads()
    {
        return _zoneSegmentIndex;
    }

    public void StartRoadGeneration()
    {
        isGameStarted = true;
        
        ResetLevel();
    }
}