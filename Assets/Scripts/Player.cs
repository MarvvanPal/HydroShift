using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    private WaterCube Cube;
    float volume = 7000f;
    Tuple<float, float, float> dimensionsOfCube;
    float volInM3;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        Cube = GetComponent<WaterCube>();
        volInM3 = Cube.volInM3(volume);
        dimensionsOfCube = Cube.GetCubeDimensions(volInM3);        

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Main()
    {
        //Cube.SpawnCube(dimensionsOfCube.Item1, dimensionsOfCube.Item2, dimensionsOfCube.Item3);
        Animator animator = Cube.GetComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load("Assets/Animations/LetCubeFlyIn.controller") as RuntimeAnimatorController;

    }
}
