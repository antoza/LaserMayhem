using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorSlash : Piece
{
    public override IEnumerable<(int, int)> ComputeNewDirections((int, int) sourceDirection)
    {
        yield return (sourceDirection.Item2, sourceDirection.Item1);
    }
}