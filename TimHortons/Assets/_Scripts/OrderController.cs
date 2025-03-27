using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class OrderController : MonoBehaviour
{
    [Header("Movement")]
    public Transform[] waypoints;
    public int currentIndex = 0;
    public float moveSpeed = 1f;

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
        // move along waypoints

        // when reach waypoints[1] isOrderPlaced = true
        // go to w2
        // waiting for order making (game part)
        // game done, isOrderReceived = true 
        // go to w3
        orderTag.SetActive(isOrderPlaced);
        if (currentIndex == 2)
        {
            if(!isOrderPlaced)
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

}
