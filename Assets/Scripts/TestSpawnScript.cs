using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawnScript : MonoBehaviour
{

    private WaterCube Cube;
    float volume = 1000f;
    Tuple<float, float, float> dimensionsOfCube;
    float volInM3;
    // Start is called before the first frame update
    void Start()
    {
        Cube = GetComponent<WaterCube>();
        volInM3 = Cube.volInM3(volume);
        dimensionsOfCube = Cube.GetCubeDimensions(volInM3);
        //Cube.SpawnCube(dimensionsOfCube.Item1, dimensionsOfCube.Item2, dimensionsOfCube.Item3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
