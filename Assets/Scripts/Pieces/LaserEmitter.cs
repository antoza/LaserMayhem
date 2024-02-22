using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

#nullable enable
public class LaserEmitter : Piece // TODO : renommer en Emitter
{
    // TODO : permettre qqc comme ça dans l'editor : private List<Direction> _startingDirections;
    // TODO : tourner la pièce de la bonne rotation en fonction du laser de départ
    protected List<Vector2Int> _startingDirections = new List<Vector2Int>() { Vector2Int.right };

    public override void ReceiveLaser(Laser? laser, Vector2Int inDirection)
    {
        return;
    }

    public void StartLaser(Laser? laser)
    {
        Assert.IsTrue(ParentTile is BoardTile);
        foreach (Vector2Int startingDirection in _startingDirections)
        {
            ((BoardTile?)ParentTile)!.TransferLaser(laser, startingDirection);
        }
    }

    //TODO : Events may be renamed
    /*private void Start()
    {
        TurnManager.OnEndTurn += OnEndTurn;
        TurnManager.OnEndLaserPhase += OnEndLaserPhase;
    }
    
    private void OnEndTurn()
    {
        StartLaser(false);
    }

    private void OnEndLaserPhase()
    {
        StartLaser(true);
    }*/
}
