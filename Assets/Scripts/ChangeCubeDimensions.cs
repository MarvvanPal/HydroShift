using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCubeDimensions : MonoBehaviour
{
    Tuple<float, float, float> dimensionsOfCube;
    // TODO: add the watercube class and set in the values from the GetCubeDimensions Method into the scale vector3
    // Start is called before the first frame update
    void Start()
    {
        dimensionsOfCube = GetCubeDimensions();
        Transform cubeTransform = GetComponent<Transform>();
        cubeTransform.localScale = new Vector3(dimensionsOfCube.Item1, dimensionsOfCube.Item2, dimensionsOfCube.Item3);
    }

    public Tuple<float, float, float> GetCubeDimensions(float m3 = 0.7f)
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