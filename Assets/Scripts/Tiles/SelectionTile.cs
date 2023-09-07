using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionTile : Tile
{

    public override void SetColor()
    {
        GetComponent<SpriteRenderer>().color = Color.black;
    }
}
