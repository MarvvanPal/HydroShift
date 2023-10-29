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

    [TestCase(50f, 30, ExpectedResult = 5)]
    [TestCase(100f, 30, ExpectedResult = 10)]
    [TestCase(300f, 30, ExpectedResult = 30)]
    [TestCase(10, 30, ExpectedResult = 1)]
    [TestCase(0, 30, ExpectedResult = 0)]
    [TestCase(-10, 30, ExpectedResult = 0)]
    [TestCase(10000, 30, ExpectedResult = 30)]
    public int CalculateCubesToBeSpawned_ReturnsCorrectValue(float testVolume, int testMaxCubes)
    {
        return spawner.CalculateCubesToBeSpawned(testVolume, testMaxCubes);
    }
    
    
}
