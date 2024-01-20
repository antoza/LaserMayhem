using UnityEngine;

#nullable enable
public class InvisibleTile : Tile
{
    public int x, y;

#if !DEDICATED_SERVER
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
#endif
}
