using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.TestTools;

public class YoloItemTests
{
    //private static COCONames cocoNames;

    [SetUp]
    public void SetUp()
    {
        
    }

    public static IEnumerable<TestCaseData> YoloItemTestCaseData()
    {
        COCONames cocoNames = new COCONames();
        int [] classIndices = { 5, 6, 10, 18, 83 };
        float[] confidences = {0.7f, 0.75f, 0.8f, 0.85f, 0.95f };
        Vector2[] centerCoordinates = { new Vector2(100, 100), new Vector2(150, 150), new Vector2(200, 200), new Vector2(250, 250), new Vector2(300, 300) };

        for (int i = 0; i < 5; i++)
        {
            float[] tensorData = new float[1 * 1 * 1 * 84];
            int index = 0;
            
            tensorData[index++] = centerCoordinates[i].x;
            tensorData[index++] = centerCoordinates[i].y;
            tensorData[index++] = 20; // Width
            tensorData[index++] = 20; // Height
            tensorData[index++] = confidences[i];
            tensorData[index + (classIndices[i] - 5)] = 1.0f;

            TensorShape tensorShape = new TensorShape(1, 1, 1, 84);
            Tensor tensor = new Tensor(tensorShape, tensorData);
            
            yield return new TestCaseData(tensor, centerCoordinates[i], new Vector2(20, 20), confidences[i], cocoNames.GetName(classIndices[i] - 5)).SetName($"TestYoloItemWith '{cocoNames.GetName(classIndices[i] - 5)}'");
        }
        
    }

    [TestCaseSource(nameof(YoloItemTestCaseData))]
    public void Constructor_SetsPropertiesCorrectly(Tensor tensor, Vector2 expectedCenter, Vector2 expectedSize,
        float expectedConfidence, string expectedObject)
    {
        COCONames cocoNames = new COCONames();
        YoloItem yoloItem = new YoloItem(tensor, 0, cocoNames);
        
        Assert.AreEqual(expectedCenter, yoloItem.Center);
        Assert.AreEqual(expectedSize, yoloItem.Size);
        Assert.AreEqual(expectedConfidence, yoloItem.Confidence);
        Assert.AreEqual(expectedObject, yoloItem.MostLikelyObject);
        
        tensor.Dispose();
    }
}
