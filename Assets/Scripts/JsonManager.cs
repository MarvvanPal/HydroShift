using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Microsoft.MixedReality.Toolkit;

public class JsonManager : MonoBehaviour
{
    //initialize the variables needed
    private string databaseFolderPath = "Assets/Database";
    private string jsonFilePath;
    private Dictionary<string, GroceryItem> groceryItems;

    // Start is called before the first frame update
    void Start()
    {
        jsonFilePath = Path.Combine(databaseFolderPath, "groceryItems.json");
        InitializeGroceryItems();
        WriteGroceryItemsToJson();
    }

    private void InitializeGroceryItems()
    {
        groceryItems = new Dictionary<string, GroceryItem>
        {
            {
                "Avocado",
                new GroceryItem
                {
                    EAN = 1234567890,
                    Category = "Vegetables",
                    WaterConsumedPerPiece = 800.0f
                }
            },
            {
                "Chocolate",
                new GroceryItem
                {
                    EAN = 9876543210,
                    Category = "Sweets",
                    WaterConsumedPerPiece = 1700.0f
                }
            },
            {
                "Beef",
                new GroceryItem
                {
                    EAN = 1234098765,
                    Category = "Meat",
                    WaterConsumedPerPiece = 15000.0f
                }
            }
        };
    }

    private void WriteGroceryItemsToJson()
    {
        string json = JsonUtility.ToJson(new SerializableDictionary(groceryItems), true);
        File.WriteAllText(jsonFilePath, json);
    }

    public GroceryItem FindItemByName(string name)
    {
        if (groceryItems.TryGetValue(name, out GroceryItem item))
        {
            return item;
        }
        else
        {
            Debug.LogError($"Item {name} not found in database.");
            return null;
        }
    }

    public float GetWaterConsumedPerPiece(string name)
    {
        GroceryItem item = FindItemByName(name);
        if (item != null)
        {
            return item.WaterConsumedPerPiece;
        }
        else
        {
            return 300f;
        }
    }
}

[System.Serializable]
public class GroceryItem
{
    public long EAN;
    public string Category;
    public float WaterConsumedPerPiece;
}

[System.Serializable]
public class SerializableDictionary : ISerializationCallbackReceiver
{
    public List<string> keys;
    public List<GroceryItem> values;
    private Dictionary<string, GroceryItem> target;

    public SerializableDictionary(Dictionary<string, GroceryItem> target)
    {
        this.target = target;
    }

    public void OnBeforeSerialize()
    {
        keys = new List<string>(target.Keys);
        values = new List<GroceryItem>(target.Values);
    }

    public void OnAfterDeserialize()
    {
        int count = Mathf.Min(keys.Count, values.Count);
        target = new Dictionary<string, GroceryItem>(count);
        for (int i = 0; 1 < count; ++i)
        {
            target[keys[i]] = values[i];
        }
    }

}
