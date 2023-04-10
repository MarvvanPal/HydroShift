using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallCubeSpawner : MonoBehaviour
{

    public JsonManager jsonManager;
    public GameObject cubePrefab;

    // Adjust the size of the cubes if necessary    
    private Vector3 smallCubeSize;

    // Varibale to determine the amount of cubes to be spawned, will be calculated
    private int cubesToBeSpawned;

    // Set a maximum amount of cubes to be spawned

    // Volume variable to be filled by a request to the json file
    private float volume;

    // How often do you want the cubes to be spawned?
    private float spawnRate = 0.3f;

    // The water consumption of which item do you want the cubes to represent? 
    private string itemName = "Burger";

    private void Start()
    {
        volume = jsonManager.GetWaterConsumedPerPiece(itemName);
        cubesToBeSpawned = calculateCubesToBeSpawned(volume);
        smallCubeSize = GetSmallCubeDimensions(cubesToBeSpawned, volume);

        Debug.Log($"volume: {volume}");
        Debug.Log($"cubesToBeSpawned: {cubesToBeSpawned}");
        Debug.Log($"smallCubeSize: {smallCubeSize}");

        StartCoroutine(SpawnSmallCubes(cubesToBeSpawned, spawnRate, smallCubeSize));
    }


    
    private IEnumerator SpawnSmallCubes(int cubesToBeSpawned, float delay, Vector3 smallCubeSize)
    {
        for (int i = 0; i <= cubesToBeSpawned; i++)
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-2, 2), Random.Range(5, 8), Random.Range(2, 3));

            GameObject spawnedCube = Instantiate(cubePrefab, randomSpawnPosition, Quaternion.identity);
            spawnedCube.transform.localScale = smallCubeSize;

            yield return new WaitForSeconds(delay);
        }
    }   


    private int calculateCubesToBeSpawned(float volume)
    {
        int cubesToBeSpawned;

        if ((((int)Mathf.Round(volume) / 10) - 1) > 100)
        {
            cubesToBeSpawned = 100;
        }

        else
        {
            cubesToBeSpawned = ((int)Mathf.Round(volume) / 10) - 1;
        }
        return cubesToBeSpawned;
    }


    private Vector3 GetSmallCubeDimensions(int cubesToBeSpawned, float volume)
    {
        Vector3 smallCubeSize = new Vector3(0f, 0f, 0f);

        if (cubesToBeSpawned < 100)
        {
            smallCubeSize = new Vector3(0.22f, 0.22f, 0.22f);
            
        }

        else
        {
            float cubeVolumeInM3 = (volume / 100) / 1000;
            float dimensions = (float)Mathf.Pow(((float)Mathf.Round(cubeVolumeInM3 * 100) / 100), (1f/3f));
            smallCubeSize = new Vector3(x : dimensions, y : dimensions, z : dimensions);
        }

        return smallCubeSize;
    }
}
