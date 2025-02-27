using UnityEngine;

public class ObserverController : MonoBehaviour
{
    private CharacterController _controller;
    private InputSystem_Actions _inputs;
    [SerializeField] private Vector2 _move;
    [SerializeField] private float _velocity;

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(_move.x * _velocity * Time.fixedDeltaTime, 0.0f, _move.y * _velocity * Time.fixedDeltaTime);
        _controller.Move(movement);
        isWalking = movement.sqrMagnitude > 0.0001f;
        animator.SetBool("isWalking", isWalking);
    }
}
