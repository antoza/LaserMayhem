using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Wall : Piece
{
    public static Wall CreateInstance()
    {
        var instance = GetInstance<Wall>();
        return instance;
    }

    public override IEnumerable<Vector2Int> ComputeNewDirections(Vector2Int sourceDirection)
    {
        return Enumerable.Empty<Vector2Int>();
    }
}