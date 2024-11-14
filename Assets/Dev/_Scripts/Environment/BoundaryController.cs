using UnityEngine;

public class BoundaryController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            
            rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
        }
    }
}