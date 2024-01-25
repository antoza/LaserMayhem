using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable enable
public class WeaknessRight : Weakness
{
    private void Awake()
    {
        weaknessDirections = new List<Vector2Int>() { Vector2Int.right };
    }
}