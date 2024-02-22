using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

#nullable enable
public abstract class Receiver : Piece
{
    protected Dictionary<Vector2Int, int> directions;

    private void Awake()
    {
        directions = new Dictionary<Vector2Int, int>() { { Vector2Int.down, 0 },
            { Vector2Int.up, 0 },
            { Vector2Int.left, 0 },
            { Vector2Int.right, 0 },
        };
    }

    public override void ReceiveLaser(Laser? laser, Vector2Int inDirection)
    {
        base.ReceiveLaser(laser, inDirection);
        if (directions.ContainsKey(inDirection))
        {
            directions[inDirection] = (laser != null) ? laser!.Intensity : 0;
        }
        else
        {
            // TODO : pouvoir choisir le comportement en dehors du laser
            ((BoardTile)ParentTile!).TransferLaser(laser, inDirection);
        }
    }

    public abstract int GetReceivedIntensity();
}
