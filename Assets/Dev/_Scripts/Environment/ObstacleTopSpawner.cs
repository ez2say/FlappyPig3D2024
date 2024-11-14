using System.Collections;
using UnityEngine;

public class TopObstacleSpawner : BaseSpawner
{
    protected override IEnumerator SpawnObjects()
    {
        while (true)
        {
            if (_spawnedObjectPositions.Count < _maxObjectCount)
            {
                Vector3 spawnPosition = GetRandomPositionInArea(_spawnArea);
                yield return new WaitForSeconds(_spawnInterval);
                InstantiateObject(spawnPosition);
            }
            else
            {
                yield return null;
            }
        }
    }

    protected override void AdjustObjectPosition(GameObject obj)
    {
        Collider objCollider = obj.GetComponent<Collider>();
        if (objCollider != null)
        {
            Vector3 objPosition = obj.transform.position;
            float objHeight = objCollider.bounds.size.y;

            objPosition.y = 80f - objHeight / 2f;
            obj.transform.position = objPosition;

            obj.transform.rotation = Quaternion.Euler(180f, 0f, 0f);
        }
    }
}