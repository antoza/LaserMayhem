using UnityEngine;

#nullable enable
public class InfiniteTile : Tile
{
    public override void setColor()
    {
        GetComponent<SpriteRenderer>().color = Color.yellow;
    }
}
