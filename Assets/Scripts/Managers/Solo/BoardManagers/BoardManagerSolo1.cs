using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using System;
using UnityEngine.Assertions;
using System.Net;
using Unity.Collections;

#nullable enable
public class BoardManagerSolo1 : BoardManagerSolo
{
    protected override void GenerateAllTiles()
    {
        GameObject boardTilesParent = new GameObject("BoardTiles");
        boardTilesParent.transform.parent = _boardParent.transform;
        GenerateBoardTilesInArea(-5, 2, -2, 2, TileName.NormalBoardTile, boardTilesParent);

        GenerateBoardTile(-5, -1, TileName.LockedBoardTile, _boardParent, PieceName.LaserEmitter);

        GenerateBoardTile(0, 2, TileName.LockedBoardTile, _boardParent, PieceName.Eye);
    }
}
