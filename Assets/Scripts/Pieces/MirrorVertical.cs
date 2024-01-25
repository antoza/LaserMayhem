using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class MirrorVertical : Mirror
{
    public override IEnumerable<Vector2Int> ComputeOutDirections(Vector2Int sourceDirection)
    {
        yield return new Vector2Int(-sourceDirection[0], sourceDirection[1]);
    }
}
