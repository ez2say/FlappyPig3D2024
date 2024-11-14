using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelManager : MonoBehaviour
{
    [Header("Tunnel Settings")]
    [SerializeField] private GameObject[] _tunnelSegments; // Массив сегментов туннеля
    [SerializeField] private float _segmentLength = 10f; // Длина одного сегмента
    [SerializeField] private float _spawnInterval = 2f; // Интервал создания новых сегментов
    [SerializeField] private Transform _player; // Ссылка на игрока (птицу)
    [SerializeField] private float _segmentDestroyDelay = 5f; // Время через которое сегмент будет удален

    private int _currentSegmentIndex = 0;
    private float _timer = 0f;
    private float _destroyTimer = 0f;
    private List<GameObject> _activeSegments = new List<GameObject>(); // Список активных сегментов
    private List<Vector3> _obstaclePositions = new List<Vector3>(); // Список позиций препятствий

    [Header("Coin Spawner")]
    [SerializeField] private CoinSpawner _coinSpawner; // Ссылка на CoinSpawner

    void Update()
    {
        _timer += Time.deltaTime;
        _destroyTimer += Time.deltaTime;

        if (_timer >= _spawnInterval)
        {
            SpawnNewSegment();
            _timer = 0f;
        }

        RecycleSegments();
    }

    private void SpawnNewSegment()
    {
        // Создаем новый сегмент за последним сегментом в списке
        GameObject newSegment = Instantiate(_tunnelSegments[_currentSegmentIndex], transform);
        newSegment.transform.position = _activeSegments.Count > 0 ? _activeSegments[_activeSegments.Count - 1].transform.position + Vector3.forward * _segmentLength : Vector3.zero;

        // Добавляем новый сегмент в список
        _activeSegments.Add(newSegment);

        // Переходим к следующему сегменту
        _currentSegmentIndex = (_currentSegmentIndex + 1) % _tunnelSegments.Length;

        // Обновляем список позиций препятствий
        UpdateObstaclePositions();

        // Передаем список позиций препятствий в CoinSpawner
        _coinSpawner.SetObstaclePositions(_obstaclePositions);
    }

    private void RecycleSegments()
    {
        // Проверяем, находится ли первый сегмент за пределами видимости игрока и прошло ли достаточно времени для его удаления
        if (_activeSegments.Count > 0 && _activeSegments[0].transform.position.z < _player.position.z - _segmentLength && _destroyTimer >= _segmentDestroyDelay)
        {
            // Удаляем первый сегмент
            Destroy(_activeSegments[0]);

            // Удаляем первый сегмент из списка
            _activeSegments.RemoveAt(0);

            // Сбрасываем таймер удаления
            _destroyTimer = 0f;

            // Обновляем список позиций препятствий
            UpdateObstaclePositions();

            // Передаем список позиций препятствий в CoinSpawner
            _coinSpawner.SetObstaclePositions(_obstaclePositions);
        }
    }

    private void UpdateObstaclePositions()
    {
        _obstaclePositions.Clear();
        foreach (GameObject segment in _activeSegments)
        {
            BaseSpawner[] spawners = segment.GetComponentsInChildren<BaseSpawner>();
            foreach (BaseSpawner spawner in spawners)
            {
                _obstaclePositions.AddRange(spawner.GetSpawnedObjectPositions());
            }
        }
    }
}