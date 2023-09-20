using System;
using System.IO;
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

    private void Awake()
    {
        imagePath = Application.dataPath + "/Pictures";
        fullPath = Path.Combine(imagePath, imageName);
    }

    void Start()
    {
        m_RuntimeModel = ModelLoader.Load(modelAsset);
        m_Worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, m_RuntimeModel);
        Tensor input = new Tensor(1, 320, 320, 3);
        
        if(!File.Exists(fullPath)) Debug.LogError("File not found!");
        byte[] imageBytes = File.ReadAllBytes(fullPath);
        
    }
}
