using UnityEngine;
using System;

public class BirdController : MonoBehaviour, IDie
{
    [Header("Movement Settings")]
    [SerializeField] private float _forwardSpeed = 3f;
    [SerializeField] private float _jumpForce = 5f;

    private Rigidbody _rb;
    private IInputController _inputController;
    private CameraManager _cameraManager;
    private ScoreManager _scoreManager;

    public event Action OnDie;

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

        if (other.CompareTag("Wire")|| other.CompareTag("Balka")|| other.CompareTag("Building") )
        {
            Die();
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
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Drone") || collision.gameObject.CompareTag("Ground"))
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

    public void SetScoreManager(ScoreManager scoreManager)
    {
        _scoreManager = scoreManager;
    }

    public void RegisterDeathListener(Action listener)
    {
        OnDie += listener;
    }

    private void Die()
    {
        Debug.Log("Смееерть");
        OnDie?.Invoke();
    }
}