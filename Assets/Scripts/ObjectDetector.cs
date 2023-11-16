using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Unity.Barracuda;
using UnityEngine;

// using HoloLensCameraStream;

public class ObjectDetector : MonoBehaviour
{

    public NNModel modelAsset;

    private string imagePath;
    // "cropped_screenshot.png"
    private readonly string imageName = "cropped_screenshot.png";
    private string fullPath;
    
    // private readonly Vector2Int yoloImageSize = new Vector2Int(416, 416);

    private Model m_RuntimeModel;
    private IWorker m_Worker;

    private static readonly COCONames CocoNamesList = new();

    private void Awake()
    {
        imagePath = Application.dataPath + "/Pictures/";
        fullPath = imagePath + imageName;
    }

    void Start()
    {
        Debug.Log(fullPath);
        m_RuntimeModel = ModelLoader.Load(modelAsset);
        m_Worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, m_RuntimeModel/*, verbose:true*/);
    }

    private void Update()
    {
        if (Input.GetKeyDown("j"))
        {
            Debug.Log("j key pressed!");
            DetectObjects();
        }
    }

    private async Task DetectObjects()
    {
        
        if (!File.Exists(fullPath)) Debug.LogError("File not found!");
        byte[] imageBytes = await File.ReadAllBytesAsync(fullPath);
        Texture2D inputImage = new Texture2D(width:2, height:2);
        inputImage.LoadImage(imageBytes);

        RenderTexture renderTexture = new RenderTexture(416, 416, 24);
        Graphics.Blit(inputImage,renderTexture);
        await Task.Delay(32);
        Texture2D textureFromRender = ToTexture2D(renderTexture);

        Tensor inputTensor = new Tensor(textureFromRender, 3);
        await Task.Delay(32);
        
        Debug.Log(inputTensor.shape);

        var outputTensor = await ForwardAsync(m_Worker, inputTensor);
        inputTensor.Dispose();
        Debug.LogError("Input Tensor has been disposed!");

        List<YoloItem> detectedObjects = GetYoloData(outputTensor, 0.65f, 0.5f);

        foreach (YoloItem detectedObject in detectedObjects)
        {
            Debug.Log($"class: {detectedObject.MostLikelyObject} -- confidence:{detectedObject.Confidence}");
        }
        
        outputTensor.Dispose();
        m_Worker.Dispose();
        Debug.LogError("Output Tensor has been disposed!");

    }

    // https://stackoverflow.com/questions/44264468/convert-rendertexture-to-texture2d
    private Texture2D ToTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();

        return tex;
    }
    
    // run the inference
    private async Task<Tensor> ForwardAsync(IWorker modelWorker, Tensor inputTensor)
    {
        var worker = m_Worker.StartManualSchedule(inputTensor);
        var randNumber = 0;
        bool hasMoreWork;
        do
        {
            hasMoreWork = worker.MoveNext();
            if (++randNumber % 20 == 0)
            {
                m_Worker.FlushSchedule();
                await Task.Delay(32);
            }
        } while (hasMoreWork);

        return modelWorker.PeekOutput();
        
    }

    private List<YoloItem> GetYoloData(Tensor outPutTensor, float minConfidence, float overlapThreshold)
    {
        List<YoloItem> allYoloItems = ExtractYoloItemsFromTensor(outPutTensor);
        List<YoloItem> yoloItemsMeetingConfidenceLevel = FilterItemsByConfidence(allYoloItems, minConfidence);
        List<YoloItem> finalYoloItems = NonMaximumSuppression(yoloItemsMeetingConfidenceLevel, overlapThreshold);
        
        return finalYoloItems;
        
    }

    private List<YoloItem> ExtractYoloItemsFromTensor(Tensor tensor)
    {
        List<YoloItem> yoloItems = new List<YoloItem>();
        for (int i = 0; i < tensor.width; i++)
        {
            YoloItem yoloItem = new YoloItem(tensor, i, CocoNamesList);
            yoloItems.Add(yoloItem);
            
        }
        
        return yoloItems;
    }

    private List<YoloItem> FilterItemsByConfidence(List<YoloItem> yoloItems, float minConfidence)
    {
        List<YoloItem> yoloItemsMeetingConfidenceLevel = new();
        foreach (YoloItem yoloItem in yoloItems)
        {
            if (yoloItem.Confidence > minConfidence)
            {
                yoloItemsMeetingConfidenceLevel.Add(yoloItem);
            }
        }
        return yoloItemsMeetingConfidenceLevel;
    }

    private List<YoloItem> NonMaximumSuppression(List<YoloItem> yoloItems, float overlapThreshold)
    {
        List<YoloItem> suppressedYoloItems = new List<YoloItem>();
        List<YoloItem> yoloItemsSortedByConfidence =
            yoloItems.OrderByDescending(yoloItem => yoloItem.Confidence).ToList();
        while (yoloItemsSortedByConfidence.Any())
        {
            YoloItem yoloItemWithHighestConfidence = yoloItemsSortedByConfidence.First();
            suppressedYoloItems.Add(yoloItemWithHighestConfidence);
            yoloItemsSortedByConfidence.RemoveAt(0);

            yoloItemsSortedByConfidence = yoloItemsSortedByConfidence
                .Where(yoloItem => ComputeIoU(yoloItemWithHighestConfidence, yoloItem) < overlapThreshold).ToList();
        }

        return suppressedYoloItems;
    }

    private static List<YoloItem> RemoveOverlappingBoxes(List<YoloItem> boxesMeetingConfidenceLevel, float overlapThreshold)
    {
        boxesMeetingConfidenceLevel.Sort((a,b) => b.Confidence.CompareTo(a.Confidence));
        var selectedBoxes = new List<YoloItem>();
        while (boxesMeetingConfidenceLevel.Count > 0)
        {
            var currentBox = boxesMeetingConfidenceLevel[0];
            selectedBoxes.Add(currentBox);
            boxesMeetingConfidenceLevel.RemoveAt(0);

            for (var i = 0; i < boxesMeetingConfidenceLevel.Count; i++)
            {
                YoloItem otherBox = boxesMeetingConfidenceLevel[i];
                float overlap = ComputeIoU(currentBox, otherBox);
                if (overlap > overlapThreshold)
                {
                    boxesMeetingConfidenceLevel.RemoveAt(i);
                    i--;
                }
            }
        }

        return selectedBoxes;
    }

    private static float ComputeIoU(YoloItem boxA, YoloItem boxB)
    {
        float xA = Math.Max(boxA.TopLeft.x, boxB.TopLeft.y);
        float yA = Math.Max(boxA.TopLeft.y, boxA.TopLeft.y);
        float xB = Math.Min(boxA.BottomRight.x, boxB.BottomRight.x);
        float yB = Math.Min(boxA.BottomRight.y, boxB.BottomRight.y);
        float intersectionArea = Math.Max(0, xB - xA + 1) * Math.Max(0, yB - yA + 1);
        float boxAArea = boxA.Size.x * boxA.Size.y;
        float boxBArea = boxB.Size.y * boxB.Size.y;
        float unionArea = boxAArea + boxBArea - intersectionArea;

        return intersectionArea / unionArea;

    }
}
