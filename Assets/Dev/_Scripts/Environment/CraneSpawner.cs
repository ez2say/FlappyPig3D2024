using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneSpawner : BaseSpawner
{
    [Header("Crane Spawner Settings")]
    [SerializeField] private BoxCollider _spawnArea;
    [SerializeField] private List<AnimationClip> _animationClips;

    protected override IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (_spawnedObjectPositions.Count < _maxObjectCount)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();

                GameObject obj = InstantiateObject(spawnPosition);

                PlayRandomAnimation(obj);
            }

            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector3 extents = _spawnArea.bounds.extents;
        Vector3 randomPosition = new Vector3(
            Random.Range(-extents.x, extents.x),
            -30,
            Random.Range(-extents.z, extents.z)
        );

        randomPosition += _spawnArea.transform.position;
        randomPosition.y -= extents.y;

        return randomPosition;
    }

    private void PlayRandomAnimation(GameObject obj)
    {
        if (_animationClips.Count > 0)
        {
            Animator animator = obj.GetComponent<Animator>();

            AnimationClip randomClip = _animationClips[Random.Range(0, _animationClips.Count)];
            Debug.Log($"Выбрана анимация {randomClip}");

            animator.Play(randomClip.name);
            
        }
    }
}