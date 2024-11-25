using UnityEngine;

public class EnterCollider : MonoBehaviour
{
    public delegate void PlayerEnterHandler();
    public event PlayerEnterHandler OnPlayerEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerEnter?.Invoke();
        }
    }
}
