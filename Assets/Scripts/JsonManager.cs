using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonManager : MonoBehaviour
{
    [SerializeField] private APIConnectionController apiConnectionController;
    public readonly Dictionary<string, GroceryItemDetails> GroceryItems = new();
    /*
    // For later, one item request use:
    //private Dictionary<string, GroceryItemDetails> oneGroceryItem = new();
    void Start()
    {
        GroceryItems.Clear();
        apiConnectionController.GetAllGroceryItems();
    }

    internal void AddGroceryItemToDictionary(string itemName, GroceryItemDetails details)
    {
        GroceryItems.Add(itemName, details);
    }
    */
}


