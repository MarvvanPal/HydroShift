using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
// 218891
public class ObjectDetectorTests
{
    private ObjectDetector objectDetector;
    private GameObject detectorObject;
    private string mockImagePath;
    

    [SetUp]
    public void SetUp()
    {
        detectorObject = new GameObject();
        objectDetector = detectorObject.AddComponent<ObjectDetector>();
        mockImagePath = "Assets/Tests/MockData/mock_image.png";
    }

    [UnityTest]
    public IEnumerator LoadImage_ReturnsCorrectByteArrayLength()
    {
        long expectedLength = 218891;

        var completionSource = new TaskCompletionSource<byte[]>();
        _ = LoadImageAsync(completionSource, mockImagePath);

        while (!completionSource.Task.IsCompleted)
        {
            yield return null;
        }

        byte[] result = completionSource.Task.Result;
    
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedLength, result.Length);
    }
    
    private async Task LoadImageAsync(TaskCompletionSource<byte[]> completionSource, string path)
    {
        byte[] result = await objectDetector.LoadImage(path);
        completionSource.SetResult(result);
    }

    [UnityTest]
    public IEnumerator LoadImage_ReturnsNullForNonExistentFile()
    {
        var completionSource = new TaskCompletionSource<byte[]>();
        _ = LoadImageAsync(completionSource, "non_existent_file.png");

        while (!completionSource.Task.IsCompleted)
        {
            yield return null;
        }

        byte[] result = completionSource.Task.Result;

        Assert.IsNull(result);
        
        LogAssert.Expect(LogType.Error, "Image not found");

        yield return null;
    }
/*
    [TearDown]
    public void TearDown()
    {
        Object.Destroy(detectorObject);
    }
    */
}
