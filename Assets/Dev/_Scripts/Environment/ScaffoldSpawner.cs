using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaffoldSpawner : BaseSpawner
{
    [Header("Scaffold Spawner Settings")]
    [SerializeField] private Transform[] _spawnPoints; // Точки спавна
    [SerializeField] private float _delayBetweenPoints = 0.5f; // Задержка между точками спавна
    [SerializeField] private float _objectLifetime = 10f; // Время жизни объекта

    private ObjectPool _objectPool; // Пул объектов
    private bool _isSpawning = false;

    // Список активных объектов
    private List<GameObject> _activeObjects = new List<GameObject>();

    protected override void Start()
    {
        base.Start();

        // Инициализация пула объектов
        if (_prefabs.Length > 0)
        {
            _objectPool = new ObjectPool(_prefabs[0], _maxObjectCount, transform);
        }
        else
        {
            Debug.LogError("No prefabs assigned in the BaseSpawner!");
        }

        // Проверка наличия точек спавна
        if (_spawnPoints == null || _spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned!");
        }
    }

    protected override IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (!_isSpawning && _objectPool != null) // Проверяем, что пул объектов существует
            {
                StartCoroutine(SpawnRandomPoints());
            }
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private IEnumerator SpawnRandomPoints()
    {
        _isSpawning = true;

        // Ограничиваем количество активных объектов
        int maxSpawnedObjects = Mathf.Min(_maxObjectCount - _activeObjects.Count, _spawnPoints.Length);

        if (maxSpawnedObjects <= 0)
        {
            Debug.LogWarning("Max object count reached for this spawner!");
            _isSpawning = false;
            yield break;
        }

        List<int> usedIndexes = new List<int>(); // Список использованных индексов для предотвращения повторов

        for (int i = 0; i < maxSpawnedObjects; i++)
        {
            int randomIndex = GetRandomUnusedIndex(usedIndexes);

            if (randomIndex != -1 && _spawnPoints[randomIndex] != null && _objectPool != null)
            {
                Transform spawnPoint = _spawnPoints[randomIndex];
                GameObject scaffold = _objectPool.GetObject();

                if (scaffold != null) // Проверяем, что объект успешно получен из пула
                {
                    scaffold.transform.position = spawnPoint.position;
                    scaffold.transform.rotation = Quaternion.identity;

                    // Добавляем объект в список активных
                    _activeObjects.Add(scaffold);

                    StartCoroutine(DespawnObject(scaffold, _objectLifetime));
                }
                else
                {
                    Debug.LogWarning("Failed to get object from pool!");
                }

                usedIndexes.Add(randomIndex); // Отмечаем индекс как использованный
            }

            yield return new WaitForSeconds(_delayBetweenPoints);
        }

        _isSpawning = false;
    }

    private int GetRandomUnusedIndex(List<int> usedIndexes)
    {
        List<int> availableIndexes = new List<int>();

        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            if (!usedIndexes.Contains(i))
            {
                availableIndexes.Add(i);
            }
        }

        if (availableIndexes.Count > 0)
        {
            int randomIndex = Random.Range(0, availableIndexes.Count);
            return availableIndexes[randomIndex];
        }

        return -1; // Если доступных индексов нет, возвращаем -1
    }

    private IEnumerator DespawnObject(GameObject obj, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);

        if (_objectPool != null) // Проверяем, что пул объектов существует
        {
            _objectPool.ReturnObject(obj);

            // Удаляем объект из списка активных
            _activeObjects.Remove(obj);
        }
        else
        {
            Debug.LogWarning("Object pool is null, cannot return object!");
        }
    }
}