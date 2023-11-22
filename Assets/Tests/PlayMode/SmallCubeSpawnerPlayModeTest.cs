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
    private readonly string smallCubePrefabTag = "smallWaterCube";

    [SetUp]
    public void SetUp()
    {
        GameObject spawnerObject = new GameObject();
        spawner = spawnerObject.AddComponent<SmallCubeSpawner>();
        Object.DestroyImmediate(spawner.jsonManager);

        testCubePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/smallWaterCube.prefab");
        spawner.cubePrefab = testCubePrefab;
    }

    [TearDown]
    public void TearDown()
    {
        CleanUpGameObjectsWithTag(smallCubePrefabTag);
        Object.Destroy(spawner.gameObject);
    }

    [UnityTest]
    public IEnumerator SpawnerCreatesCorrectNumberOfCubes(
        [ValueSource(nameof(SpawnSmallCubeCases))] TestCaseData testData)
    {
        spawner.volume = (float)testData.Arguments[0];
        int expectedCubes = (int)testData.Arguments[1];

        spawner.SpawnSmallCubes();
        
        yield return new WaitForSeconds(spawner.spawnRate * (expectedCubes + 2));

        int spawnedCubes = GameObject.FindGameObjectsWithTag(smallCubePrefabTag).Length;
        Assert.AreEqual(expectedCubes, spawnedCubes);
    }

    private static void CleanUpGameObjectsWithTag(string tag)
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject cube in cubes)
        {
            Object.Destroy(cube);
        }
    }
    
    private static IEnumerable<TestCaseData> SpawnSmallCubeCases()
    {
        yield return new TestCaseData(1700f, 30).SetName("SpawnSmallCubes_Volume1700_Expect17");
        yield return new TestCaseData(200f, 20).SetName("SpawnSmallCubes_Volume200_Expect20");
        yield return new TestCaseData(50f, 5).SetName("SpawnSmallCubes_Volume50_Expect5");
    }
}



