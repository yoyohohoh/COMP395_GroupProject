using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoffeeMakerController : MonoBehaviour
{
    public int totalSteps;
    public List<int> sequence;
    public Text stepText;
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
        if (sequence.Count == totalSteps)
        {
            Debug.Log("Checking steps");
            for (int i = 0; i < totalSteps; i++)
            {
                if (sequence[i] != i)
                {
                    Debug.Log("Wrong sequence");
                    return false;
                }
            }
            Debug.Log("Correct sequence");
            return true;
        }
        else if (sequence.Count > totalSteps)
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
        if(sequence.Count == totalSteps)
        {
            sequence.Clear();
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
                sequence.Add(0);
                break;
            case "Water":
                Debug.Log("Adding water to the coffee");
                RemoveEverything();
                transform.Find("AddWater").gameObject.SetActive(true);
                sequence.Add(1);
                break;
            case "Espresso":
                Debug.Log("Adding espresso to the coffee");
                RemoveEverything();
                transform.Find("AddEspresso").gameObject.SetActive(true);
                sequence.Add(2);
                break;
            case "Milk":
                Debug.Log("Adding milk to the coffee");
                break;
            case "Sugar":
                Debug.Log("Adding sugar to the coffee");
                break;
            default:
                Debug.Log("No object selected");
                break;
        }
    }
}
