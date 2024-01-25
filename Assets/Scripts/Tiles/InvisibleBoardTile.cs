using UnityEngine;

#nullable enable
public class InvisibleBoardTile : BoardTile
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
