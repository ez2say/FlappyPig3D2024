// using System.Collections;
// using UnityEngine;

// public class ForestSpawner : BaseSpawner
// {
//     [Header("Forest Spawn Settings")]
//     [SerializeField] private float _minYOffset = 5f;
//     [SerializeField] private float _maxYOffset = 10f;
//     [SerializeField] private float _spawnXPosition = 4f; // Позиция по X для спауна
//     [SerializeField] private float _initialSpawnDuration = 10f;
//     [SerializeField] private float _spawnDurationIncrease = 2.5f;
//     [SerializeField] private float _spawnInterval = 0.5f;
//     [SerializeField] private float _distanceBetweenScaffoldings = 2f; // Расстояние между лесами
//     [SerializeField] private float _timeToSpawn = 30f; // Время до начала спауна

//     private RoadGenerator _roadGenerator;
//     private float _currentSpawnDuration;
//     private float _segmentLength = 100f; // Длина одного сегмента
//     private int _spawnedSegmentCount = 0; // Счетчик заспауненных сегментов
//     private float _lastSpawnZPosition = 0f; // Последняя позиция спауна по Z
//     private float _nextSpawnTime; // Время следующего спауна
//     private bool _isSpawning = false; // Флаг для отслеживания процесса спауна

//     protected override void Start()
//     {
//         _roadGenerator = FindObjectOfType<RoadGenerator>();
//         if (_roadGenerator == null)
//         {
//             Debug.LogError("RoadGenerator не найден!");
//             return;
//         }

//         _roadGenerator.OnNewSegmentAdded += HandleNewSegmentAdded;
//         _currentSpawnDuration = _initialSpawnDuration;
//         _nextSpawnTime = Time.time + _timeToSpawn;

//         // Учитываем уже существующие сегменты при старте
//         _spawnedSegmentCount = _roadGenerator.SpawnedSegmentCount;
//         _lastSpawnZPosition = _spawnedSegmentCount * _segmentLength;

//         base.Start();
//     }

//     private void OnDestroy()
//     {
//         if (_roadGenerator != null)
//         {
//             _roadGenerator.OnNewSegmentAdded -= HandleNewSegmentAdded;
//         }
//     }

//     private void Update()
//     {
//         if (Time.time >= _nextSpawnTime && !_isSpawning)
//         {
//             StartCoroutine(SpawnScaffoldings());
//             _currentSpawnDuration += _spawnDurationIncrease; // Увеличиваем продолжительность спауна
//             _nextSpawnTime = Time.time + _timeToSpawn; // Обновляем время следующего спауна
//         }
//     }

//     private void HandleNewSegmentAdded(GameObject newSegment)
//     {
//         _spawnedSegmentCount++;
//         _lastSpawnZPosition = _spawnedSegmentCount * _segmentLength; // Обновляем последнюю позицию спауна по Z
//     }

//     protected override IEnumerator SpawnObjects()
//     {
//         yield return null; // Базовый спаун не используется, вместо этого используется SpawnScaffoldings
//     }

//     private IEnumerator SpawnScaffoldings()
//     {
//         _isSpawning = true;
//         float startTime = Time.time;
//         float currentZPosition = _lastSpawnZPosition; // Начальная позиция спауна по Z

//         while (Time.time - startTime < _currentSpawnDuration)
//         {
//             Vector3 spawnPosition = GetRandomPosition(currentZPosition);
//             if (IsPositionValid(spawnPosition))
//             {
//                 InstantiateObject(spawnPosition);
//                 currentZPosition += _distanceBetweenScaffoldings; // Увеличиваем позицию спауна по Z на расстояние между лесами
//             }
//             else
//             {
//                 // Если позиция невалидна, сдвигаем точку спауна на 1 единицу по оси Z
//                 currentZPosition += 1f;
//             }
//             yield return new WaitForSeconds(_spawnInterval);
//         }

//         _lastSpawnZPosition = currentZPosition; // Обновляем последнюю позицию спауна по Z
//         _isSpawning = false;
//     }

//     private Vector3 GetRandomPosition(float currentZPosition)
//     {
//         float randomY = Random.Range(_minYOffset, _maxYOffset);
//         float spawnZ = currentZPosition; // Используем текущую позицию спауна по Z

//         Vector3 point = new Vector3(_spawnXPosition, randomY, spawnZ);
//         return point;
//     }

//     private bool IsPositionValid(Vector3 position)
//     {
//         // Проверка на столкновение с другими объектами
//         if (Physics.CheckSphere(position, 1f)) // Предполагаем, что 1f - минимальное расстояние между объектами
//         {
//             return false;
//         }

//         return true;
//     }

//     protected override void InstantiateObject(Vector3 spawnPosition)
//     {
//         int randomIndex = GetRandomObjectIndex();

//         GameObject obj = CreateObject(randomIndex, spawnPosition);

//         AddObjectPosition(spawnPosition);

//         obj.transform.SetParent(transform);

//         // Отладочное сообщение с координатами спауна лесов
//         Debug.Log($"Спаун лесов на позиции: {spawnPosition}");
//     }
// }
