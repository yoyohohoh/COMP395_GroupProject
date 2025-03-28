﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Unity.Burst.Intrinsics;

public class DataKeeper : MonoBehaviour
{
    public static DataKeeper Instance;
    // PJ1
    public int totalCustomerCount;

    // PJ2
    public List<string> taskList;
    public int totalTask;
    public string currentScene;
    public bool isTutorialCompleted;
    public float targetSale;
    public float todaySale;
    public string playerLevel;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Subscribe to scene change event
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = SceneManager.GetActiveScene().name;
        // Scene scene, LoadSceneMode mode, they are argument when do SceneManager.sceneLoaded
        RecordCurrentTask();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //currentScene = SceneManager.GetActiveScene().name;
        //RecordCurrentTask();
    }

    // Update is called once per frame
    void Update()
    {
        // PJ1
        if (SceneManager.GetActiveScene().name == "GameOver")
        {
            //GameObject.Find("CustomerCount").GetComponentInChildren<Text>().text = "Today's Customer: " + totalCustomerCount.ToString();
            GameObject.Find("CustomerCount").GetComponentInChildren<Text>().text = "Daily Sales: $" + todaySale.ToString();
            GameObject.Find("PlayerLevel").GetComponentInChildren<Text>().text = "You are promoted to " + playerLevel + " !";
        }

        if (taskList.Contains("LongBlack") && taskList.Contains("CaffeLatte"))
        {
            Debug.Log("Tutorial Completed");
            isTutorialCompleted = true;
        }
    }
    
    public void RecordCurrentTask()
    {
        if (taskList.Count == 0 || !taskList.Contains(currentScene))
        {
            taskList.Add(currentScene);
            Debug.Log("Recorded Scene: " + currentScene);
        }
        else
        {
            Debug.Log("Scene already recorded: " + currentScene);
        }
    }
}
