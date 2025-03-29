using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrderGenerator : MonoBehaviour
{
    public GameObject[] customerPrefabs;
    public GameObject customerPrefab;
    public Transform customerSpawnPlace;
    public bool generateOrders;
    public int orderCount;

    public SimulationParameters simulationParameters;
    public float startTime;
    public float endTime;
    public float interArrivalTime;
    public float waitingTime;

    private Coroutine orderCoroutine;
    private bool isPaused = false;  // Prevent multiple pauses
    private bool isSlowMode = false; // Control slow mode when many orders exist

    void Start()
    {
        orderCount = 0;
        startTime = simulationParameters.StartTime;
        endTime = simulationParameters.EndTime;
        generateOrders = true;
        orderCoroutine = StartCoroutine(GenerateArrivals());
    }

    private IEnumerator GenerateArrivals()
    {
        while (generateOrders)
        {
            GenerateOrder();
            interArrivalTime = (-Mathf.Log(1 - UnityEngine.Random.value) / simulationParameters.lambda) * simulationParameters.TimeScale * 3;

            // If too many orders, slow down instead of stopping completely
            waitingTime = isSlowMode ? interArrivalTime * 2 : interArrivalTime;

            yield return new WaitForSeconds(waitingTime);
        }
    }

    void GenerateOrder()
    {
        int randomInt = UnityEngine.Random.Range(0, customerPrefabs.Length);
        customerPrefab = customerPrefabs[randomInt];
        Instantiate(customerPrefab, customerSpawnPlace.position, Quaternion.identity);
        orderCount++;
    }

    public void Update()
    {
        if (generateOrders)
        {
            startTime += Time.deltaTime;
            if (startTime >= endTime)
            {
                generateOrders = false;
                if (orderCoroutine != null)
                {
                    StopCoroutine(orderCoroutine);
                    orderCoroutine = null;
                }
                GameObject.Find("DataKeeper").GetComponent<DataKeeper>().totalCustomerCount = orderCount;
                GameObject.Find("LevelController").GetComponent<LevelController>().LoadLevel("GameOver");
            }
        }

        int numberOfOrderOnWait = GameObject.FindGameObjectsWithTag("AI")
            .Count(obj => !obj.GetComponent<OrderController>().isOrderReceived);

        if (numberOfOrderOnWait <= 0)
        {
            Debug.Log("No Order On Wait");
            GenerateOrder();
            GenerateOrder();
            Debug.Log("Order Order");
        }
        else if (numberOfOrderOnWait >= 10 && !isPaused)
        {
            Debug.Log("Too Many Orders! Slowing down...");
            isSlowMode = true;  // Instead of stopping, slow down order generation
            isPaused = true;
            StartCoroutine(ResumeOrderGenerationAfterDelay(60f));
        }
        else if (numberOfOrderOnWait < 5 && isPaused)
        {
            Debug.Log("Orders cleared! Resuming normal speed...");
            isSlowMode = false; // Resume normal order speed
            isPaused = false;
        }
    }

    private IEnumerator ResumeOrderGenerationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (generateOrders && isPaused)
        {
            Debug.Log("Checking if we can resume order generation...");
            int numberOfOrderOnWait = GameObject.FindGameObjectsWithTag("AI")
                .Count(obj => !obj.GetComponent<OrderController>().isOrderReceived);

            if (numberOfOrderOnWait < 10)
            {
                Debug.Log("Resuming Order Generation...");
                isPaused = false;
                isSlowMode = false; // Resume normal speed
                if (orderCoroutine == null)
                {
                    orderCoroutine = StartCoroutine(GenerateArrivals());
                }
            }
            else
            {
                Debug.Log("Still too many orders. Waiting another 60 seconds...");
                StartCoroutine(ResumeOrderGenerationAfterDelay(60f));
            }
        }
    }
}
