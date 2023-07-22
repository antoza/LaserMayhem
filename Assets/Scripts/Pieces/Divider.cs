using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Divider : Piece
{
    public override IEnumerable<(int, int)> ComputeNewDirections((int, int) sourceDirection)
    {
        yield return (sourceDirection.Item2, sourceDirection.Item1);
        yield return (sourceDirection.Item2, -sourceDirection.Item1);
    }
}