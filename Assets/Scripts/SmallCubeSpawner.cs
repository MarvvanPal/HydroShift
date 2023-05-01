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
    private int maxAmountOfCubes;

    // Volume variable to be filled by a request to the json file
    private float volume;

    // How often do you want the cubes to be spawned?
    private float spawnRate = 0.2f;

    // The water consumption of which item do you want the cubes to represent? 
    private string itemName = "Cereal Grains";

    private void Start()
    {
        //jsonManager = GetComponent<JsonManager>();
        GameObject jsonManagerObject = GameObject.FindWithTag("JsonManagerTag"); // Ersetzen Sie "JsonManagerTag" durch das von Ihnen zugewiesene Tag
        jsonManager = jsonManagerObject.GetComponent<JsonManager>();
        maxAmountOfCubes = 30;
        volume = jsonManager.GetWaterConsumedPerPiece(itemName);
        //volume = 1700f; // Workaround, because otherwise script won't run
        cubesToBeSpawned = calculateCubesToBeSpawned(volume, maxAmountOfCubes);
        smallCubeSize = GetSmallCubeDimensions(cubesToBeSpawned, maxAmountOfCubes, volume);

        Debug.Log($"volume: {volume}");
        Debug.Log($"cubesToBeSpawned: {cubesToBeSpawned}");
        Debug.Log($"smallCubeSize: {smallCubeSize}");

        StartCoroutine(SpawnSmallCubes(cubesToBeSpawned, spawnRate, smallCubeSize));
    }


    
    private IEnumerator SpawnSmallCubes(int cubesToBeSpawned, float delay, Vector3 smallCubeSize)
    {
        for (int i = 0; i <= cubesToBeSpawned; i++)
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-0.8f, -1.5f), Random.Range(-1, 1.5f), Random.Range(1, 2));

            GameObject spawnedCube = Instantiate(cubePrefab, randomSpawnPosition, Quaternion.identity);
            spawnedCube.transform.localScale = smallCubeSize;

            yield return new WaitForSeconds(delay);
        }
    }   


    private int calculateCubesToBeSpawned(float volume, int maxAmountOfCubes)
    {
        int cubesToBeSpawned;

        if ((((int)Mathf.Round(volume) / 10) - 1) > maxAmountOfCubes)
        {
            cubesToBeSpawned = maxAmountOfCubes;
        }

        else
        {
            cubesToBeSpawned = ((int)Mathf.Round(volume) / 10) - 1;
        }
        return cubesToBeSpawned;
    }


    private Vector3 GetSmallCubeDimensions(int cubesToBeSpawned, int maxAmountOfCubes, float volume)
    {
        Vector3 smallCubeSize = new Vector3(0f, 0f, 0f);

        if (cubesToBeSpawned < maxAmountOfCubes)
        {
            smallCubeSize = new Vector3(0.22f, 0.22f, 0.22f);
            
        }

        else
        {
            float cubeVolumeInM3 = (volume / (float)maxAmountOfCubes) / 1000f;
            float dimensions = Mathf.Pow(((float)Mathf.Round(cubeVolumeInM3 * 100) / 100), (1f/3f));
            smallCubeSize = new Vector3(x : dimensions, y : dimensions, z : dimensions);
        }

        return smallCubeSize;
    }
}
