using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Microsoft.MixedReality.Toolkit.UX;


public class SmallCubeSpawnerPlayModeTest
{
    private SmallCubeSpawner spawner;

    [SetUp]
    public void SetUp()
    {
        GameObject spawnerObject = new GameObject();
        spawner = spawnerObject.AddComponent<SmallCubeSpawner>();
        spawner.jsonManager = spawnerObject.AddComponent<MockJsonManager>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(spawner.gameObject);
    }

    [UnityTest]
    public IEnumerator SpawnerCreatesCorrectNumberOfCubes()
    {
        PressableButton spawnButton = GameObject.Find("SpawnSmallCubes").GetComponent<PressableButton>();
        
        //invoke the OnClicked() event of the spawnButton component
        //need to invoke the event I guess
        spawnButton.
        
        yield return new WaitForSeconds(spawner.spawnRate * (spawner.maxAmountOfCubes + 1));
    }

}

public class MockJsonManager : JsonManager
{
    public float GetWaterConsumedForItem(string itemName)
    {
        return 100f;
    }
}

