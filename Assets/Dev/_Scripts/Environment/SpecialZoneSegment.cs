using UnityEngine;

public class SpecialZoneSegment : MonoBehaviour
{
    public BoxCollider enterCollider;
    public BoxCollider exitCollider;
    public DroneSpawner droneSpawner;

    private void OnValidate()
    {
        if (enterCollider != null)
        {
            enterCollider.isTrigger = true;
        }

        if (exitCollider != null)
        {
            exitCollider.isTrigger = true;
        }
    }

    private void Start()
    {
        if (enterCollider != null)
        {
            EnterCollider enterColliderComponent = enterCollider.GetComponent<EnterCollider>();
            if (enterColliderComponent != null)
            {
                enterColliderComponent.OnPlayerEnter += droneSpawner.StartSpawning;
            }
        }

        if (exitCollider != null)
        {
            ExitCollider exitColliderComponent = exitCollider.GetComponent<ExitCollider>();
            if (exitColliderComponent != null)
            {
                exitColliderComponent.OnPlayerExit += droneSpawner.StopSpawning;
            }
        }
    }
}
