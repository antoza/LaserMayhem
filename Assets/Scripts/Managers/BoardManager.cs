using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using System;
using UnityEngine.Assertions;
using System.Net;
using Unity.Collections;

#nullable enable
public sealed class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; private set; }

    private Dictionary<Vector2Int, BoardTile> _board = new Dictionary<Vector2Int, BoardTile>();
    [SerializeField]
    private GameObject _boardParent;

    private List<BoardTile> _weakSides = new List<BoardTile>();

    // TODO : créer un référenceur de lasers où stocker les gameobjects
    [SerializeField]
    private List<Laser> _lasers;

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
    public void RegisterBoardTile(Vector2Int spot, BoardTile tile)
    {
        Assert.IsFalse(_board.ContainsKey(spot));
        _board[spot] = tile;
    }

    public BoardTile? GetBoardTile(Vector2Int spot)
    {
        if (_board.TryGetValue(new Vector2Int(spot[0], spot[1]), out BoardTile tile)) {
            return tile;
        }
        return null;
    }
    /*
    public bool IsOnBoard(Vector2Int tile)
    {
        return tile[0] >= 0 && tile[0] < Width && tile[1] >= 0 && tile[1] < Height;
    }*/

    // TODO : peut-être mettre tout ceci dans LaserManager
    private void DisplayLaser(Laser? laser)
    {
        // TODO : faire en sorte de connaître directement tous les LaserEmitters
        foreach (BoardTile tile in _board.Values)
        {
            if (tile.Piece is LaserEmitter)
            {
                ((LaserEmitter)tile.Piece).StartLaser(laser);
            }
        }
    }

    public void DisplayEndTurnLaser()
    {
        DisplayLaser(_lasers[0]);
        SoundManager.Instance.PlayLaserSound();
    }

    public void DisplayPredictionLaser()
    {
        DisplayLaser(_lasers[1]);
    }

    public void ClearLaser()
    {
        DisplayLaser(null);
    }

    public void SwitchWeakSides(int turnNumber)
    {
        foreach (BoardTile tile in _weakSides)
        {
            tile.Piece = tile.PieceStorage[(turnNumber-1)%2];
        }
    }


    /*
    //End turn events
    public delegate void DestroyLaserEvent();
    public static event DestroyLaserEvent? DestroyLaser;

    public delegate void EndTurnEventHandler();
    public static event EndTurnEventHandler? OnEndTurn;

    public delegate void EndLaserPhaseEventHandler();
    public static event EndLaserPhaseEventHandler? OnEndLaserPhase;


    //Dans StartLaserPhase
    DestroyLaser?.Invoke();
    OnEndTurn?.Invoke();

    // Dans StartAnnouncementPhase
    DestroyLaser?.Invoke();
    OnEndLaserPhase?.Invoke();


    Faut aussi les appeller dans le RPG.cs lorsqu'une pièce est ajouté, déplacée ou enlevée.
    */

    // TODO : le GenerateAllTiles doit être abstrait, et BoardManagerRPG doit l'implémenter
    private void GenerateAllTiles()
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
    
    private List<BoardTile> GenerateBoardTilesInArea(int xLeft, int xRight, int yBottom, int yTop, TileName tileName, GameObject? parent)
    {
        List<BoardTile> tiles = new List<BoardTile>();
        for (int x = xLeft; x <= xRight; x++)
        {
            for (int y = yBottom; y <= yTop; y++)
            {
                tiles.Add(GenerateBoardTile(x, y, tileName, parent));
            }
        }
        return tiles;
    }
    
    private BoardTile GenerateBoardTile(int x, int y, TileName tileName, GameObject? parent)
    {
        BoardTile tile = (BoardTile)TilePrefabs.Instance.GetTile(tileName).InstantiateTile();
        if (parent) tile.transform.SetParent(parent!.transform);
        tile.name = $"{tileName}_{x}_{y}";

        Vector2Int spot = new Vector2Int(x, y);
        tile.Spot = spot;
        tile.SetColor(); // TODO : cette ligne n'est pas à sa place, trouver un meilleur endroit où l'appeler
        int sign = GameInitialParameters.localPlayerID == 1 ? -1 : 1;
        tile.transform.position = sign * (Vector2)spot;

        RegisterBoardTile(spot, tile);
        return tile;
    }
}
