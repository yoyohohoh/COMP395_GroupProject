using UnityEngine;
using UnityEngine.InputSystem;

public class ClickableObject : MonoBehaviour
{
    private InputSystem_Actions _inputs;
    private InputAction clickAction; // Assign this via Inspector
    public string objectName;

    private void Awake()
    {
        _inputs = new InputSystem_Actions();
        clickAction = _inputs.Player.Click;
    }
    private void OnEnable()
    {
        _inputs.Enable();
        clickAction.performed += OnClick;
    }

    private void OnDisable()
    {
        clickAction.performed -= OnClick;
        _inputs.Disable();
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("Clicked on: " + hit.transform.name);
            objectName = hit.transform.name;
            GameObject.Find(objectName).transform.Find("Juice")?.gameObject.SetActive(false);
            GameObject.Find("CoffeeMaker").GetComponent<CoffeeMakerController>().AddItems(objectName);
        }
    }
}
