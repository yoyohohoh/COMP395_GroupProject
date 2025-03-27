using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public List<string> listOfOrders;
    public List<string> listOfCompletedOrder;
    public List<string> listOfCompletedCoffee;
    public List<string> listOfIngredientsAdded;
    public Dictionary<string, string[]> recipes = new Dictionary<string, string[]>();

    void Start()
    {
        recipes["Long Black"] = new string[] { "Ice", "Water", "Espresso" };
        recipes["Caffee Latte"] = new string[] { "Espresso", "Milk", "Foam" };
    }

    void Update()
    {
        if (listOfIngredientsAdded.Count == 3)
        {
            CheckIngredients(listOfIngredientsAdded.ToArray());
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
            if(completedOrder == orderReceived)
            {
                listOfCompletedCoffee.Remove(completedOrder);
                listOfCompletedOrder.Add(completedOrder);
                return true;
            }
        }
        return false;
    }
}
