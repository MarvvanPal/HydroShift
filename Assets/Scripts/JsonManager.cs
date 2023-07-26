using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Microsoft.MixedReality.Toolkit;
/*
[System.Serializable]
public partial class GroceryItem
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
        for (int i = 0; i < count; ++i)
        {
            target[keys[i]] = values[i];
        }
    }

    public Dictionary<string, GroceryItem> ToDictionary()
    {
        return target;
    }

}


public class JsonManager : MonoBehaviour
{
    private string databaseFolderPath = "Assets/Database";
    private string jsonFilePath;
    private Dictionary<string, GroceryItem> groceryItems;

    void Start()
    {
        jsonFilePath = Path.Combine(databaseFolderPath, "groceryItems.json");
        LoadGroceryItemsFromJson();        
    }

    private void LoadGroceryItemsFromJson()
    {
        if (File.Exists(jsonFilePath))
        {
            string json = File.ReadAllText(jsonFilePath);
            SerializableDictionary loadedData = JsonUtility.FromJson<SerializableDictionary>(json);
            groceryItems = loadedData.ToDictionary();
        }

        else
        {
            Debug.LogError($"Could not find file in {jsonFilePath}.");
        }
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
            Debug.LogError($"Could not return value for {name}.");
            return 1f;
        }
    }
}
*/

