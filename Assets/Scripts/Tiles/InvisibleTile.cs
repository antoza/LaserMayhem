using UnityEngine;

#nullable enable
public class InvisibleTile : Tile
{
    public int x, y;

    protected override bool VerifyOnMouseButtonDown()
    {
        return false;
    }

    protected override void DoOnMouseButtonDown()
    {
    }

    protected override void DoOnMouseButtonUp()
    {
    }
}
