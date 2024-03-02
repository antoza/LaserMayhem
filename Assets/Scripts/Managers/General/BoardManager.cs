using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using System;
using UnityEngine.Assertions;
using System.Net;
using Unity.Collections;
using Unity.VisualScripting;

#nullable enable
public abstract class BoardManager : Manager<BoardManager>
{
    private Dictionary<Vector2Int, BoardTile> _board = new Dictionary<Vector2Int, BoardTile>();
    [SerializeField]
    protected GameObject _boardParent;

    // TODO : créer un référenceur de lasers où stocker les gameobjects
    [SerializeField]
    private List<Laser> _lasers;

    protected override void Awake()
    {
        base.Awake();
    }

    protected void Start()
    {
        GenerateAllTiles();
    }

    public void RegisterBoardTile(Vector2Int spot, BoardTile tile)
    {
        if (_board.TryGetValue(spot, out BoardTile oldTile))
        {
            Destroy(oldTile.gameObject);
        }
        _board[spot] = tile;
    }

    public BoardTile? GetBoardTile(Vector2Int spot)
    {
        if (_board.TryGetValue(new Vector2Int(spot[0], spot[1]), out BoardTile tile)) {
            return tile;
        }
        return null;
    }

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

    // TODO : pareil, avoir une liste des weakness, peut-être même avoir une liste de listes de weakness
    public IEnumerable<Receiver> GetReceivers()
    {
        // TODO : faire en sorte de connaître directement tous les LaserEmitters
        foreach (BoardTile tile in _board.Values)
        {
            if (tile.Piece is Receiver)
            {
                yield return (Receiver)tile.Piece;
            }
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

    protected virtual void GenerateAllTiles() { }
    
    protected List<BoardTile> GenerateBoardTilesInArea(int xLeft, int xRight, int yBottom, int yTop, TileName tileName, GameObject? parent)
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
    
    protected BoardTile GenerateBoardTile(int x, int y, TileName tileName, GameObject? parent, PieceName initialPiece = PieceName.None)
    {
        BoardTile tile = (BoardTile)TilePrefabs.Instance.GetTile(tileName).InstantiateTile();
        if (parent) tile.transform.SetParent(parent!.transform);
        tile.name = $"{tileName}_{x}_{y}";

        Vector2Int spot = new Vector2Int(x, y);
        tile.Spot = spot;
        tile.SetColor(); // TODO : cette ligne n'est pas à sa place, trouver un meilleur endroit où l'appeler
        int sign = GameInitialParameters.localPlayerID == 1 ? -1 : 1;
        tile.transform.position = sign * (Vector2)spot;

        tile.InstantiatePiece(initialPiece);

        RegisterBoardTile(spot, tile);
        return tile;
    }
}
