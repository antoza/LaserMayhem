using UnityEngine;

#nullable enable
public class BoardTile : Tile
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

    public Vector2Int GetDeltaPosFrom(BoardTile otherBoardTile)
    {
        return new Vector2Int(Spot.x - otherBoardTile.Spot.x, Spot.y - otherBoardTile.Spot.y);
    }

    public bool IsCloseEnoughFrom(BoardTile otherBoardTile, int maxDistance)
    {
        Vector2Int deltaPos = GetDeltaPosFrom(otherBoardTile);
        return Mathf.Abs(deltaPos.x) <= maxDistance && Mathf.Abs(deltaPos.y) <= maxDistance;
    }
}
