using Mono.CompilerServices.SymbolWriter;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

#nullable enable
public class ServerSpawnPieceAction : Action
{
    public Tile Tile;
    public PieceName PieceName;

    public override string SerializeAction()
    {
        return base.SerializeAction() + "+" + Tile.name + "+" + PieceName;
    }

    public ServerSpawnPieceAction() : base()
    {
    }

    public ServerSpawnPieceAction(Tile tile, PieceName pieceName) : base()
    {
        Tile = tile;
        PieceName = pieceName;
    }

    public override bool DeserializeSubAction(Queue<string> parsedString)
    {
        base.DeserializeSubAction(parsedString);
        try
        {
            Tile = GameObject.Find(parsedString.Dequeue()).GetComponent<Tile>();
            Assert.IsTrue(Enum.TryParse(parsedString.Dequeue(), out PieceName));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    /*
    public override void NetworkSerialize<T>(BufferSerializer<T> serializer)
    {
        base.NetworkSerialize(serializer);
        serializer.SerializeValue(ref SourceTileID);
        serializer.SerializeValue(ref TargetTileID);
    }*/
}
