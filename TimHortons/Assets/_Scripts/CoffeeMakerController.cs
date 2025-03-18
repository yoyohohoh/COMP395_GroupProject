using UnityEngine;

public class CoffeeMakerController : MonoBehaviour
{
    public string objectName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void RemoveEverything()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        transform.Find("Glass").gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        switch (objectName)
        {
            case "Ice":
                Debug.Log("Adding ice to the coffee");
                RemoveEverything();
                transform.Find("AddIce").gameObject.SetActive(true);
                break;
            case "Water":
                Debug.Log("Adding water to the coffee");
                RemoveEverything();
                transform.Find("AddWater").gameObject.SetActive(true);
                break;
            case "Espresso":
                Debug.Log("Adding espresso to the coffee");
                RemoveEverything();
                transform.Find("AddEspresso").gameObject.SetActive(true);
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
