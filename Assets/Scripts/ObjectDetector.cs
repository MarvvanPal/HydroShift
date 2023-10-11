using System;
using System.Collections;
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

    private Model m_RuntimeModel;
    private IWorker m_Worker;

    private COCONames cocoNamesList = new ();

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
        Texture2D inputImage = new Texture2D(width:416, height:416);
        inputImage.LoadImage(imageBytes);

        // Normalization and input tensor construction:

        Color32[] picture = inputImage.GetPixels32();

        float[] floatValues = new float[inputImage.width * inputImage.height * 3];

        int width = inputImage.width;
        int height = inputImage.height;

        for (int c = 0; c < 3; c++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color color = picture[y * width + x];
                    int index = c * width * height + y * width + x;

                    if (c == 0) floatValues[index] = color.r / 255.0f;
                    if (c == 1) floatValues[index] = color.g / 255.0f;
                    if (c == 2) floatValues[index] = color.b / 255.0f;
                }
            }
        }

        Tensor inputTensor = new Tensor(1, height, width, 3, floatValues);
        await Task.Delay(32);
        
        Debug.Log(inputTensor.shape);
        
        // This is what the RecognizeObjects method in the YoloProcessor class does
        var outputTensor = await ForwardAsync(m_Worker, inputTensor);
        inputTensor.Dispose();

        List<YoloItem> detectedObjects = GetYoloData(outputTensor, cocoNamesList, 0.65f, 0.3f);

        foreach (YoloItem detectedObject in detectedObjects)
        {
            Console.WriteLine(detectedObject.MostLikelyObject);
        }
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

    private List<YoloItem> GetYoloData(Tensor tensor, COCONames cocoNamesList, float minProbability,
        float overlapThreshold)
    {
        float maxConfidence = 0;
        YoloItem maxConfidenceItem = null;
        var boxesMeetingConfidenceLevel = new List<YoloItem>();
        for (var i = 0; i < tensor.width; i++)
        {
            YoloItem yoloItem = new YoloItem(tensor, i, cocoNamesList);
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
        
        return result;
    }

    private static List<YoloItem> RemoveOverlappingBoxes(List<YoloItem> boxesMeetingConfidenceLevel,
        float overlapThreshold)
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
            MostLikelyObject = cocoNames.Map[maxIndex];
        }
    }

    public class COCONames
    {
        public readonly List<string> Map = new()
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
