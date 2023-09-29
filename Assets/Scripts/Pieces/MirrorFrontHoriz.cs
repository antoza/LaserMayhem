    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorFrontHoriz : Piece
{
    public static MirrorFrontHoriz CreateInstance()
    {
        var instance = GetInstance<MirrorFrontHoriz>();
        return instance;
    }

    public override IEnumerable<Vector2Int> ComputeNewDirections(Vector2Int sourceDirection)
    {
        yield return new Vector2Int(sourceDirection[0], -sourceDirection[1]);
    }
}
