using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

#nullable enable
public abstract class Weakness : Piece
{
    protected List<Vector2Int> weaknessDirections;
    private PlayerData _weakPlayer;
    public PlayerData WeakPlayer {
        get => _weakPlayer;
        set {
            _weakPlayer = value;
            GetComponent<SpriteRenderer>().color = value.PlayerID == 0 ? new Color(.2f, .4f, 1f, 1f) : new Color(1f, .2f, .2f, 1f);
        }
    }

    public override void ReceiveLaser(Laser? laser, Vector2Int inDirection)
    {
        base.ReceiveLaser(laser, inDirection);
        if (weaknessDirections.Contains(inDirection))
        {
            if (laser != null && laser!.DealsDamage)
            {
                WeakPlayer.PlayerHealth.TakeDamage(1); // Mettre le nb de dégâts renseigné par le laser
#if !DEDICATED_SERVER
                ((UIManagerGame)UIManager.Instance).DisplayHealthLoss(1, transform.position);
#endif
            }
        }
        else
        {
            ((BoardTile)ParentTile!).TransferLaser(laser, inDirection);
        }
    }
}
