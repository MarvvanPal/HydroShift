using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIConnectionController : MonoBehaviour
{
    [SerializeField] private JsonManager jsonManager;
    public string dataUrl = "http://192.168.0.5:5017/groceryitems";
    
    internal void GetAllGroceryItems()
    {
        StartCoroutine(GetAllGroceryItemsFromApi());
    }

    internal void GetOneGroceryItem(string itemName)
    {
        StartCoroutine(GetOneGroceryItemFromApi(itemName));
    }

    private IEnumerator GetAllGroceryItemsFromApi()
    {
        UnityWebRequest getRequest = UnityWebRequest.Get(dataUrl);
        getRequest.useHttpContinue = true;
        var request = getRequest.SendWebRequest();
        yield return new WaitUntil(() => request.isDone);

        if (getRequest.result == UnityWebRequest.Result.ConnectionError ||
            getRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error while fetching data: " + getRequest.error);
        }

        string jsonResponse = getRequest.downloadHandler.text;

        Debug.LogWarning(jsonResponse);

        APIGroceryResponse apiAPIGroceryResponse = JsonUtility.FromJson<APIGroceryResponse>(jsonResponse);
        
        if (apiAPIGroceryResponse != null)
        {
            foreach (var item in apiAPIGroceryResponse.groceryItems)
            {
                //jsonManager.AddGroceryItemToDictionary(item.name, item.details);
            }
        }
    }

    private IEnumerator GetOneGroceryItemFromApi(string itemName)
    {
        UnityWebRequest getOneItemRequest = UnityWebRequest.Get(dataUrl + $"/{itemName}");
        getOneItemRequest.useHttpContinue = true;
        var oneItemRequest = getOneItemRequest.SendWebRequest();
        yield return new WaitUntil(() => oneItemRequest.isDone);
        
        if (getOneItemRequest.result == UnityWebRequest.Result.ConnectionError ||
            getOneItemRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error while fetching data: " + getOneItemRequest.error);
        }

        string oneItemJsonResponse = getOneItemRequest.downloadHandler.text;
        GroceryItem groceryItem = JsonUtility.FromJson<GroceryItem>(oneItemJsonResponse);

        Debug.Log("Name: " + groceryItem.name);
        Debug.Log("Water consumed: " + groceryItem.details.waterConsumedPerPiece);

    }
}