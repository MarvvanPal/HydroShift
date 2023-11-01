using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using Microsoft.MixedReality.Toolkit.UX;
using Object = UnityEngine.Object;

public class SmallCubeSpawnerPlayModeTest
{
    private SmallCubeSpawner spawner;
    private PressableButton spawnButton;
    private GameObject mainMenuPrefab;
    private GameObject mainMenu;

    [SetUp]
    public void SetUp()
    {
        GameObject spawnerObject = new GameObject();
        spawner = spawnerObject.AddComponent<SmallCubeSpawner>();
        Object.Destroy(spawner.jsonManager);
        spawner.jsonManager = spawnerObject.AddComponent<MockJsonManager>();
        spawner.gameObject.SetActive(false);
        
        mainMenuPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/MainMenu.prefab");
        mainMenu = GameObject.Instantiate(mainMenuPrefab);
        spawnButton = mainMenu.GetComponentInChildren<PressableButton>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(spawner.gameObject);
    }

    [UnityTest]
    public IEnumerator SpawnerCreatesCorrectNumberOfCubes()
    {
        
        spawnButton.ForceSetToggled(true);
        
        yield return new WaitForSeconds(spawner.spawnRate * (spawner.maxAmountOfCubes + 1));

        int spawnedCubes = GameObject.FindGameObjectsWithTag("smallWaterCube").Length;
        Assert.AreEqual(spawner.maxAmountOfCubes, spawnedCubes);
    }

}

public class MockJsonManager : JsonManager
{
    public Dictionary<string, GroceryItem> GroceryItems =
        new()
        {
            { 
                "Cheese", 
                new GroceryItem 
                { 
                    name = "Cheese",
                    details = new GroceryItemDetails 
                    { 
                        waterConsumedPerPiece = 100f 
                    } 
                } 
            }
        };
}


