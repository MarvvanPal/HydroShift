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

    private readonly COCONames cocoNames = new ();

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

        // This is what the RecognizeObjects method in the YoloProcessor class does

        var outputTensor = await ForwardAsync(m_Worker, inputTensor);
        inputTensor.Dispose();
        Debug.LogError("Input Tensor has been disposed!");

        List<YoloItem> detectedObjects = GetYoloData(outputTensor, cocoNames, 0.65f, 0.00001f);

        foreach (YoloItem detectedObject in detectedObjects)
        {
            Console.WriteLine(detectedObject.MostLikelyObject);
        }
        Debug.LogError(outputTensor.shape);
        outputTensor.Dispose();
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

    private List<YoloItem> GetYoloData(Tensor tensor, COCONames cocoNames, float minProbability,
        float overlapThreshold)
    {
        float maxConfidence = 0;
        YoloItem maxConfidenceItem = null;
        var boxesMeetingConfidenceLevel = new List<YoloItem>();
        for (var i = 0; i < tensor.width; i++)
        {
            YoloItem yoloItem = new YoloItem(tensor, i, cocoNames);
            //maxConfidence = yoloItem.Confidence > maxConfidence ? yoloItem.Confidence : maxConfidence;
            if (yoloItem.Confidence > maxConfidence)
            {
                maxConfidence = yoloItem.Confidence;
                maxConfidenceItem = yoloItem;
            }
            if (yoloItem.Confidence > minProbability)
            {
                boxesMeetingConfidenceLevel.Add(yoloItem);
            }
        }
        Debug.LogError($"max confidence = {maxConfidence}");
        if (maxConfidenceItem != null)
        {
            Debug.LogError($"max confidence item = {maxConfidenceItem.MostLikelyObject}");
        }
        
        var result = new List<YoloItem>();
        var recognizedTypes = boxesMeetingConfidenceLevel.Select(b => b.MostLikelyObject).Distinct();
        foreach (string objType in recognizedTypes)
        {
            var boxesOfThisType = boxesMeetingConfidenceLevel.Where(b => b.MostLikelyObject == objType).ToList();
            result.AddRange(RemoveOverlappingBoxes(boxesOfThisType, overlapThreshold));
        }
        
        tensor.Dispose();
        
        Debug.LogError($"Boxes Meeting confidence level: {boxesMeetingConfidenceLevel.Count}");
        
        return result;
        
    }

    private static List<YoloItem> RemoveOverlappingBoxes(List<YoloItem> boxesMeetingConfidenceLevel, float overlapThreshold)
    {
        boxesMeetingConfidenceLevel.Sort((a,b) => b.Confidence.CompareTo(a.Confidence));
        var selectedBoxes = new List<YoloItem>();
        Debug.LogError($"Boxes meeting confidence level: {boxesMeetingConfidenceLevel.Count}");
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

    public class YoloItem
    {
        public Vector2 Center { get; }
        public Vector2 Size { get; }
        public Vector2 TopLeft { get; }
        public Vector2 BottomRight { get; }
        public float Confidence { get; }
        public string MostLikelyObject { get; }

        public YoloItem (Tensor tensorData, int boxIndex, COCONames cocoNames)
        {
            Center = new Vector2(tensorData[0, 0, boxIndex, 0], tensorData[0, 0, boxIndex, 1]);
            Size = new Vector2(tensorData[0, 0, boxIndex, 2], tensorData[0, 0, boxIndex, 3]);
            TopLeft = Center - Size / 2;
            BottomRight = Center + Size / 2;
            Confidence = tensorData[0, 0, boxIndex, 4];

            var classProbabilities = new List<float>();
            for (var i = 5; i < tensorData.channels; i++)
            {
                classProbabilities.Add(tensorData[0, 0, boxIndex, i]);
            }

            var maxIndex = classProbabilities.Any() ? classProbabilities.IndexOf(classProbabilities.Max()) : 0;
            MostLikelyObject = cocoNames.GetName(maxIndex);
        }
    }

    public class COCONames
    {
        public string GetName(int mapIndex)
        {
            return detectableObjects[mapIndex];
        }
        
        private readonly List<string> detectableObjects = new()
        {
            "person",
            "bicycle",
            "car",
            "motorcycle",
            "airplane",
            "bus",
            "train",
            "truck",
            "boat",
            "traffic light",
            "fire hydrant",
            "stop sign",
            "parking meter",
            "bench",
            "bird",
            "cat",
            "dog",
            "horse",
            "sheep",
            "cow",
            "elephant",
            "bear",
            "zebra",
            "giraffe",
            "backpack",
            "umbrella",
            "handbag",
            "tie",
            "suitcase",
            "frisbee",
            "skis",
            "snowboard",
            "sports ball",
            "kite",
            "baseball bat",
            "baseball glove",
            "skateboard",
            "surfboard",
            "tennis racket",
            "bottle",
            "wine glass",
            "cup",
            "fork",
            "knife",
            "spoon",
            "bowl",
            "banana",
            "apple",
            "sandwich",
            "orange",
            "broccoli",
            "carrot",
            "hot dog",
            "pizza",
            "donut",
            "cake",
            "chair",
            "couch",
            "potted plant",
            "bed",
            "dining table",
            "toilet",
            "tv",
            "laptop",
            "mouse",
            "remote",
            "keyboard",
            "cell phone",
            "microwave",
            "oven",
            "toaster",
            "sink",
            "refrigerator",
            "book",
            "clock",
            "vase",
            "scissors",
            "teddy bear",
            "hair drier",
            "toothbrush"
        };
    }
}
