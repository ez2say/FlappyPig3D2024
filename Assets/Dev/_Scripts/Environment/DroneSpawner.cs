using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSpawner : BaseSpawner
{
    [Header("Drone Spawner Settings")]
    [SerializeField] private Transform[] _spawnPoints;

    [SerializeField] private float _minDroneSpeed = 2f;

    [SerializeField] private float _maxDroneSpeed = 10f;

    [SerializeField] private GameObject _boxPrefab;

    private bool _isSpawning = false;

    private List<int> _spawnPointIndices;

    private int _lastSpawnIndex = -1;

    protected override void Start()
    {
        InitializeSpawnPointIndices();
        base.Start();
    }

    private void InitializeSpawnPointIndices()
    {
        _spawnPointIndices = new List<int>();
        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            _spawnPointIndices.Add(i);
        }
    }

    protected override IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (_isSpawning && _spawnedObjectPositions.Count < _maxObjectCount)
            {
                Transform spawnPoint = GetRandomSpawnPoint();

                if (spawnPoint != null)
                {
                    GameObject drone = InstantiateObject(spawnPoint.position);
                    if (drone != null)
                    {
                        Rigidbody rb = drone.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            int boxCount = Random.Range(1, 4);

                            float droneSpeed = GetDroneSpeed(boxCount);

                            rb.velocity = new Vector3(0, 0, -droneSpeed);

                            AttachBoxesToDrone(drone, rb, boxCount);
                        }
                    }
                }
            }

            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private void AttachBoxesToDrone(GameObject drone, Rigidbody rb, int boxCount)
    {
        GameObject previousBox = null;
        for (int i = 0; i < boxCount; i++)
        {
            GameObject box = InstantiateBox(drone, previousBox, rb, i);

            previousBox = box;
        }
    }

    private GameObject InstantiateBox(GameObject drone, GameObject previousBox, Rigidbody rb, int index)
    {
        GameObject box = Instantiate(_boxPrefab, drone.transform.position - new Vector3(0, 1 + index, 0), Quaternion.identity);
        
        FixedJoint joint = box.AddComponent<FixedJoint>();
        
        ConfigureJoint(joint, previousBox, rb);
        
        box.transform.SetParent(drone.transform);
       
        return box;
    }

    private void ConfigureJoint(FixedJoint joint, GameObject previousBox, Rigidbody rb)
    {
        if (previousBox != null)
        {
            joint.connectedBody = previousBox.GetComponent<Rigidbody>();
            
            joint.anchor = new Vector3(0, -2, 0);
        }
        else
        {
            joint.connectedBody = rb;
            
            joint.anchor = new Vector3(0, 1, 0);
        }
    }

    private float GetDroneSpeed(int boxCount)
    {
        switch (boxCount)
        {
            case 1:
                return _minDroneSpeed;
            case 2:
                return (_minDroneSpeed + _maxDroneSpeed) / 2;
            case 3:
                return _maxDroneSpeed;
            default:
                return _minDroneSpeed;
        }
    }

    private Transform GetRandomSpawnPoint()
    {
        if (_spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned!");
            return null;
        }

        int nextIndex;
        do
        {
            nextIndex = Random.Range(0, _spawnPoints.Length);
        } while (nextIndex == _lastSpawnIndex);

        _lastSpawnIndex = nextIndex;
        return _spawnPoints[nextIndex];
    }

    public void StartSpawning()
    {
        _isSpawning = true;
    }

    public void StopSpawning()
    {
        _isSpawning = false;
    }
}
