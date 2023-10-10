using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectClassification : MonoBehaviour
{

    public NNModel model;
    private string imagePath;
    private readonly string imageName = "cropped_screenshot.png";
    private string fullPath;

    private Model runTimeModel;
    private IWorker worker;
    
    // TODO: get the class names from the dataset

    private void Awake()
    {
        imagePath = Application.dataPath + "/Pictures";
        fullPath = imagePath + imageName;
    }

    void Start()
    {
        Debug.Log(fullPath);
        runTimeModel = ModelLoader.Load(model);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, runTimeModel);
    }

    void Update()
    {
        
    }
    
    // method to normalize the image
    
    /*
    private Tensor constructInputTensor(string imagePath)
    {
        
        ;
    }
    */
}
