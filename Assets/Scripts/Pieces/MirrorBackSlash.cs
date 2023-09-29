using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBackSlash : Piece
{
    public static MirrorBackSlash CreateInstance()
    {
        var instance = GetInstance<MirrorBackSlash>();
        return instance;
    }

    public override IEnumerable<Vector2Int> ComputeNewDirections(Vector2Int sourceDirection)
    {
        yield return new Vector2Int(-sourceDirection[1], -sourceDirection[0]);
    }
}
