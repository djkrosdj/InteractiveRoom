using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] 
    private float _speed = 5f;
    [SerializeField] private float _jumpForce = 100f; // Сила прыжка
    private CharacterController _characterController;
 
    // [SerializeField] private GameObject _player;
    
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Получаем пользовательский ввод.
        var verticalInput = Input.GetAxis("Vertical");
        var horizontalInput = Input.GetAxis("Horizontal");
    
        // Вычисляем вектор направления.
        var inputDirection = new Vector3(horizontalInput, 0, verticalInput);
        // Конвертируем локальное направление персонажа вперед в мировое.
        var direction = transform.TransformDirection(inputDirection);

        // Перемещаем персонажа.
        _characterController.SimpleMove(direction * _speed);

        // Проверяем, была ли нажата клавиша пробела и находится ли персонаж на земле.
        if (Input.GetKeyDown(KeyCode.Space) && _characterController.isGrounded)
        {
            // Применяем силу вверх для прыжка.
            Vector3 jumpVector = Vector3.up * _jumpForce;
            _characterController.Move(jumpVector * Time.deltaTime);
        }
      
    }
}