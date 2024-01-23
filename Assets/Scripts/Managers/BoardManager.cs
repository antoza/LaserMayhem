using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using System;
using UnityEngine.Assertions;

#nullable enable
public sealed class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; private set; }

    private Dictionary<Vector2Int, Tile> _board = new Dictionary<Vector2Int, Tile>();
    [SerializeField]
    private GameObject _boardParent;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GenerateAllTiles();
    }
    /*
    public Piece? GetPiece(Vector2Int tile)
    {
        return _board.TryGetValue(new Vector2Int(tile[0], tile[1]).Piece;
    }
    */
    public void RegisterTile(Vector2Int spot, Tile tile)
    {
        Assert.IsFalse(_board.ContainsKey(spot));
        _board[spot] = tile;
    }

    public Tile? GetTile(Vector2Int spot)
    {
        if (_board.TryGetValue(new Vector2Int(spot[0], spot[1]), out Tile tile)) {
            return tile;
        }
        return null;
    }
    /*
    public bool IsOnBoard(Vector2Int tile)
    {
        return tile[0] >= 0 && tile[0] < Width && tile[1] >= 0 && tile[1] < Height;
    }*/
    
    private void GenerateAllTiles()
    {
        GameObject boardTilesParent = new GameObject("BoardTiles");
        boardTilesParent.transform.parent = _boardParent.transform;
        GenerateTilesInArea(-3, 3, -3, 3, TileName.BoardTile, boardTilesParent);

        GameObject weakBlueSideParent = new GameObject("WeakBlueSide");
        weakBlueSideParent.transform.parent = _boardParent.transform;
        List<Tile> weakBlueSide = GenerateTilesInArea(-3, 3, -4, -4, TileName.InvisibleTile, weakBlueSideParent);
        foreach (Tile tile in weakBlueSide)
        {
            tile.InstantiatePiece(PieceName.WeakWallBot);
        }

        GameObject weakRedSideParent = new GameObject("WeakRedSide");
        weakRedSideParent.transform.parent = _boardParent.transform;
        List<Tile> weakRedSide = GenerateTilesInArea(-3, 3, 4, 4, TileName.InvisibleTile, weakRedSideParent);
        foreach (Tile tile in weakRedSide)
        {
            tile.InstantiatePiece(PieceName.WeakWallTop);
        }

        GameObject weakLeftSideParent = new GameObject("WeakLeftSide");
        weakLeftSideParent.transform.parent = _boardParent.transform;
        List<Tile> weakLeftSide = GenerateTilesInArea(-4, -4, -3, 3, TileName.InvisibleTile, weakLeftSideParent);
        foreach (Tile tile in weakLeftSide)
        {
            tile.StorePiece(PieceName.WeakWallLeft);
            //tile.PieceStorage[0].WeakPlayer = 0;
            tile.StorePiece(PieceName.WeakWallLeft);
            //tile.PieceStorage[1].WeakPlayer = 1;
            tile.Piece = tile.PieceStorage[0];
        }

        GameObject weakRightSideParent = new GameObject("WeakRightSide");
        weakRightSideParent.transform.parent = _boardParent.transform;
        List<Tile> weakRightSide = GenerateTilesInArea(4, 4, -3, 3, TileName.InvisibleTile, weakRightSideParent);
        foreach (Tile tile in weakRightSide)
        {
            tile.StorePiece(PieceName.WeakWallRight);
            //tile.PieceStorage[0].WeakPlayer = 0;
            tile.StorePiece(PieceName.WeakWallRight);
            //tile.PieceStorage[1].WeakPlayer = 1;
            tile.Piece = tile.PieceStorage[0];
        }

        Tile laserGeneratorTile = GenerateTile(-5, 0, TileName.InvisibleTile, _boardParent);
        //laserGeneratorTile.InstantiatePiece(PieceName.LaserGeneratorRight);
    }
    
    private List<Tile> GenerateTilesInArea(int xLeft, int xRight, int yBottom, int yTop, TileName tileName, GameObject? parent)
    {
        List<Tile> tiles = new List<Tile>();
        for (int x = xLeft; x <= xRight; x++)
        {
            for (int y = yBottom; y <= yTop; y++)
            {
                tiles.Add(GenerateTile(x, y, tileName, parent));
            }
        }
        return tiles;
    }
    
    private Tile GenerateTile(int x, int y, TileName tileName, GameObject? parent)
    {
        Tile tile = TilePrefabs.Instance.GetTile(tileName).InstantiateTile();
        if (parent) tile.transform.SetParent(parent!.transform);
        tile.name = $"{tileName}_{x}_{y}";

        Vector2Int spot = new Vector2Int(x, y);
        tile.Spot = spot;
        int sign = GameInitialParameters.localPlayerID == 1 ? -1 : 1;
        tile.transform.position = sign * (Vector2)spot;

        RegisterTile(spot, tile);
        return tile;
    }
    /*
    public Vector2Int ConvertBoardCoordinateToWorldCoordinates(Vector2Int coord)
    {
        return new Vector2Int(coord[0] - (Width-1)/2, coord[1] - (Height-1) / 2);
    }*/
}
