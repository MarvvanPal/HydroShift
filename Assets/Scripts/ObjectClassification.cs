using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectClassification : MonoBehaviour
{

    public NNModel model;
    public TextAsset labelTextFile;
    private string imagePath;
    private readonly string imageName = "cropped_screenshot.png";
    private string fullPath;

    private Model runTimeModel;
    private IWorker worker;

    private List<string> labels;

    private void Awake()
    {
        imagePath = Application.dataPath + "/Pictures/";
        fullPath = imagePath + imageName;
    }

    void Start()
    {
        Debug.Log(fullPath);
        runTimeModel = ModelLoader.Load(model);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, runTimeModel);
        LoadLabels();
    }

    void Update()
    {
        if (Input.GetKeyDown("c"))
        {
            Debug.Log("c key was pressed");
            ClassifyImage();
        }
    }

    private void LoadLabels()
    {
        string stringsFromTheTextFile = labelTextFile.text;
        labels = ParseClasses(stringsFromTheTextFile);
        for(int i = 0; i < 10; i++)
        {
            Debug.Log(labels[i]);
        }
    }

    private List<string> ParseClasses(string input)
    {
        List<string> classList = new List<string>();
        MatchCollection matches = Regex.Matches(input, @"'([^']+)'");

        foreach (Match match in matches)
        {
            classList.Add(match.Groups[1].Value);
        }

        return classList;
    }

    private async Task ClassifyImage()
    {
        if (!File.Exists(fullPath)) Debug.LogError("File not found!");
        byte[] imageBytes = await File.ReadAllBytesAsync(fullPath);
        Texture2D inputImage = new Texture2D(width:2, height:2);
        inputImage.LoadImage(imageBytes);

        RenderTexture renderTexture = new RenderTexture(416, 416, 24);
        Graphics.Blit(inputImage,renderTexture);
        await Task.Delay(32);
        Texture2D textureFromRender = ToTexture2D(renderTexture);
        
        // input tensor construction
        Tensor inputTensor = new Tensor(textureFromRender, 3);
        await Task.Delay(32);
        Debug.Log(inputTensor.shape);
        Debug.Log($"label count: {labels.Count()}");
        worker.Execute(inputTensor);
        Tensor outputTensor = worker.PeekOutput();
        Debug.Log(outputTensor.shape);
        // shape of the output: (n:1, h:1, w:1, c:1000)

        for (int i = 0; i < 100; i++)
        {
            //Debug.Log(outputTensor[i, 0, 0, 0]);
            //Debug.Log(outputTensor[0, i, 0, 0]);
            //Debug.Log(outputTensor[0, 0, i, 0]);
            //Debug.Log(outputTensor[0, 0, 0, i]);
            
        }

        float[] probabilities = outputTensor.ToReadOnlyArray();
        await Task.Delay(32);
        

        int maxIndex = 0;
        float maxProb = probabilities[0];
        for (int i = 1; i < probabilities.Length; i++)
        {
            if (probabilities[i] > maxProb)
            {
                maxProb = probabilities[i];
                maxIndex = i;
            }
        }
        
        Debug.Log($"Class: {labels[maxIndex]}, Probability: {maxProb}");

        outputTensor.Dispose();

    }
    
    private Texture2D ToTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();

        return tex;
    }
    
}
