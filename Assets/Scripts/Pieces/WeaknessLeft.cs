using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable enable
public class WeaknessLeft : Weakness
{
    private void Awake()
    {
        directions = new Dictionary<Vector2Int, int>() { { Vector2Int.left, 0 } };
    }
}