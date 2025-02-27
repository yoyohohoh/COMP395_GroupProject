using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public SimulationParameters simulationParameters;
    public Transform[][] waypointsRoutes;
    public Transform[] waypointsRoute1;
    public Transform[] waypointsRoute2;
    public Transform[] waypointsRoute3;
    public Transform[] currentRoute;
    private int currentRouteIndex = 0;
    ArrivalProcess arrivalProcess;
    public float serviceTime;
    public float waitTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waypointsRoutes = new Transform[][] { waypointsRoute1, waypointsRoute2, waypointsRoute3 };
        arrivalProcess = GameObject.Find("CustomerArrival").GetComponent<ArrivalProcess>();
    }

    // Update is called once per frame
    void Update()
    {
        if (arrivalProcess.generateArrivals)
        {
            currentRouteIndex = arrivalProcess.customerCount % 3;
            currentRoute = waypointsRoutes[currentRouteIndex];
            serviceTime = -Mathf.Log(1 - UnityEngine.Random.value) / simulationParameters.mu;
            waitTime = -Mathf.Log(1 - UnityEngine.Random.value) / simulationParameters.wt;
        }
    }
}
