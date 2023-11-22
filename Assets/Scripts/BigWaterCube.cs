using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BigWaterCube : MonoBehaviour
{
    [SerializeField] private JsonManager jsonManager;
    [SerializeField] private MeshRenderer cubeMesh;
    [SerializeField] private MeshRenderer liquidMesh;
    [SerializeField] private Animator animator;

    private string itemName = "Cheese";
    private float volume;

    private Tuple<float, float, float> dimensionsOfCube;
    private float volumeInCubicMeters;
    
    public void AdjustCubeScaleToVolume()
    {
        if (volume == 0)
        {    
            volume = 500f;
        }
        
        volumeInCubicMeters = VolInM3(volume);
        dimensionsOfCube = GetCubeDimensions(volumeInCubicMeters);
        GetComponent<Transform>().localScale = new Vector3(dimensionsOfCube.Item1, dimensionsOfCube.Item2, dimensionsOfCube.Item3);
        
        SetActiveRunAnimationsAndEnableMeshRenderer();
    }

    private void SetActiveRunAnimationsAndEnableMeshRenderer()
    {
        gameObject.SetActive(true);
        cubeMesh.enabled = true;
        liquidMesh.enabled = true;
        animator.Play("BigWaterCubeAnimation");
    }

    private static float VolInM3(float volume)
    {
        float volInM3 = volume / 1000;
        return volInM3;
    }

    private Tuple<float, float, float> GetCubeDimensions(float volumeInM3)
    {
        float width, height, length;
        
        if (volumeInM3 <= 2.5)
        {
            width = 1;
            length = 1;
            height = volumeInM3;
        }

        else if (volumeInM3 <= 8 && volumeInM3 > 2.5)
        {
            width = 2;
            length = 2;
            height = volumeInM3 / 4;
        }

        else
        {
            width = 2.5f;
            length = 2.5f;
            height = volumeInM3 / 6.25f;
        }
        return Tuple.Create(width, height, length);
    }
}

