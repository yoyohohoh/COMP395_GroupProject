using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class OrderController : MonoBehaviour
{
    [Header("Movement")]
    public Transform[] waypoints;
    public int currentIndex = 0;
    public float moveSpeed = 4f;
    public Animator animator;

    [Header("Detection")]
    public float detectionRadius = 5f; // Distance at which AIs detect each other
    public float alignmentDistance = 2f; // Distance at which they align
    private Rigidbody rb;

    [Header("Order Information")]
    public string order;
    public bool isOrderPlaced;
    public bool isOrderReceived;
    GameObject orderTag;
    public GameObject latte;
    public GameObject black;

    private void Awake()
    {
        OrderWaypoints orderWaypoints = GameObject.Find("Waypoints").GetComponent<OrderWaypoints>();

        waypoints = new Transform[orderWaypoints._waypoints.Length];
        orderWaypoints._waypoints.CopyTo(waypoints, 0);

        int randomInt = Random.Range(0, orderWaypoints._tables.Length);
        waypoints[3] = orderWaypoints._tables[randomInt];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isOrderPlaced = false;
        isOrderReceived = false;

        StartCoroutine(OrderMovement());

        rb = GetComponent<Rigidbody>();

        orderTag = this.GetComponentsInChildren<Transform>()[1].gameObject;
        order = GetRandomOrder();
        orderTag.SetActive(true);
        if (order == "Caffe Latte")
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
        if (currentIndex >= waypoints.Length)
        {
            Destroy(gameObject);
        }
        else
        {
            MoveAI();

            if (isOrderReceived)
            {
                animator.SetBool("isIdle", false);
            }

            orderTag.SetActive(isOrderPlaced && !isOrderReceived);

            if (currentIndex == 2)
            {
                if (!isOrderPlaced)
                {
                    GameObject.Find("OrderManager").GetComponent<OrderManager>().listOfOrders.Add(order);
                }
                isOrderPlaced = true;
            }
           
            if (currentIndex == 3 && !isOrderReceived)
            {
                this.transform.rotation = Quaternion.Euler(0, 0, 0);

            }
            else
            {
                Vector3 targetDirection = waypoints[currentIndex].position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
            }

            // If waiting at waypoint 4, set Idle animation
            // when this transform close by waypoiint 3
            if (Vector3.Distance(transform.position, waypoints[3].transform.position) < 1f)
            {
                if (animator != null)
                {
                    animator.SetBool("isIdle", true);
                }
            }

        }
    }

    private IEnumerator OrderMovement()
    {
        while (currentIndex < waypoints.Length)
        {
            Transform targetWaypoint = waypoints[currentIndex];
            // When reaching waypoint 3, wait for order to be received
            if (currentIndex == 3)
            {
                if (!isOrderReceived)
                {
                    if (animator != null)
                    {
                        animator.SetBool("isIdle", true);
                    }

                    yield return new WaitUntil(() => isOrderReceived);
                }
            }
            
            if (currentIndex == 4)
            {
                yield return new WaitForSeconds(Random.Range(0.5f, 10.0f));

            }

            // Move towards the target waypoint
            while (Vector3.Distance(transform.position, targetWaypoint.position) > 0.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);
                if (animator != null)
                {
                    animator.SetBool("isIdle", false);
                }
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

        return randomNumber == 0 ? "Caffe Latte" : "Long Black";
    }

    private void MoveAI()
    {
        // Check for nearby AIs within the detection radius
        Collider[] nearbyAIs = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (var ai in nearbyAIs)
        {
            if ((ai.CompareTag("AI") && ai.gameObject != gameObject))
            {
                float distanceToAI = Vector3.Distance(transform.position, ai.transform.position);

                if (distanceToAI < alignmentDistance)
                {
                    // Align the AI (move apart)
                    //Vector3 directionAwayFromAI = transform.position - ai.transform.position;
                    Vector3 directionAwayFromAI = transform.position - ai.transform.position;
                    Vector3 alignedPosition = transform.position + directionAwayFromAI.normalized * alignmentDistance;

                    // Move the AI to the new aligned position
                    transform.position = alignedPosition;
                }
            }
        }

    }


}
