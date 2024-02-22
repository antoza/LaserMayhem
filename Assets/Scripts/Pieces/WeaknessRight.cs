using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable enable
public class WeaknessRight : Weakness
{
    private void Awake()
    {
        directions = new Dictionary<Vector2Int, int>() { { Vector2Int.right, 0 } };
    }
}