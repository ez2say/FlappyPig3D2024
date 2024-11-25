// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CraneSpawner : BaseSpawner
// {
//     [Header("Crane Spawner Settings")]
//     [SerializeField] private List<Transform> _spawnPoints = new List<Transform>();
//     [SerializeField] private float _spawnIntervalMin = 5f;
//     [SerializeField] private float _spawnIntervalMax = 10f;
//     [SerializeField] private float _spawnHeightMin = 10f;
//     [SerializeField] private float _spawnHeightMax = 20f;

//     [SerializeField] protected float _minDistanceBetweenObjects = 2f;
    
//     protected override IEnumerator SpawnObjects()
//     {
//         while (true)
//         {
//             if (_spawnedObjectPositions.Count < _maxObjectCount)
//             {
//                 Vector3 spawnPosition = GetRandomSpawnPoint();

//                 if (IsPositionValid(spawnPosition))
//                 {
//                     InstantiateObject(spawnPosition);
//                 }
//             }

//             yield return new WaitForSeconds(Random.Range(_spawnIntervalMin, _spawnIntervalMax));
//         }
//     }

//     private Vector3 GetRandomSpawnPoint()
//     {
//         if (_spawnPoints.Count == 0)
//         {
//             Debug.LogError("No spawn points defined for CraneSpawner.");
//             return Vector3.zero;
//         }

//         int randomIndex = Random.Range(0, _spawnPoints.Count);
//         Vector3 basePosition = _spawnPoints[randomIndex].position;

//         // Добавляем случайную высоту
//         float randomHeight = Random.Range(_spawnHeightMin, _spawnHeightMax);
//         Vector3 spawnPosition = new Vector3(basePosition.x, randomHeight, basePosition.z);

//         return spawnPosition;
//     }

//     protected override void InstantiateObject(Vector3 spawnPosition)
//     {
//         int randomIndex = GetRandomObjectIndex();

//         GameObject obj = CreateObject(randomIndex, spawnPosition);

//         AddObjectPosition(spawnPosition);

//         obj.transform.SetParent(transform);

//         // Поворот по оси X на -90 градусов
//         obj.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
//     }

//     private bool IsPositionValid(Vector3 position)
//     {
//         // Проверка на расстояние между объектами
//         foreach (Vector3 spawnedPosition in _spawnedObjectPositions)
//         {
//             if (Vector3.Distance(position, spawnedPosition) < _minDistanceBetweenObjects)
//             {
//                 return false;
//             }
//         }

//         // Проверка на столкновение с другими объектами
//         if (Physics.CheckSphere(position, _minDistanceBetweenObjects))
//         {
//             return false;
//         }

//         return true;
//     }
// }