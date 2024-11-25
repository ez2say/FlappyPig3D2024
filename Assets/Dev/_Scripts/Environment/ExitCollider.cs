using UnityEngine;

public class ExitCollider : MonoBehaviour
{
    public delegate void PlayerExitHandler();
    public event PlayerExitHandler OnPlayerExit;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerExit?.Invoke();
        }
    }
}
