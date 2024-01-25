using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class MirrorDivider : Mirror
{
    public override IEnumerable<Vector2Int> ComputeOutDirections(Vector2Int sourceDirection)
    {
        yield return new Vector2Int(-sourceDirection[1], sourceDirection[0]);
        yield return new Vector2Int(sourceDirection[1], -sourceDirection[0]);
    }
}