using UnityEngine;

public class BirdController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _forwardSpeed = 3f;
    [SerializeField] private float _jumpForce = 5f;

    private Rigidbody _rb;
    private IInputController _inputController;
    private CameraManager _cameraManager;
    private ScoreManager _scoreManager;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (Application.isMobilePlatform)
        {
            _inputController = new MobileInputController();
        }
        else
        {
            _inputController = new KeyboardInputController();
        }

        _cameraManager = FindObjectOfType<CameraManager>();
    }

    public void SetScoreManager(ScoreManager scoreManager)
    {
        _scoreManager = scoreManager;
    }

    private void Update()
    {
        if (_cameraManager.Is2DView())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            ConstrainXPosition();
        }
        else
        {
            MoveForward();
            if (_inputController.IsJumpInput())
            {
                Jump();
            }
            float horizontalInput = _inputController.GetHorizontalInput();
            MoveHorizontally(horizontalInput);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.name} with {other.tag}");
        if (other.CompareTag("Coin"))
        {
            _scoreManager.AddScore(10);
            UnityEngine.Object.Destroy(other.gameObject);
        }

        if (other.CompareTag("SpecialZoneEnter"))
        {
            SwitchCameraView(true);
        }

        if (other.CompareTag("BoxCoin"))
        {
            _scoreManager.AddScore(100);
            UnityEngine.Object.Destroy(other.gameObject);
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            HandleObstacleCollision();
        }
    }

    private void MoveForward()
    {
        _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y, _forwardSpeed);
    }

    private void Jump()
    {
        _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, _forwardSpeed);
    }

    private void MoveHorizontally(float input)
    {
        _rb.velocity = new Vector3(input * _forwardSpeed, _rb.velocity.y, _forwardSpeed);
    }

    private void ConstrainXPosition()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = 8;
        transform.position = newPosition;
    }

    public void SwitchCameraView(bool to2D)
    {
        _cameraManager?.SwitchCameraView(to2D);
    }

    public void HandleObstacleCollision()
    {
        Die();
    }

    private void Die()
    {
        Debug.Log("Умерла свинка, всёёёёёёёёё");
    }
}