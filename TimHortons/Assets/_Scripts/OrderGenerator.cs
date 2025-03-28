using System;
using System.Collections;
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        orderCount = 0;
        startTime = simulationParameters.StartTime;
        endTime = simulationParameters.EndTime;

        StartCoroutine(GenerateArrivals());
    }

    private IEnumerator GenerateArrivals()
    {
        while (generateOrders)
        {
            int randomInt = UnityEngine.Random.Range(0, customerPrefabs.Length);
            customerPrefab = customerPrefabs[randomInt];
            Instantiate(customerPrefab, customerSpawnPlace.position, Quaternion.identity);
            orderCount++;
            interArrivalTime = -Mathf.Log(1 - UnityEngine.Random.value) / simulationParameters.lambda;
            yield return new WaitForSeconds((interArrivalTime * simulationParameters.TimeScale) * 3);
        }
    }

    public void Update()
    {
        if (generateOrders)
        {
            startTime += Time.deltaTime;
            if (startTime >= endTime)
            {
                generateOrders = false;
                GameObject.Find("DataKeeper").GetComponent<DataKeeper>().totalCustomerCount = orderCount;
                // level done
            }
        }
    }
}
