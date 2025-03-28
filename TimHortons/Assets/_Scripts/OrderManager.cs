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

    public void CheckIngredients(string[] ingredients)
    {
        foreach (var recipe in recipes)
        {
            if (recipe.Value.SequenceEqual(ingredients))
            {
                listOfCompletedCoffee.Add(recipe.Key);
                listOfIngredientsAdded.Clear();
                ingredientsAddedTxt.text = "";
                coffeeMade.SetActive(true);
                coffeeMade.GetComponentInChildren<TextMeshProUGUI>().text = recipe.Key + " !";
                Invoke("CloseCoffeeMade", 1f);
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
                listOfCompletedCoffee.Remove(completedOrder);
                listOfCompletedOrder.Add(completedOrder);
                return true;
            }
        }
        return false;
    }
}
