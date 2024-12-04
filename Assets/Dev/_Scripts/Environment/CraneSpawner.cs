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
            Random.Range(-extents.y, extents.y),
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
            AnimationClip randomClip = _animationClips[Random.Range(0, _animationClips.Count)];
            Animation animation = obj.GetComponent<Animation>();

            if (animation != null)
            {
                animation.clip = randomClip;
                animation.Play();
            }
            else
            {
                Debug.LogWarning("No Animation component found on the spawned object.");
            }
        }
        else
        {
            Debug.LogWarning("No animation clips assigned to the CraneSpawner.");
        }
    }
}