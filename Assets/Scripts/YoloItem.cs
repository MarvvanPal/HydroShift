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

        var classProbabilities = new List<float>();
        for (var i = 5; i < tensorData.channels; i++)
        {
            classProbabilities.Add(tensorData[0, 0, boxIndex, i]);
        }

        var maxIndex = classProbabilities.Any() ? classProbabilities.IndexOf(classProbabilities.Max()) : 0;
        MostLikelyObject = cocoNames.GetName(maxIndex);
    }
}
