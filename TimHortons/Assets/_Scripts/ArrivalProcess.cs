using System;
using System.Collections;
using System.IO;  // For file handling
using UnityEngine;

public class ArrivalProcess : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform customerSpawnPlace;
    public SimulationParameters simulationParameters;
    public bool generateArrivals;

    private string filePath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(GenerateArrivals());
    }

    // Coroutine to generate customer arrivals based on inter-arrival time
    private IEnumerator GenerateArrivals()
    {
        while (generateArrivals)
        {
            // Instantiate a customer at the spawn location
            Instantiate(customerPrefab, customerSpawnPlace.position, Quaternion.identity);

            // Calculate inter-arrival time using exponential distribution
            float interArrivalTime = -Mathf.Log(1 - UnityEngine.Random.value) / simulationParameters.lambda;

            // Print to console (optional)
            print($"interArrivalTime = {interArrivalTime}");

            // Wait for the calculated inter-arrival time before generating the next arrival
            yield return new WaitForSeconds(interArrivalTime);
        }
    }

}
