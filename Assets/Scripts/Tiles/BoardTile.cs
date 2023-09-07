using UnityEngine;

#nullable enable
public class BoardTile : Tile
{
    public int x, y;

    public override void SetColor()
    {
        if ((x + y) % 2 == 0)
        {
            GetComponent<SpriteRenderer>().color = Color.black;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.grey;
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
