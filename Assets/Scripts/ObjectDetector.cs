using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.MixedReality.Toolkit.SpatialManipulation;
using Unity.Barracuda;
using UnityEngine;
// using HoloLensCameraStream;

public class ObjectDetector : MonoBehaviour
{

    public NNModel modelAsset;

    private string imagePath;
    private readonly string imageName = "cropped_screenshot.png";
    private string fullPath;

    private Model m_RuntimeModel;
    private IWorker m_Worker;

    private COCONames cocoNames = new ();

    private void Awake()
    {
        imagePath = Application.dataPath + "/Pictures";
        fullPath = Path.Combine(imagePath, imageName);
    }

    void Start()
    {
        m_RuntimeModel = ModelLoader.Load(modelAsset);
        m_Worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, m_RuntimeModel);
        
        DetectObject();
        
    }
    private void DetectObject()
    {
        if (!File.Exists(fullPath)) Debug.LogError("File not found!");
        byte[] imageBytes = File.ReadAllBytes(fullPath);
        Texture2D inputImage = new Texture2D(2, 2);
        inputImage.LoadImage(imageBytes);

        // Normalization and input tensor construction:

        Color32[] picture = inputImage.GetPixels32();

        float[] floatValues = new float[inputImage.width * inputImage.height * 3];

        for (int i = 0; i < inputImage.height; i++)
        {
            for (int j = 0; j < inputImage.width; j++)
            {
                Color color = picture[i * inputImage.width + j];

                floatValues[(i * inputImage.width + j) * 3 + 0] = color.r / 255.0f;
                floatValues[(i * inputImage.width + j) * 3 + 1] = color.g / 255.0f;
                floatValues[(i * inputImage.width + j) * 3 + 2] = color.b / 255.0f;
            }
        }

        Tensor inputTensor = new Tensor(1, inputImage.height, inputImage.width, 3, floatValues);

        // Inference
        m_Worker.Execute(inputTensor);

        var output = m_Worker.PeekOutput();

        List<string> detectedClasses = new List<string>();
        float confidenceThreshold = 0.65f;
        int numberOfClasses = cocoNames.Map.Count;

        for (int i = 0; i < output.shape.width; i++)
        {
            // it does not change!!! 
            int classIdx = 0;
            float maxConfidence = output[0, 0, i, 4];
            Debug.Log(maxConfidence);
            for (int j = 0; j < numberOfClasses; ++j)
            {
                if (output[0, 0, i, 4 + j] > maxConfidence)
                {
                    maxConfidence = output[0, 0, i, 4 + j];
                    classIdx = j;
                }
            }

            
            if (maxConfidence < confidenceThreshold)
            {
                continue;
            }
            

            Debug.Log("Class Index: " + classIdx);
            string labelName = cocoNames.Map[classIdx];
            detectedClasses.Add(labelName);
        }

        foreach (string detectedClass in detectedClasses)
        {
            Debug.Log(detectedClass);
        }
        
        inputTensor.Dispose();
        output.Dispose();
        m_Worker.Dispose();
    }

    public class COCONames
    {
        public List<String> Map = new()
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
