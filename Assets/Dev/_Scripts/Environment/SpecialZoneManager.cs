using System;
using UnityEngine;

public class SpecialZoneManager
{   public enum State
    {   
        Normal, Drones, Cranes, Balkas
    }
    private GameObject _droneSegmentPrefab;
    private GameObject _craneSegmentPrefab;
    private GameObject _balkaSegmentPrefab;

    private int _normalSegmentCount = 0;
    private int _droneSegmentCount = 0;
    private int _craneSegmentCount = 0;
    private int _balkaSegmentCount = 0;
    private int _totalSpawnedSegment = 0;

    private State _currentState = State.Normal;

    
    public SpecialZoneManager(GameObject droneSegmentPrefab, GameObject craneSegmentPrefab, GameObject balkaSegmentPrefab)
    {
        _droneSegmentPrefab = droneSegmentPrefab;
        _craneSegmentPrefab = craneSegmentPrefab;
        _balkaSegmentPrefab = balkaSegmentPrefab;
    }

    public GameObject GetSpecialSegment(int totalSpawnedSegments, Vector3 position)
    {
        switch (_currentState)
        {
            case State.Normal:
                _normalSegmentCount++;
                _totalSpawnedSegment++;
                if (_normalSegmentCount >= 6)
                {
                    _normalSegmentCount = 0;
                    _currentState = State.Drones;
                }
                return null;

            case State.Drones:
                if (_droneSegmentCount < 3)
                {
                    _totalSpawnedSegment++;
                    _droneSegmentCount++;
                    return UnityEngine.Object.Instantiate(_droneSegmentPrefab, position, Quaternion.identity);
                }
                else
                {
                    _droneSegmentCount = 0;
                    _currentState = State.Normal;
                    return null;
                }

            case State.Cranes:
                if (_craneSegmentCount < 3)
                {
                    _totalSpawnedSegment++;
                    _craneSegmentCount++;
                    return UnityEngine.Object.Instantiate(_craneSegmentPrefab, position, Quaternion.identity);
                }
                else
                {
                    _craneSegmentCount = 0;
                    _currentState = State.Balkas;
                    return null;
                }

            case State.Balkas:
                if (_balkaSegmentCount < 3)
                {
                    _totalSpawnedSegment++;
                    _balkaSegmentCount++;
                    return UnityEngine.Object.Instantiate(_balkaSegmentPrefab, position, Quaternion.identity);
                }
                else
                {
                    _balkaSegmentCount = 0;
                    _currentState = State.Normal;
                    return null;
                }
        }
        return null;
    }

    public void CheckStateTransition(int totalSpawnedSegment)
    {
        Debug.Log($"количество обычных{_normalSegmentCount} , c дронами {_droneSegmentCount}, с балками {_balkaSegmentCount}, с  кранами{_craneSegmentCount}");

        if (_currentState == State.Normal && _normalSegmentCount >= 6)
        {
            _currentState = State.Drones;
        }

        if (_totalSpawnedSegment % 18 == 0)
        {
            _currentState = State.Cranes;
        }

        if ( _totalSpawnedSegment %  21 == 0)
        {
            _currentState = State.Balkas;
        }

        if (_currentState == State.Balkas && _balkaSegmentCount >= 3)
        {
            _currentState = State.Normal;
        }
    }
}