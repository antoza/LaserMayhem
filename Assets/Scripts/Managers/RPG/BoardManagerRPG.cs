using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using System;
using UnityEngine.Assertions;
using System.Net;
using Unity.Collections;

#nullable enable
public class BoardManagerRPG : BoardManager
{
    public static new BoardManagerRPG Instance => (BoardManagerRPG)BoardManager.Instance;

    private List<BoardTile> _weakSides = new List<BoardTile>();

    public void SwitchWeakSides(int turnNumber)
    {
        foreach (BoardTile tile in _weakSides)
        {
            tile.Piece = tile.PieceStorage[(turnNumber-1)%2];
        }
    }

    protected override void GenerateAllTiles()
    {
        GameObject boardTilesParent = new GameObject("BoardTiles");
        boardTilesParent.transform.parent = _boardParent.transform;
        GenerateBoardTilesInArea(-3, 3, -3, 3, TileName.NormalBoardTile, boardTilesParent);

        GameObject weakBlueSideParent = new GameObject("WeakBlueSide");
        weakBlueSideParent.transform.parent = _boardParent.transform;
        List<BoardTile> weakBlueSide = GenerateBoardTilesInArea(-3, 3, -4, -4, TileName.InvisibleBoardTile, weakBlueSideParent);
        foreach (BoardTile tile in weakBlueSide)
        {
            tile.InstantiatePiece(PieceName.WeaknessDown);
            ((Weakness)tile.Piece!).WeakPlayer = PlayersManager.Instance.GetPlayer(0);
        }

        GameObject weakRedSideParent = new GameObject("WeakRedSide");
        weakRedSideParent.transform.parent = _boardParent.transform;
        List<BoardTile> weakRedSide = GenerateBoardTilesInArea(-3, 3, 4, 4, TileName.InvisibleBoardTile, weakRedSideParent);
        foreach (BoardTile tile in weakRedSide)
        {
            tile.InstantiatePiece(PieceName.WeaknessUp);
            ((Weakness)tile.Piece!).WeakPlayer = PlayersManager.Instance.GetPlayer(1);
        }

        GameObject weakLeftSideParent = new GameObject("WeakLeftSide");
        weakLeftSideParent.transform.parent = _boardParent.transform;
        List<BoardTile> weakLeftSide = GenerateBoardTilesInArea(-4, -4, -3, 3, TileName.InvisibleBoardTile, weakLeftSideParent);
        foreach (BoardTile tile in weakLeftSide)
        {
            tile.StorePiece(PieceName.WeaknessLeft);
            ((Weakness)tile.PieceStorage[0]).WeakPlayer = PlayersManager.Instance.GetPlayer(0);
            tile.StorePiece(PieceName.WeaknessLeft);
            ((Weakness)tile.PieceStorage[1]).WeakPlayer = PlayersManager.Instance.GetPlayer(1);
        }
        _weakSides.AddRange(weakLeftSide);

        GameObject weakRightSideParent = new GameObject("WeakRightSide");
        weakRightSideParent.transform.parent = _boardParent.transform;
        List<BoardTile> weakRightSide = GenerateBoardTilesInArea(4, 4, -3, 3, TileName.InvisibleBoardTile, weakRightSideParent);
        foreach (BoardTile tile in weakRightSide)
        {
            tile.StorePiece(PieceName.WeaknessRight);
            ((Weakness)tile.PieceStorage[0]).WeakPlayer = PlayersManager.Instance.GetPlayer(0);
            tile.StorePiece(PieceName.WeaknessRight);
            ((Weakness)tile.PieceStorage[1]).WeakPlayer = PlayersManager.Instance.GetPlayer(1);
        }
        _weakSides.AddRange(weakRightSide);

        BoardTile laserGeneratorTile = GenerateBoardTile(-5, 0, TileName.InvisibleBoardTile, _boardParent);
        laserGeneratorTile.InstantiatePiece(PieceName.LaserEmitter);
    }
}
