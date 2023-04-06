using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallCubeSpawner : MonoBehaviour
{

    public JsonManager jsonManager;
    public GameObject cubePrefab;
         
    private int cubesToBeSpawned = 0;
    private int amountOfCubes;
    private float update;
    private float volume = 0f;

    // How often do you want the cubes to be spawned?
    private float spawnRate = 0.1f;

    // Which item do you want the cubes to represent? 
    private string itemName = "Avocado";

    private void Start()
    {
        //volume = jsonManager.GetWaterConsumedPerPiece(itemName);
        //int cubesToBeSpawned = (int)Mathf.Round(volume) / 10;
        cubesToBeSpawned = 50;
        InvokeRepeating("SpawnCubeWrapper", 0f, spawnRate);

    }

    /*
    void Awake()
    {
        update = 0.0f;
    }
    
    // Update is called once per frame
    void Update()
    {

        update += Time.deltaTime;

        if (update > 0.1f)
        {
            if (amountOfCubes <= cubesToBeSpawned)
            {
                Vector3 randomSpawnPosition = new Vector3(Random.Range(-2, 2), Random.Range(5, 8), Random.Range(2, 3));
                Instantiate(cubePrefab, randomSpawnPosition, Quaternion.identity);
            }

            update = 0.0f;
            amountOfCubes += 1;
        }
    }
    */
    private void SpawnCube(int amountOfCubes, int cubesToBeSpawned)
    {
        if (amountOfCubes <= cubesToBeSpawned)
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-2, 2), Random.Range(5, 8), Random.Range(2, 3));
            Instantiate(cubePrefab, randomSpawnPosition, Quaternion.identity);
            amountOfCubes += 1;
        }
    }

    private void SpawnCubeWrapper()
    {
        SpawnCube(amountOfCubes, cubesToBeSpawned);
    }

}
