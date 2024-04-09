using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

#nullable enable
public class Orb : Piece
{
    public override void ReceiveLaser(Laser? laser, Vector2Int inDirection)
    {
        base.ReceiveLaser(laser, inDirection);

        ((BoardTile)ParentTile!).TransferLaser(laser, inDirection);
        if (laser != null && laser.Intensity > 0)
        {
            ParentTile!.Piece = null;
            Destroy(this);
        }
    }
}
