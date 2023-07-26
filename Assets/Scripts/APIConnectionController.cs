using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIConnectionController : MonoBehaviour
{

    public string dataUrl = "http://192.168.0.5:5017/groceryitems";

    // Rename this method to "request" and make it public so that it can be called by the json manager
    void Start()
    {
        StartCoroutine(MakeRequestToApi());
    }

    private IEnumerator MakeRequestToApi()
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

        //JsonUtility.FromJsonOverwrite(jsonResponse, apiAPIGroceryResponse);

        foreach (var item in apiAPIGroceryResponse.groceryItems)
        {
            Debug.Log(item.name);
            Debug.Log(item.details.category);
            Debug.Log(item.details.waterConsumedPerPiece);
        }
    }
}