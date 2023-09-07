using UnityEngine;

#nullable enable
public class InfiniteTile : Tile
{
    public override void SetColor()
    {
        GetComponent<SpriteRenderer>().color = Color.yellow;
    }
}
