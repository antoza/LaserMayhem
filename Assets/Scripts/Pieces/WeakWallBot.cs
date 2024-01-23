using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeakWallBot : Piece
{
    public override IEnumerable<Vector2Int> ComputeNewDirections(Vector2Int sourceDirection)
    {
        return Enumerable.Empty<Vector2Int>();
    }
}