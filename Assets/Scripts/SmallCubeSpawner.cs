using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallCubeSpawner : MonoBehaviour
{

    [SerializeField] private JsonManager jsonManager;
    [SerializeField] private GameObject cubePrefab;

    // Adjust the size of the cubes if necessary    
    private Vector3 smallCubeSize;

    // Variabale to determine the amount of cubes to be spawned, will be calculated
    private int cubesToBeSpawned;

    // Set a maximum amount of cubes to be spawned
    private int maxAmountOfCubes;

    // Volume variable to be filled by a request to the json file
    private float volume;

    // How often do you want the cubes to be spawned?
    private float spawnRate = 0.2f;

    // The water consumtion of which item do you want the cubes to represent? 
    public string itemName = "Cheese";

    private void Start()
    {      
        maxAmountOfCubes = 30;
        volume = jsonManager.groceryItems[itemName].waterConsumedPerPiece;
        cubesToBeSpawned = CalculateCubesToBeSpawned(volume, maxAmountOfCubes);
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


    private int CalculateCubesToBeSpawned(float volume, int maxAmountOfCubes)
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
