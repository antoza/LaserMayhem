using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using System;
using UnityEngine.Assertions;
using System.Net;
using Unity.Collections;

#nullable enable
public class BoardManagerSolo3 : BoardManagerSolo
{
    protected override void GenerateAllTiles()
    {
        GameObject boardTilesParent = new GameObject("BoardTiles");
        boardTilesParent.transform.parent = _boardParent.transform;
        GenerateBoardTilesInArea(-4, 3, -3, 2, TileName.NormalBoardTile, boardTilesParent);

        GenerateBoardTile(-4, 0, TileName.LockedBoardTile, _boardParent, PieceName.LaserEmitter);

        GenerateBoardTile(3, 0, TileName.LockedBoardTile, _boardParent, PieceName.Eye);

        GenerateBoardTile(-1, 0, TileName.LockedBoardTile, _boardParent, PieceName.MirrorHour);
        GenerateBoardTile(-1, 2, TileName.LockedBoardTile, _boardParent, PieceName.MirrorSlash);
        GenerateBoardTile(1, 2, TileName.LockedBoardTile, _boardParent, PieceName.MirrorBackSlash);
        GenerateBoardTile(1, -3, TileName.LockedBoardTile, _boardParent, PieceName.MirrorSlash);
        GenerateBoardTile(-1, -3, TileName.LockedBoardTile, _boardParent, PieceName.MirrorBackSlash);
    }
}
