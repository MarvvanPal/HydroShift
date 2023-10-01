using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageCropper : MonoBehaviour
{
    private string imagePath;
    private readonly string imageName = "screenshot.png";
    private readonly int cropSize = 416;
    private string fullPath;

    private void Awake()
    {
        imagePath = Application.dataPath + "/Pictures";
        fullPath = Path.Combine(imagePath, imageName);
    }

    void Update()
    {
        if (Input.GetKeyDown("l"))
        {
            StartCoroutine(CropImage());
        }
    }

    private IEnumerator CropImage()
    {
        if (!File.Exists(fullPath)) Debug.LogError("File does not exist!");
        {
            byte[] imageBytes = File.ReadAllBytes(fullPath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);

            int startX = (texture.width - cropSize) / 2;
            int startY = (texture.height - cropSize) / 2;

            Color[] pixels = texture.GetPixels(startX, startY, cropSize, cropSize);
            Texture2D croppedTexture = new Texture2D(cropSize, cropSize);
            croppedTexture.SetPixels(pixels);
            croppedTexture.Apply();

            byte[] bytes = croppedTexture.EncodeToPNG();
            File.WriteAllBytes(Path.Combine(imagePath, "cropped_" + imageName), bytes);
            
            Debug.Log("Image cropped and saved successfully!");

        }
        
        yield return new WaitForSeconds(1f);
    }
}