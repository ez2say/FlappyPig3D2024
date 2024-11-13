using UnityEngine;

public class BirdController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _forwardSpeed = 3f; // Скорость движения вперед
    [SerializeField] private float _jumpForce = 5f; // Сила прыжка
    [SerializeField] private float _horizontalSpeed = 2f; // Скорость горизонтального перемещения

    private Rigidbody _rb;
    private int _score = 0; // Переменная для хранения очков

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Движение вперед
        _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y, _forwardSpeed);

        // Управление прыжком
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _jumpForce, _forwardSpeed);
        }

        // Горизонтальное перемещение
        float horizontalInput = Input.GetAxis("Horizontal");
        _rb.velocity = new Vector3(horizontalInput * _horizontalSpeed, _rb.velocity.y, _forwardSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            // Увеличиваем количество очков
            _score += 10;

            // Выводим информацию о подборе монетки и количестве очков
            Debug.Log("Подобрана монетка! Очки: " + _score);

            // Уничтожаем монетку
            Destroy(other.gameObject);
        }
    }
}