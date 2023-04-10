using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallCubeSpawner : MonoBehaviour
{

    public JsonManager jsonManager;
    public GameObject cubePrefab;
         
    private int cubesToBeSpawned;
    private float volume;

    // How often do you want the cubes to be spawned?
    private float spawnRate = 0.7f;

    // Which item do you want the cubes to represent? 
    private string itemName = "Milk";

    private void Start()
    {
        volume = jsonManager.GetWaterConsumedPerPiece(itemName);
        cubesToBeSpawned = (int)Mathf.Round(volume) / 100;

        //InvokeRepeating("SpawnCubeWrapper", 0f, spawnRate);

        StartCoroutine(SpawnCube(cubesToBeSpawned, spawnRate));
    }


    
    private IEnumerator SpawnCube(int cubesToBeSpawned, float delay)
    {
        for (int i = 0; i <= cubesToBeSpawned; i++)
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-2, 2), Random.Range(5, 8), Random.Range(2, 3));
            Instantiate(cubePrefab, randomSpawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(delay);
        }
    }   

}
