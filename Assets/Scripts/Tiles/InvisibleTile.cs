using UnityEngine;

#nullable enable
public class InvisibleTile : Tile
{
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
