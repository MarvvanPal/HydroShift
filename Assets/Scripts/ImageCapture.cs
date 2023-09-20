using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageCapture : MonoBehaviour
{
    private string fileSavePath;

    private void Awake()
    {
        fileSavePath = Application.dataPath + "/Pictures";
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            StartCoroutine(TakePicture());
        }
        
    }

    private IEnumerator TakePicture()
    {
        ScreenCapture.CaptureScreenshot($"{fileSavePath}/screenshot.png");
        Debug.Log("A screenshot was taken!");
        yield return new WaitForSeconds(1f);
    }
}
