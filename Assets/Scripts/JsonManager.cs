using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Microsoft.MixedReality.Toolkit;

[System.Serializable]
public class GroceryItem
{
    public long EAN;
    public string Category;
    public float WaterConsumedPerPiece;
    public float MassPerPieceInGram;
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
                    EAN = 2694560410,
                    Category = "Vegetables",
                    WaterConsumedPerPiece = 170.0f,
                    MassPerPieceInGram = 200.0f

                }
            },
            {
                "Chocolate",
                new GroceryItem
                {
                    EAN = 9568921812,
                    Category = "Sweets",
                    WaterConsumedPerPiece = 1700.0f,
                    MassPerPieceInGram = 100.0f
                }
            },
            {
                "Beef",
                new GroceryItem
                {
                    EAN = 3494725170,
                    Category = "Meat",
                    WaterConsumedPerPiece = 4650.0f,
                    MassPerPieceInGram = 300
                }
            },
            {
                "Millet",
                new GroceryItem
                {
                    EAN = 8207461166,
                    Category = "Cereal Grains",
                    WaterConsumedPerPiece = 2500.0f,
                    MassPerPieceInGram = 500.0f
                }
            },
            {
                "Toast",
                new GroceryItem
                {
                    EAN = 8672704622,
                    Category = "Bread",
                    WaterConsumedPerPiece = 650.0f,
                    MassPerPieceInGram = 500.0f
                }
            },
            {
                "Burger",
                new GroceryItem
                {
                    EAN = 3587403851,
                    Category = "Meat",
                    WaterConsumedPerPiece = 2500.0f,
                    MassPerPieceInGram = 150.0f
                }
            },
            {
                "Barley",
                new GroceryItem
                {
                    EAN = 7533822919,
                    Category = "Cereal Grains",
                    WaterConsumedPerPiece = 650.0f,
                    MassPerPieceInGram = 500.0f
                }
            },
            {
                "Sorghum",
                new GroceryItem
                {
                    EAN = 4976045919,
                    Category = "Cereal Grains",
                    WaterConsumedPerPiece = 1400.0f,
                    MassPerPieceInGram = 500.0f
                }
            },
            {
                "Cane Sugar",
                new GroceryItem
                {
                    EAN = 3404068827,
                    Category = "Baking Supplies",
                    WaterConsumedPerPiece = 750.0f,
                    MassPerPieceInGram = 500.0f
                }
            },
            {
                "Tea",
                new GroceryItem
                {
                    EAN = 1369121209,
                    Category = "Drinks",
                    WaterConsumedPerPiece = 90.0f,
                    MassPerPieceInGram = 750.0f,
                }
            },
            {
                "Coffee",
                new GroceryItem
                {
                    EAN = 9817029772,
                    Category = "Drinks",
                    WaterConsumedPerPiece = 840.0f,
                    MassPerPieceInGram = 750.0f

                }
            },
            {
                "Milk",
                new GroceryItem
                {
                    EAN = 4507482257,
                    Category = "Dairy Products",
                    WaterConsumedPerPiece = 1000.0f,
                    MassPerPieceInGram = 1000.0f
                }
            },
            {
                "Cheese",
                new GroceryItem
                {
                    EAN = 4854009171,
                    Category = "Dairy Products",
                    WaterConsumedPerPiece = 2500.0f,
                    MassPerPieceInGram = 500.0f
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


