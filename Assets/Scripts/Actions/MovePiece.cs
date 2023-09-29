using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePiece : RevertableAction
{
    public Tile SourceTile { get; private set; }
    public Tile TargetTile { get; private set; }
    public Piece Piece { get; private set; }

    public MovePiece(DataManager dataManager, PlayerData playerData, Tile sourceTile, Tile targetTile, Piece piece) : base(dataManager, playerData)
    {
        SourceTile = sourceTile;
        TargetTile = targetTile;
        Piece = piece;
    }

    public override void Revert()
    {
        DM.GameMode.RevertMove(SourceTile, TargetTile, Piece, PlayerData);
    }
}
