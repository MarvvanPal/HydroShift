using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaterCube : MonoBehaviour
{
    //public GameObject Cube;
    /*
    public void SpawnCube(float width, float height, float length)
    {
        Vector3 spawnposition = new Vector3(1, 1, 1);
        Quaternion spawnRotation = Quaternion.identity;
        Vector3 spawnScale = new Vector3(width, height, length);

        GameObject newObject = Instantiate(Cube, spawnposition, spawnRotation);
        newObject.transform.localScale = spawnScale;
    }
    */
    public float volInM3(float volume)
    {
        float volInM3 = volume / 1000;
        return volInM3;
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

        else if(m3 <= 8 && m3 > 2.5)
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
