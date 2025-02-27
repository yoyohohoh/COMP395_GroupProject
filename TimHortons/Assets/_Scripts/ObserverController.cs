using UnityEngine;

public class ObserverController : MonoBehaviour
{
    private CharacterController _controller;
    private InputSystem_Actions _inputs;
    [SerializeField] private Vector2 _move;
    [SerializeField] private float _velocity;
    [SerializeField] private float rotationSpeed; // Speed of smooth rotation

    public bool isWalking;
    public Animator animator;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _inputs = new InputSystem_Actions();

        _inputs.Player.Move.performed += context => _move = context.ReadValue<Vector2>();
        _inputs.Player.Move.canceled += context => _move = Vector2.zero;
    }

    private void OnEnable() => _inputs.Enable();
    private void OnDisable() => _inputs.Disable();

    void Update()
    {
        Vector3 movement = new Vector3(_move.x * _velocity * Time.fixedDeltaTime, 0.0f, _move.y * _velocity * Time.fixedDeltaTime);
        _controller.Move(movement);

        isWalking = movement.sqrMagnitude > 0.0001f;
        animator.SetBool("isWalking", isWalking);

        if (isWalking)
        {
            RotateCharacter();
        }
    }

    private void RotateCharacter()
    {
        Vector3 moveDirection = new Vector3(_move.x, 0, _move.y).normalized * -1;
        if (moveDirection != Vector3.zero)
        {
            // Smoothly rotate towards movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        }
    }
}
