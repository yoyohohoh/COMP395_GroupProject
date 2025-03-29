using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Collections;

public class OrderManager : MonoBehaviour
{
    public TextMeshProUGUI ingredientsAddedTxt;
    public GameObject coffeeMade;
    public GameObject latte;
    public GameObject longblack;
    public TextMeshPro latteTxt;
    public TextMeshPro longblackTxt;
    public List<string> listOfOrders;
    public List<string> listOfCompletedOrder;
    public List<string> listOfCompletedCoffee;
    public List<string> listOfIngredientsAdded;
    public Dictionary<string, string[]> recipes = new Dictionary<string, string[]>();
    public float totalSale;
    public TextMeshPro totalSaleTxt;
    public TextMeshPro titleTxt;
    string playerLevel;
    void Start()
    {
        recipes["Long Black"] = new string[] { "Ice", "Water", "Espresso" };
        recipes["Caffe Latte"] = new string[] { "Espresso", "Milk", "Foam" };
        coffeeMade.SetActive(false);
        ingredientsAddedTxt.text = "";
    }

    void Update()
    {
        CoffeeLabel("Caffe Latte", latte, latteTxt);
        CoffeeLabel("Long Black", longblack, longblackTxt);
        totalSale = CalculateSale("Long Black") + CalculateSale("Caffe Latte");
        DataKeeper.Instance.todaySale = totalSale;
        totalSaleTxt.text = "$" + totalSale.ToString();        
        titleTxt.text = CheckPlayerLevel(totalSale);
        DataKeeper.Instance.playerLevel = playerLevel;
    }
    string CheckPlayerLevel(float sales)
    {
        switch (sales)
        {
            case >= 200:
                playerLevel = "Cafe Manager";
                break;
            case >= 180 and < 200:
                playerLevel = "Best Barista";
                break;
            case >= 60 and < 180:
                playerLevel = "Barista";
                break;
            default:
                playerLevel = "Cafe Trainee";
                break;
        }

        return playerLevel;
    }
    float CalculateSale(string coffeeName)
    {
        switch (coffeeName)
        {
            case "Long Black":
                return listOfCompletedOrder.Count(coffee => coffee == coffeeName) * 2.99f;
            case "Caffe Latte":
                return listOfCompletedOrder.Count(coffee => coffee == coffeeName) * 3.39f;
            default:
                return 0;
        }
    }

    public void OnIngredientAdded(string ingredient)
    {
        listOfIngredientsAdded.Add(ingredient);
        StartCoroutine(UpdateIngredientsTextWithDelay(0.2f));
    }

    public IEnumerator UpdateIngredientsTextWithDelay(float delay)
    {
        ingredientsAddedTxt.text = string.Join(" + ", listOfIngredientsAdded);
        yield return new WaitForSeconds(delay);

        if (listOfIngredientsAdded.Count == 3)
        {
            CheckIngredients(listOfIngredientsAdded.ToArray());
        }
    }
    public void CoffeeLabel(string coffeeName, GameObject coffee, TextMeshPro label)
    {
        coffee.SetActive(listOfCompletedCoffee.Contains(coffeeName));
        int coffeeInList = listOfCompletedCoffee.Count(coffee => coffee == coffeeName);
        if (coffeeInList >= 2)
        {
            label.GetComponent<TextMeshPro>().text = coffeeInList.ToString();
        }
        else
        {
            label.GetComponent<TextMeshPro>().text = "";
        }
    }
    public void ClearIngredients()
    {
        listOfIngredientsAdded.Clear();
        ingredientsAddedTxt.text = "";
    }
    public void CheckIngredients(string[] ingredients)
    {
        foreach (var recipe in recipes)
        {
            if (recipe.Value.SequenceEqual(ingredients))
            {
                listOfCompletedCoffee.Add(recipe.Key);
                ClearIngredients();
                coffeeMade.SetActive(true);
                coffeeMade.GetComponentInChildren<TextMeshProUGUI>().text = recipe.Key + " !";
                Invoke("CloseCoffeeMade", 0.3f);
                return;
            }
        }

        Debug.Log("Wrong Recipe");
        coffeeMade.SetActive(true);
        coffeeMade.GetComponentInChildren<TextMeshProUGUI>().text = "Wrong Recipe!";
        ingredientsAddedTxt.text = "";
        Invoke("CloseCoffeeMade", 1f);
        listOfIngredientsAdded.Clear();
    }
    void CloseCoffeeMade() => coffeeMade.SetActive(false);
    public bool CheckCompletedCoffee(string orderReceived)
    {
        foreach (var completedOrder in listOfCompletedCoffee)
        {
            if (completedOrder == orderReceived)
            {
                // play sound
                GameObject audioObject = GameObject.Find("Audio Source");
                if (audioObject)
                {
                    AudioSource audioSource = audioObject.AddComponent<AudioSource>();
                    AudioClip clip = Resources.Load<AudioClip>("BGM/cash-register");
                    if (clip)
                    {
                        audioSource.clip = clip;
                        audioSource.Play();

                        Destroy(audioSource, clip.length);
                    }
                }

                listOfCompletedCoffee.Remove(completedOrder);
                listOfCompletedOrder.Add(completedOrder);
                return true;
            }
        }
        return false;
    }
}
