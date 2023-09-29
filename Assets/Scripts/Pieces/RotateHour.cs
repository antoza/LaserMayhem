using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RotateHour : Piece
{
    public static Divider CreateInstance()
    {
        var instance = GetInstance<Divider>();
        return instance;
    }

    public override IEnumerable<Vector2Int> ComputeNewDirections(Vector2Int sourceDirection)
    {
        yield return new Vector2Int(-sourceDirection[1], sourceDirection[0]);
    }
}