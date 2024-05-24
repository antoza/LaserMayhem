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
public abstract class Gem : Receiver
{
    public int HP;

    public override void ReceiveLaser(Laser? laser, Vector2Int inDirection)
    {
        base.ReceiveLaser(laser, inDirection);

        ((BoardTile)ParentTile!).TransferLaser(laser, inDirection);
    }

    public override int GetReceivedIntensity()
    {
        return directions.Values.Sum();
    }

    public void UpdateState()
    {
        int receivedIntensity = GetReceivedIntensity();
        if (receivedIntensity > 0)
        {
            HP -= receivedIntensity;
            if (HP < 0) HP = 0;
            // Animate
        }
    }

    public void Destroy()
    {
        ParentTile!.Piece = null;
        Destroy(this);
    }
}
