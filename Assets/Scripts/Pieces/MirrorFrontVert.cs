    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorFrontVert : Piece
{
    public static MirrorFrontVert CreateInstance()
    {
        var instance = GetInstance<MirrorFrontVert>();
        return instance;
    }

    public override IEnumerable<Vector2Int> ComputeNewDirections(Vector2Int sourceDirection)
    {
        yield return new Vector2Int(-sourceDirection[0], sourceDirection[1]);
    }
}
