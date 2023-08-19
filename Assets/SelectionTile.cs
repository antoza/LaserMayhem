using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionTile : Tile
{

    public override void setColor()
    {
        GetComponent<SpriteRenderer>().color = Color.black;
    }
}
