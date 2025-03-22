using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoffeeMakerController : MonoBehaviour
{
    public List<string> taskSequence;
    public List<string> playerSequence;
    public Text stepText;

    private void Start()
    {

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        if (CheckSteps())
        { 
            stepText.text = "Make Coffee"; 
        }
        else
        {
            stepText.text = "Making Coffee";
        }
    }

    public bool CheckSteps()
    {
        if (playerSequence.Count == taskSequence.Count)
        {
            Debug.Log("Checking steps");
            for (int i = 0; i < taskSequence.Count; i++)
            {
                if (playerSequence[i] != taskSequence[i])
                {
                    Debug.Log("Wrong sequence");
                    return false;
                }
            }
            Debug.Log("Correct sequence");
            return true;
        }
        else if (playerSequence.Count > taskSequence.Count)
        {
            Debug.Log("Too many steps have been completed");
            return false;
        }
        else
        {
            Debug.Log("Not all steps have been completed");
            return false;
        }
    }

    public void ResetSteps()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MakeCoffee()
    {
        if (CheckSteps())
        { 
            Debug.Log("Make coffee");
            // load next scene
        }
        else
        {
            Debug.Log("Not all steps have been completed");
            ResetSteps();
        }
    }

    public void RemoveEverything()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        transform.Find("Glass").gameObject.SetActive(true);

        if(playerSequence.Count == taskSequence.Count)
        {
            playerSequence.Clear();
        }
    }
    // Update is called once per frame
    public void AddItems(string objectName)
    {
        switch (objectName)
        {
            case "Ice":
                Debug.Log("Adding ice to the coffee");
                RemoveEverything();
                transform.Find("AddIce").gameObject.SetActive(true);
                playerSequence.Add("ice");
                break;
            case "Water":
                Debug.Log("Adding water to the coffee");
                RemoveEverything();
                transform.Find("AddWater").gameObject.SetActive(true);
                playerSequence.Add("water");
                break;
            case "Espresso":
                Debug.Log("Adding espresso to the coffee");
                RemoveEverything();
                transform.Find("AddEspresso").gameObject.SetActive(true);
                playerSequence.Add("espresso");
                break;
            case "Milk":
                Debug.Log("Adding milk to the coffee");
                RemoveEverything();
                transform.Find("AddMilk").gameObject.SetActive(true);
                playerSequence.Add("milk");
                break;
            case "Foam":
                Debug.Log("Adding foam to the coffee");
                RemoveEverything();
                transform.Find("AddFoam").gameObject.SetActive(true);
                playerSequence.Add("foam");
                break;
            case "Sugar":
                Debug.Log("Adding sugar to the coffee");
                RemoveEverything();
                transform.Find("AddSugar").gameObject.SetActive(true);
                playerSequence.Add("sugar");
                break;
            default:
                Debug.Log("No object selected");
                break;
        }
    }
}
