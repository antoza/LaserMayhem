using UnityEngine;

#nullable enable
public class BoardTile : Tile
{
    public int x, y;

    public override void SetColor()
    {
        Debug.Log("I change colors");
        if ((x + y) % 2 == 0)
        {
            GetComponent<SpriteRenderer>().color = new Color(145, 110, 70);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(219, 221, 196);
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
