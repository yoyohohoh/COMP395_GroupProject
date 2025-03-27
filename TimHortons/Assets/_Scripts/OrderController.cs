using System.Collections;
using UnityEngine;

public class OrderController : MonoBehaviour
{
    public Transform[] waypoints;
    public int currentIndex = 0;
    public float moveSpeed = 1f;
    public bool isOrderPlaced;
    public bool isOrderReceived;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isOrderPlaced = false;
        isOrderReceived = false;
        waypoints = GameObject.Find("Waypoints").GetComponent<OrderWaypoints>()._waypoints;
        StartCoroutine(OrderMovement());
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

        if(currentIndex >= waypoints.Length)
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

}
