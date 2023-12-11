using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UX;
[assembly: InternalsVisibleTo("EditMode")]
[assembly: InternalsVisibleTo("PlayMode")]


public class SmallCubeSpawner : MonoBehaviour
{

    [SerializeField] public JsonManager jsonManager;
    public GameObject cubePrefab;
    
    private Vector3 smallCubeSize;
    private int cubesToBeSpawned;
    public int maxAmountOfCubes = 30;
    public float volume;
    public readonly float spawnRate = 0.2f;
    public string itemName = "Cheese";
  
    public void SpawnSmallCubes()
    {
        if (volume == 0)
        {
            volume = jsonManager.GroceryItems[itemName].waterConsumedPerPiece;
        }
        
        cubesToBeSpawned = CalculateCubesToBeSpawned(volume);
        smallCubeSize = GetSmallCubeDimensions(volume, cubesToBeSpawned);
        
        StartCoroutine(SpawningSmallCubes(spawnRate));
    }
    
    private IEnumerator SpawningSmallCubes(float delay)
    {
        for (int i = 1; i <= cubesToBeSpawned; i++)
        {
            Vector3 randomSpawnPosition = new Vector3(
                UnityEngine.Random.Range(-0.8f, -1.5f),
                UnityEngine.Random.Range(-1, 1.5f),
                UnityEngine.Random.Range(1, 2));

            GameObject spawnedCube = Instantiate(cubePrefab, randomSpawnPosition, Quaternion.identity);
            spawnedCube.transform.localScale = smallCubeSize;

            yield return new WaitForSeconds(delay);
        }
    }

    internal int CalculateCubesToBeSpawned(float volumeOfItemInLiters)
    {
        int amountOfCubesWith10LiterVolume = Mathf.RoundToInt(volumeOfItemInLiters / 10);
        
        return Mathf.Clamp(amountOfCubesWith10LiterVolume, 0, maxAmountOfCubes);
    }

    internal Vector3 GetSmallCubeDimensions(float volumeOfItemInLiters, int amountOfCubesToBeSpawned)
    {       

        if (amountOfCubesToBeSpawned < maxAmountOfCubes)
        {
            smallCubeSize = new Vector3(0.22f, 0.22f, 0.22f);
        }

        else
        {
            float cubeVolumeInM3 = volumeOfItemInLiters / maxAmountOfCubes / 1000f;
            float dimensions = (float)Math.Round(Mathf.Pow(cubeVolumeInM3 * 100 / 100, 1f/3f), 2);
            smallCubeSize = new Vector3(x : dimensions, y : dimensions, z : dimensions);
        }

        return smallCubeSize;
    }
}
