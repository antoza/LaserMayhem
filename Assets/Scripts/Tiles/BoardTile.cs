using UnityEngine;

#nullable enable
public class BoardTile : Tile
{
    public int x, y;
    [field: SerializeField]
    private Sprite BlackSprite;
    [field: SerializeField]
    private Sprite WhiteSprite;

    public override void SetColor()
    {
        Debug.Log("I change colors");
        if ((x + y) % 2 == 0)
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
        return new Vector2Int(x - otherBoardTile.x, y - otherBoardTile.y);
    }

    public bool IsCloseEnoughFrom(BoardTile otherBoardTile, int maxDistance)
    {
        Vector2Int deltaPos = GetDeltaPosFrom(otherBoardTile);
        return Mathf.Abs(deltaPos.x) <= maxDistance && Mathf.Abs(deltaPos.y) <= maxDistance;
    }
}
