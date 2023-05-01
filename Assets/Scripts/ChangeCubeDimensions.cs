using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ChangeCubeDimensions : MonoBehaviour
{
    public JsonManager jsonManager;

    Tuple<float, float, float> dimensionsOfCube;
    float volumeInCubicMeters;

    // Database lookup is here:
    string itemName = "Chocolate";
    private float volume;


    // Start is called before the first frame update
    void Start()
    {
        // volume = jsonManager.GetWaterConsumedPerPiece(itemName);
        volume = 1700f; //Workaround, because script is not loading 
        volumeInCubicMeters = volInM3(volume);
        dimensionsOfCube = GetCubeDimensions(volumeInCubicMeters);
        Transform cubeTransform = GetComponent<Transform>();
        cubeTransform.localScale = new Vector3(dimensionsOfCube.Item1, dimensionsOfCube.Item2, dimensionsOfCube.Item3);
    }

    public float volInM3(float volume)
    {
        float volInM3 = volume / 1000;
        return volInM3;
    }

    public Tuple<float, float, float> GetCubeDimensions(float m3)
    {
        float width, height, length;

        if (m3 <= 2.5)
        {
            width = 1;
            length = 1;
            height = m3;
        }

        else if (m3 <= 8 && m3 > 2.5)
        {
            width = 2;
            length = 2;
            height = m3 / 4;
        }

        else
        {
            width = 2.5f;
            length = 2.5f;
            height = m3 / 6.25f;
        }
        return Tuple.Create(width, height, length);
    }
}

