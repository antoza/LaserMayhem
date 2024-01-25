using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class NormalBoardTile : BoardTile
{
    [field: SerializeField]
    private Sprite BlackSprite;
    [field: SerializeField]
    private Sprite WhiteSprite;

    public override void SetColor()
    {
        if ((Spot.x + Spot.y) % 2 == 0)
        {
            GetComponent<SpriteRenderer>().sprite = WhiteSprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = BlackSprite;
        }
    }

    public Vector2Int GetDeltaPosFrom(NormalBoardTile otherBoardTile)
    {
        return new Vector2Int(Spot.x - otherBoardTile.Spot.x, Spot.y - otherBoardTile.Spot.y);
    }

    public bool IsCloseEnoughFrom(NormalBoardTile otherBoardTile, int maxDistance)
    {
        Vector2Int deltaPos = GetDeltaPosFrom(otherBoardTile);
        return Mathf.Abs(deltaPos.x) <= maxDistance && Mathf.Abs(deltaPos.y) <= maxDistance;
    }
}