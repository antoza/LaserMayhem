using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using System;
using UnityEngine.Assertions;
using System.Net;
using Unity.Collections;

#nullable enable
public class BoardManagerSolo : BoardManager
{
    public static new BoardManagerSolo Instance => (BoardManagerSolo)BoardManager.Instance;

    protected override void GenerateAllTiles()
    {
        GameObject boardTilesParent = new GameObject("BoardTiles");
        boardTilesParent.transform.parent = _boardParent.transform;
        GenerateBoardTilesInArea(-4, 4, -2, 2, TileName.NormalBoardTile, boardTilesParent);

        BoardTile laserGeneratorTile = GenerateBoardTile(-4, 0, TileName.InvisibleBoardTile, _boardParent);
        laserGeneratorTile.InstantiatePiece(PieceName.LaserEmitter);
    }
}
