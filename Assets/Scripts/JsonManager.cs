using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonManager : MonoBehaviour
{
    [SerializeField] private APIConnectionController apiConnectionController;
    public Dictionary<string, GroceryItemDetails> groceryItems = new();
    
    // For later, one item request use:
    //private Dictionary<string, GroceryItemDetails> oneGroceryItem = new();
    void Start()
    {
        groceryItems.Clear();
        apiConnectionController.GetAllGroceryItems();
    }

    internal void AddGroceryItemToDictionary(string itemName, GroceryItemDetails details)
    {
        groceryItems.Add(itemName, details);
    }
    
    

}


