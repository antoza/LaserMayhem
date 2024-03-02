using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using System;
using UnityEngine.Assertions;
using System.Net;
using Unity.Collections;

#nullable enable
public class BoardManagerSolo6 : BoardManagerSolo
{
    protected override void GenerateAllTiles()
    {
        GameObject boardTilesParent = new GameObject("BoardTiles");
        boardTilesParent.transform.parent = _boardParent.transform;
        GenerateBoardTilesInArea(-1, 2, -2, 2, TileName.NormalBoardTile, boardTilesParent);

        GenerateBoardTile(-2, -0, TileName.LockedBoardTile, _boardParent, PieceName.LaserEmitter);

        GenerateBoardTile(2, 2, TileName.LockedBoardTile, _boardParent, PieceName.Eye);
        GenerateBoardTile(-1, -2, TileName.LockedBoardTile, _boardParent, PieceName.Eye);
        GenerateBoardTile(1, -2, TileName.LockedBoardTile, _boardParent, PieceName.Eye);

        GenerateBoardTile(1, 0, TileName.LockedBoardTile, _boardParent, PieceName.MirrorHour);
        GenerateBoardTile(1, 2, TileName.LockedBoardTile, _boardParent, PieceName.MirrorBackSlash);
    }
}
