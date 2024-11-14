using System.Collections;
using UnityEngine;

public class BottomObstacleSpawner : BaseSpawner
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

            objPosition.y = 10f + objHeight / 2f;
            obj.transform.position = objPosition;
        }
    }
}