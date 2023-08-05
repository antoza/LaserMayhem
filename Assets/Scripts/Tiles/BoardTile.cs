using UnityEngine;

#nullable enable
public class BoardTile : Tile
{
    public int x, y;

    public override void setColor()
    {
        if ((x + y) % 2 == 0)
        {
            GetComponent<SpriteRenderer>().color = Color.black;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.grey;
        }

        m_TileState = TileState.Taken;
    }
}
