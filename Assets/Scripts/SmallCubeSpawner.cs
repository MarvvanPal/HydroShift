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

    // Adjust the size of the cubes if necessary    
    private Vector3 smallCubeSize;

    // Variable to determine the amount of cubes to be spawned, will be calculated
    private int cubesToBeSpawned;

    // Set a maximum amount of cubes to be spawned
    public int maxAmountOfCubes = 30;

    // Volume variable to be filled by a request to the json file
    public float volume;

    // How often do you want the cubes to be spawned?
    public readonly float spawnRate = 0.2f;

    // The water consumption of which item do you want the cubes to represent? 
    public string itemName = "Cheese";
  
    public void SpawnSmallCubes()
    {
        volume = 1700f/*jsonManager.GroceryItems[itemName].waterConsumedPerPiece*/;
        cubesToBeSpawned = CalculateCubesToBeSpawned(volume, maxAmountOfCubes);
        smallCubeSize = GetSmallCubeDimensions(cubesToBeSpawned, maxAmountOfCubes, volume);
        
        StartCoroutine(SpawningSmallCubes(cubesToBeSpawned, spawnRate, smallCubeSize));
    }
    
    private IEnumerator SpawningSmallCubes(int cubesToBeSpawned, float delay, Vector3 smallCubeSize)
    {
        for (int i = 1; i <= cubesToBeSpawned; i++)
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-0.8f, -1.5f), Random.Range(-1, 1.5f), Random.Range(1, 2));

            GameObject spawnedCube = Instantiate(cubePrefab, randomSpawnPosition, Quaternion.identity);
            spawnedCube.transform.localScale = smallCubeSize;

            yield return new WaitForSeconds(delay);
        }
    }   


    internal int CalculateCubesToBeSpawned(float volume, int maxAmountOfCubes)
    {
        int cubesToBeSpawned = Mathf.RoundToInt(volume / 10);
        
        return Mathf.Clamp(cubesToBeSpawned, 0, maxAmountOfCubes);
    }


    internal Vector3 GetSmallCubeDimensions(int cubesToBeSpawned, int maxAmountOfCubes, float volume)
    {       

        if (cubesToBeSpawned < maxAmountOfCubes)
        {
            smallCubeSize = new Vector3(0.22f, 0.22f, 0.22f);
            
        }

        else
        {
            float cubeVolumeInM3 = volume / maxAmountOfCubes / 1000f;
            float dimensions = Mathf.Pow(Mathf.Round(cubeVolumeInM3 * 100) / 100, 1f/3f);
            smallCubeSize = new Vector3(x : dimensions, y : dimensions, z : dimensions);
        }

        return smallCubeSize;
    }
}
