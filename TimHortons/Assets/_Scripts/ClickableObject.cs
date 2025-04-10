using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickableObject : MonoBehaviour
{
    private InputSystem_Actions _inputs;
    private InputAction clickAction; // Assign this via Inspector
    public string objectName;

    private GameObject lastHoveredObject;
    private Material originalMaterial;
    public Material highlightMaterial;
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

    private void Update()
    {
        HighlightOnHover();
    }

    private void HighlightOnHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject hitObj = hit.transform.gameObject;

            // If hovering over an AI object
            if (hitObj.CompareTag("AI"))
            {
                OrderController orderController = hitObj.GetComponent<OrderController>();
                MeshRenderer headRenderer = orderController.head;
                SkinnedMeshRenderer bodyRenderer = orderController.body;

                // Only change material if we are hovering over a new object
                if (lastHoveredObject != hitObj)
                {
                    // Clear highlight from the last hovered object if it's different
                    if (lastHoveredObject != null)
                    {
                        OrderController lastOrderController = lastHoveredObject.GetComponent<OrderController>();
                        MeshRenderer lastHeadRenderer = lastOrderController.head;
                        SkinnedMeshRenderer lastBodyRenderer = lastOrderController.body;

                        ClearHighlight(lastHeadRenderer, lastBodyRenderer);
                    }

                    // Highlight the new object
                    if (headRenderer != null && bodyRenderer != null)
                    {
                        originalMaterial = headRenderer.material;
                        headRenderer.material = highlightMaterial;
                        bodyRenderer.material = highlightMaterial;
                        lastHoveredObject = hitObj;
                    }
                }
            }
            else
            {
                if (lastHoveredObject != null)
                {
                    OrderController lastOrderController = lastHoveredObject.GetComponent<OrderController>();
                    MeshRenderer lastHeadRenderer = lastOrderController.head;
                    SkinnedMeshRenderer lastBodyRenderer = lastOrderController.body;

                    ClearHighlight(lastHeadRenderer, lastBodyRenderer);
                }
            }
        }
        else
        {
            // If the raycast didn't hit anything, clear the highlight
            if (lastHoveredObject != null)
            {
                OrderController lastOrderController = lastHoveredObject.GetComponent<OrderController>();
                MeshRenderer lastHeadRenderer = lastOrderController.head;
                SkinnedMeshRenderer lastBodyRenderer = lastOrderController.body;

                ClearHighlight(lastHeadRenderer, lastBodyRenderer);
            }
        }
    }

    private void ClearHighlight(MeshRenderer headRenderer, SkinnedMeshRenderer bodyRenderer)
    {
        if (headRenderer != null && bodyRenderer != null)
        {
            headRenderer.material = originalMaterial;
            bodyRenderer.material = originalMaterial;
        }
        lastHoveredObject = null;
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            //Debug.Log("Clicked on: " + hit.transform.name);
            Transform hitObject = hit.transform;
            objectName = hitObject.name;

            // tutorial
            GameObject coffeeMaker = GameObject.Find("CoffeeMaker");
            if (coffeeMaker != null)
            {
                coffeeMaker.GetComponent<CoffeeMakerController>().AddItems(objectName);
                GameObject.Find(objectName).transform.Find("Juice")?.gameObject.SetActive(false);
            }

            // level
            if (hitObject.CompareTag("AI"))
            {
                OrderController orderController = hitObject.GetComponent<OrderController>();
                if (!orderController.isOrderReceived)
                { orderController.isOrderReceived = GameObject.Find("OrderManager").GetComponent<OrderManager>().CheckCompletedCoffee(orderController.order); }
            }
            else if ((hitObject.CompareTag("Ingredient")))
            {
                GameObject orderManager = GameObject.Find("OrderManager");
                if (orderManager != null)
                {
                    orderManager.GetComponent<OrderManager>().OnIngredientAdded(objectName);
                }
            }
            else if ((hitObject.CompareTag("Clear")))
            {
                GameObject orderManager = GameObject.Find("OrderManager");
                if (orderManager != null)
                {
                    orderManager.GetComponent<OrderManager>().ClearIngredients();
                }
            }





        }
    }
}
