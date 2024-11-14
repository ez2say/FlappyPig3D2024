using UnityEngine;

public class BirdController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _forwardSpeed = 3f;

    [SerializeField] private float _jumpForce = 5f;

    [SerializeField] private float _horizontalSpeed = 2f;

    private Rigidbody _rb;

    private int _score = 0;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y, _forwardSpeed);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, _forwardSpeed);
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        
        _rb.velocity = new Vector3(horizontalInput * _horizontalSpeed, _rb.velocity.y, _forwardSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            _score += 10;

            Debug.Log("Подобрана монетка! Очки: " + _score);

            Destroy(other.gameObject);
        }
    }
    
    private void OnCollisionEnter(Collision other) 
    {
        if (IsObstacleCollision(other))
        {
            HandleObstacleCollision();
        }
    }

    private bool IsObstacleCollision(Collision other)
    {
        return other.gameObject.CompareTag("Obstacle");
    }

    private void HandleObstacleCollision()
    {
        Die();
    }

    private void Die()
    {
        Debug.Log("Умерла свинка,всёёёёёёёёёё");
    }
}