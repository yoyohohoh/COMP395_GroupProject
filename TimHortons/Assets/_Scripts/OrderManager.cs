using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class OrderManager : MonoBehaviour
{
    public TextMeshProUGUI ingredientsAddedTxt;
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
    }

    void Update()
    {
        if (listOfIngredientsAdded.Count == 3)
        {
            CheckIngredients(listOfIngredientsAdded.ToArray());
        }

        ingredientsAddedTxt.text = "Making Coffee\n" + "----------\n" + string.Join("\n", listOfIngredientsAdded);

        CoffeeLabel("Caffe Latte", latte, latteTxt);
        CoffeeLabel("Long Black", longblack, longblackTxt);


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
                return;
            }
        }

        Debug.Log("Wrong Recipe");
        listOfIngredientsAdded.Clear();
    }

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
