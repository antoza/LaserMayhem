using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

#nullable enable
public abstract class Mirror : Piece
{
    public override void ReceiveLaser(Laser? laser, Vector2Int inDirection)
    {
        base.ReceiveLaser(laser, inDirection);
        foreach (Vector2Int outDirection in ComputeOutDirections(inDirection))
        {
            ((BoardTile?)ParentTile)!.TransferLaser(laser, outDirection);
        }
    }

    public abstract IEnumerable<Vector2Int> ComputeOutDirections(Vector2Int inDirection);
}
