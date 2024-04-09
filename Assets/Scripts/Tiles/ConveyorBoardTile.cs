using UnityEngine;

#nullable enable
public class ConveyorBoardTile : BoardTile
{
    public Vector2Int ConveyorDirection = Vector2Int.down;

    public void ConveyPiece()
    {
        BoardTile? targetTile = BoardManager.Instance.GetBoardTile(Spot + ConveyorDirection);
        if (targetTile == null) return;
        if (targetTile is ConveyorBoardTile) ((ConveyorBoardTile)targetTile).ConveyPiece();
        if (targetTile.Piece == null) targetTile.Piece = Piece;
    }
}
