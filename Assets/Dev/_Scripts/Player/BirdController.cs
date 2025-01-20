using UnityEngine;
using Cinemachine;

public class BirdController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _forwardSpeed = 3f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private CinemachineVirtualCamera _mainCamera;
    [SerializeField] private CinemachineVirtualCamera _2DCamera;

    private Rigidbody _rb;
    private int _score = 0;
    private bool _is2DView = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (_is2DView)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, _forwardSpeed);
            }

            Vector3 newPosition = transform.position;
            newPosition.x = 8;
            transform.position = newPosition;
        }
        else
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y, _forwardSpeed);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, _forwardSpeed);
            }

            float horizontalInput = Input.GetAxis("Horizontal");
            _rb.velocity = new Vector3(horizontalInput * _forwardSpeed, _rb.velocity.y, _forwardSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            _score += 10;
            Debug.Log("Подобрана монетка! Очки: " + _score);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("SpecialZoneEnter"))
        {
            SwitchCameraView(true);
        }

        if (other.CompareTag("BoxCoin"))
        {
            _score += 100;
            Debug.Log("Подобрана коробка монет! Очки: " + _score);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Wire"))
        {
            Debug.Log("Ударило током");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SpecialZoneExit"))
        {
            SwitchCameraView(false);
        }
    }

    private void SwitchCameraView(bool to2D)
    {
        _is2DView = to2D;
        if (to2D)
        {
            _mainCamera.Priority = 0;
            _2DCamera.Priority = 1;
        }
        else
        {
            _mainCamera.Priority = 1;
            _2DCamera.Priority = 0;
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
        Debug.Log("Умерла свинка, всёёёёёёёёё");
    }
}
