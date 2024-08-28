using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

#nullable enable
public class MovePieceAction : PlayerAction
{    
    public Tile SourceTile;
    public Tile TargetTile;
    public Piece? SourcePiece;
    public Piece? TargetPiece;

    public override string SerializeAction()
    {
        return base.SerializeAction() + "+" + SourceTile.name + "+" + TargetTile.name;
    }

    public MovePieceAction() : base()
    {
    }

    public MovePieceAction(PlayerData playerData, Tile sourceTile, Tile targetTile) : base(playerData)
    {
        SourceTile = sourceTile;
        TargetTile = targetTile;
    }

    public override bool DeserializeSubAction(Queue<string> parsedString)
    {
        base.DeserializeSubAction(parsedString);
        try
        {
            SourceTile = GameObject.Find(parsedString.Dequeue()).GetComponent<Tile>();
            TargetTile = GameObject.Find(parsedString.Dequeue()).GetComponent<Tile>();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
