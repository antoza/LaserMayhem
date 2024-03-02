using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using System;
using UnityEngine.Assertions;
using System.Net;
using Unity.Collections;

#nullable enable
public class BoardManagerSolo2 : BoardManagerSolo
{
    protected override void GenerateAllTiles()
    {
        GameObject boardTilesParent = new GameObject("BoardTiles");
        boardTilesParent.transform.parent = _boardParent.transform;
        GenerateBoardTilesInArea(-3, 3, -3, 2, TileName.NormalBoardTile, boardTilesParent);

        GenerateBoardTile(-4, 2, TileName.LockedBoardTile, _boardParent, PieceName.LaserEmitter);

        GenerateBoardTile(-2, -3, TileName.LockedBoardTile, _boardParent, PieceName.Eye);

        GenerateBoardTile(3, 2, TileName.LockedBoardTile, _boardParent, PieceName.MirrorTrigo);
        GenerateBoardTile(3, -1, TileName.LockedBoardTile, _boardParent, PieceName.MirrorTrigo);
    }
}
