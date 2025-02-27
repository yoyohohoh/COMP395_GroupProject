using System.Collections;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    public Waypoints waypoints;
    public Transform[] selectedRoute;
    public int currentWaypointIndex = 0;
    private bool hasRotated = false; // Track if rotation has been done
    private float moveSpeed;

    void Start()
    {
        waypoints = GameObject.Find("Waypoints").GetComponent<Waypoints>();
        selectedRoute = waypoints.currentRoute;

        // Calculate move speed dynamically so the object reaches the last waypoint in serviceTime
        if (selectedRoute.Length >= 3)
        {
            float totalDistance = Vector3.Distance(selectedRoute[0].position, selectedRoute[2].position);
            moveSpeed = totalDistance / waypoints.serviceTime;
        }
        else
        {
            moveSpeed = 5f; // Default speed if there aren't enough waypoints
        }

        StartCoroutine(MoveThroughWaypoints());
    }

    void Update()
    {
        if (currentWaypointIndex >= selectedRoute.Length)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator MoveThroughWaypoints()
    {
        while (currentWaypointIndex < selectedRoute.Length)
        {
            Transform targetWaypoint = selectedRoute[currentWaypointIndex];

            // Move towards the target waypoint
            while (Vector3.Distance(transform.position, targetWaypoint.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);
                yield return null; // Wait for the next frame
            }

            // Stop and wait at selectedRoute[1]
            if (currentWaypointIndex == 1 && !hasRotated)
            {
                yield return new WaitForSeconds(waypoints.waitTime); // Wait before rotating
                yield return StartCoroutine(Rotate180Degrees()); // Rotate before proceeding
                hasRotated = true; // Ensure rotation happens only once
            }

            // Move to the next waypoint
            currentWaypointIndex++;
        }
    }

    private IEnumerator Rotate180Degrees()
    {
        float rotationSpeed = 180f; // Degrees per second
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 180, 0); // Rotate 180 degrees

        while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }
    }
}
