using Mono.CompilerServices.SymbolWriter;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

#nullable enable
public class MovePieceAction : Action
{
    public string SourceTileID;
    public string TargetTileID;
    public Piece? SourcePiece;
    public Piece? TargetPiece;
    
    public Tile SourceTile => GameObject.Find(SourceTileID).GetComponent<Tile>();
    public Tile TargetTile => GameObject.Find(TargetTileID).GetComponent<Tile>();

    public override string SerializeAction()
    {
        return base.SerializeAction() + '+' + SourceTileID + '+' + TargetTileID;
    }

    public MovePieceAction() : base()
    {
        SourceTileID = "";
        TargetTileID = "";
    }

    public MovePieceAction(PlayerData playerData, Tile sourceTile, Tile targetTile) : base(playerData)
    {
        SourceTileID = sourceTile.name;
        TargetTileID = targetTile.name;
    }

    public override bool Initialize(string[] parsedString)
    {
        base.Initialize(parsedString);
        try
        {
            SourceTileID = parsedString[2];
            TargetTileID = parsedString[3];
            return true;
        }
        catch (Exception ex) when (ex is IndexOutOfRangeException)
        {
            return false;
        }
    }

    public override void NetworkSerialize<T>(BufferSerializer<T> serializer)
    {
        base.NetworkSerialize(serializer);
        serializer.SerializeValue(ref SourceTileID);
        serializer.SerializeValue(ref TargetTileID);
    }
}
