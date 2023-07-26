using System;
using System.Collections.Generic;

[Serializable]
public class APIGroceryResponse
{
    public List<GroceryItem> groceryItems;
}
[Serializable]
public class GroceryItem
{
    public string name = String.Empty;
    public GroceryItemDetails details;
}

[Serializable]
public sealed class GroceryItemDetails
{
    public long ean = 0;
    public string category = String.Empty;
    public float waterConsumedPerPiece = 0f;
    public float massPerPieceInGram = 0f;
}