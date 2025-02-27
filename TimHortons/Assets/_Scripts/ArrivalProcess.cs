using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class ArrivalProcess : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform customerSpawnPlace;
    public SimulationParameters simulationParameters;
    public bool generateArrivals;
    public int customerCount;

    public float startTime;
    public float endTime;
    public float interArrivalTime;
    void Start()
    {
        customerCount = 0;
        startTime = simulationParameters.StartTime;
        endTime = simulationParameters.EndTime;
        StartCoroutine(GenerateArrivals());
    }

    private IEnumerator GenerateArrivals()
    {
        while (generateArrivals)
        {
            Instantiate(customerPrefab, customerSpawnPlace.position, Quaternion.identity);
            customerCount++;
            interArrivalTime = -Mathf.Log(1 - UnityEngine.Random.value) / simulationParameters.lambda;
            yield return new WaitForSeconds(interArrivalTime * simulationParameters.TimeScale);
        }
    }

    public void Update()
    {
        if(generateArrivals)
        {
            startTime += Time.deltaTime;
            if (startTime >= endTime)
            {
                generateArrivals = false;
                GameObject.Find("DataKeeper").GetComponent<DataKeeper>().totalCustomerCount = customerCount;
                SceneManager.LoadScene(2);
            }
        }
    }
}
