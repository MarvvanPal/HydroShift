using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

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

    [TestCase(50f, ExpectedResult = 5)]
    [TestCase(100f, ExpectedResult = 10)]
    [TestCase(300f,ExpectedResult = 30)]
    [TestCase(10,ExpectedResult = 1)]
    [TestCase(0, ExpectedResult = 0)]
    [TestCase(-10, ExpectedResult = 0)]
    [TestCase(10000, ExpectedResult = 30)]
    public int CalculateCubesToBeSpawned_ReturnsCorrectValue(float testVolume)
    {
        return spawner.CalculateCubesToBeSpawned(testVolume);
    }
    
    [TestCaseSource(nameof(GetSmallCubeDimensionsTestCases))]
    public (float, float, float) GetSmallCubeDimensions_ReturnsCorrectDimensions(float volume, int testAmountOfCubesToBeSpawned)
    {
        Vector3 result = spawner.GetSmallCubeDimensions(volume, testAmountOfCubesToBeSpawned);
        return (result.x, result.y, result.z);
    }
    
    public static IEnumerable<TestCaseData> GetSmallCubeDimensionsTestCases()
    {
        yield return new TestCaseData(50f, 5).Returns((0.22f, 0.22f, 0.22f));
        yield return new TestCaseData(2500f, 30).Returns((0.44f, 0.44f, 0.44f));
        yield return new TestCaseData(15000f, 30).Returns((0.79f, 0.79f, 0.79f));
        yield return new TestCaseData(1000f, 30).Returns((0.32f, 0.32f, 0.32f));
        yield return new TestCaseData(150f, 30).Returns((0.17f, 0.17f, 0.17f));


    }
}
