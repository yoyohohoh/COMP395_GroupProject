using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class OrderController : MonoBehaviour
{
    [Header("Movement")]
    public Transform[] waypoints;
    public int currentIndex = 0;
    public float moveSpeed = 1f;
    public Animator animator;

    [Header("Detection")]
    public float detectionRadius = 5f; // Distance at which AIs detect each other
    public float alignmentDistance = 1f; // Distance at which they align
    private Rigidbody rb;

    [Header("Order Information")]
    public string order;
    public bool isOrderPlaced;
    public bool isOrderReceived;
    GameObject orderTag;
    public GameObject latte;
    public GameObject black;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isOrderPlaced = false;
        isOrderReceived = false;
        waypoints = GameObject.Find("Waypoints").GetComponent<OrderWaypoints>()._waypoints;
        StartCoroutine(OrderMovement());
        if (isOrderReceived)
        {
            if (animator != null)
            {
                animator.SetBool("isIdle", false);
            }
        }
        rb = GetComponent<Rigidbody>();

        orderTag = this.GetComponentsInChildren<Transform>()[1].gameObject;
        order = GetRandomOrder();
        orderTag.SetActive(true);
        if (order == "Caffee Latte")
        {
            latte.SetActive(true);
            black.SetActive(false);
        }
        else if (order == "Long Black")
        {
            latte.SetActive(false);
            black.SetActive(true);
        }
        else
        {
            Debug.Log("Order Error");
        }
        orderTag.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //MoveAI();

        if (isOrderReceived)
        {
            animator.SetBool("isIdle", false);
        }

        orderTag.SetActive(isOrderPlaced);

        if (currentIndex == 2)
        {
            if (!isOrderPlaced)
            {
                DataKeeper.Instance.listOfOrders.Add(order);
            }
            isOrderPlaced = true;
        }

        if (currentIndex >= waypoints.Length)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator OrderMovement()
    {
        while (currentIndex < waypoints.Length)
        {
            Transform targetWaypoint = waypoints[currentIndex];
            if (currentIndex == 2)
            {
                this.transform.rotation = Quaternion.Euler(0, 90, 0);
            }

            if (currentIndex == 3 && !isOrderReceived)
            {
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                Debug.Log("Waiting to receive Order");
                if (animator != null)
                {
                    animator.SetBool("isIdle", true);
                }
                yield return new WaitUntil(() => isOrderReceived);
            }

            // Move towards the target waypoint
            while (Vector3.Distance(transform.position, targetWaypoint.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);
                yield return null;
            }

            currentIndex++;
            yield return new WaitForSeconds(0.5f);
        }
    }

    private string GetRandomOrder()
    {
        System.Random random = new System.Random();
        int randomNumber = random.Next(0, 2);

        return randomNumber == 0 ? "Caffee Latte" : "Long Black";
    }

    private void MoveAI()
    {
        // Example movement (you can replace it with actual movement logic)
        Vector3 moveDirection = transform.forward;
        rb.linearVelocity = moveDirection * moveSpeed;

        // Check for nearby AIs within the detection radius
        Collider[] nearbyAIs = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (var ai in nearbyAIs)
        {
            if (ai.CompareTag("AI") && ai.gameObject != gameObject)
            {
                float distanceToAI = Vector3.Distance(transform.position, ai.transform.position);

                if (distanceToAI < alignmentDistance)
                {
                    // Align the AI (move apart)
                    Vector3 directionAwayFromAI = transform.position - ai.transform.position;
                    Vector3 alignedPosition = transform.position + directionAwayFromAI.normalized * alignmentDistance;

                    // Move the AI to the new aligned position
                    rb.MovePosition(alignedPosition);
                }
            }
        }
    }

}
