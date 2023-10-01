using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePiece : RevertableAction
{
    public Tile SourceTile { get; private set; }
    public Tile TargetTile { get; private set; }
    public Piece Piece { get; private set; }

    public MovePiece(PlayerData playerData, Tile sourceTile, Tile targetTile, Piece piece) : base(playerData)
    {
        SourceTile = sourceTile;
        TargetTile = targetTile;
        Piece = piece;
    }

    public override void Revert()
    {
        DataManager.Instance.GameMode.RevertMove(SourceTile, TargetTile, Piece, PlayerData);
    }
}
