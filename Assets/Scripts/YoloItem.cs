using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Barracuda;
using UnityEngine;



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
        
        float maxProbability = float.MinValue;
        int maxIndex = 0;
        for (int i = 5; i < tensorData.channels; i++)
        {
            float probability = tensorData[0, 0, boxIndex, i];
            if (!(probability > maxProbability)) continue;
            maxProbability = probability;
            maxIndex = i - 5;
        }
        
        MostLikelyObject = cocoNames.GetName(maxIndex);
        
    }
}
