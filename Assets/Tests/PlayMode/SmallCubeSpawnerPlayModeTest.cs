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
    private GameObject testCubePrefab;
    private string smallCubePrefabTag;

    [SetUp]
    public void SetUp()
    {
        GameObject spawnerObject = new GameObject();
        spawner = spawnerObject.AddComponent<SmallCubeSpawner>();
        Object.Destroy(spawner.jsonManager);

        testCubePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/smallWaterCube.prefab");
        smallCubePrefabTag = "smallWaterCube";
        spawner.cubePrefab = testCubePrefab;
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(spawner.gameObject);
    }

    [UnityTest]
    public IEnumerator SpawnerCreatesCorrectNumberOfCubes(
        [ValueSource(nameof(SpawnSmallCubeCases))] TestCaseData testData)
    {
        float volume = (float)testData.Arguments[0];
        int expectedCubes = (int)testData.Arguments[1];

        spawner.volume = volume;

        spawner.SpawnSmallCubes();
        
        yield return new WaitForSeconds(spawner.spawnRate * (spawner.maxAmountOfCubes + 1));

        int spawnedCubes = GameObject.FindGameObjectsWithTag(smallCubePrefabTag).Length;
        Assert.AreEqual(expectedCubes, spawnedCubes);
        
        yield return CleanUpCubes(smallCubePrefabTag);
    }

    private static IEnumerator CleanUpCubes(string tag)
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag(tag);

        for (int i = 0; i < cubes.Length; i++)
        {
            Object.Destroy(cubes[i]);
        }

        yield return new WaitForEndOfFrame();
    }
    
    
    private static IEnumerable<TestCaseData> SpawnSmallCubeCases()
    {
        yield return new TestCaseData(1700f, 30).SetName("SpawnSmallCubes_Volume1700_Expect17");
        yield return new TestCaseData(200f, 20).SetName("SpawnSmallCubes_Volume1000_Expect10");
        yield return new TestCaseData(50f, 5).SetName("SpawnSmallCubes_Volume1700_Expect17");
    }

}



