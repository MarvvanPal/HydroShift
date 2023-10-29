using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SmallCubeSpawnerTests
{
    private SmallCubeSpawner spawner;
    private GameObject testObject;

    [SetUp]
    public void SetUp()
    {
        testObject = new GameObject();
        spawner = testObject.AddComponent<SmallCubeSpawner>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(testObject);
    }

    [Test]
    public void CalculateCubesToBeSpawned_ReturnsCorrectValue()
    {
        float testVolume = 50f;
        int testMaxCubes = 30;

        int result = spawner.CalculateCubesToBeSpawned(testVolume, testMaxCubes);
        
        Assert.AreEqual(4, result);
    }
}
