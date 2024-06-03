using UnityEngine;
using System;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using UnityEngine.Assertions;
#nullable enable

public abstract class TurnManager : Manager<TurnManager>
{
    public int TurnNumber { get; protected set; } = 0;

    [field: SerializeField]
    public int LaserPhaseDuration { get; private set; }
    [field: SerializeField]
    public int RevertLaserPhaseDuration { get; private set; }

    [HideInInspector]
    public bool IsWaitingForPlayerAction { get; protected set; } = false;

    protected HashSet<Piece> _piecesPlayedThisTurn = new HashSet<Piece>();

    protected virtual void Start()
    {
        StartCoroutine(StartTurnCoroutine());
    }

    public void EndTurn(EndTurnAction action)
    {
        StartCoroutine(EndTurnCoroutine(action));
    }

    public void RevertEndTurn(EndTurnAction action)
    {
        StartCoroutine(RevertEndTurnCoroutine(action));
    }

    protected abstract IEnumerator StartTurnCoroutine();

    protected abstract IEnumerator EndTurnCoroutine(EndTurnAction action);

    protected abstract IEnumerator RevertEndTurnCoroutine(EndTurnAction action);



    // Pieces played this turn

    public void ClearPiecesPlayedThisTurn(EndTurnAction action)
    {
        while (_piecesPlayedThisTurn.Count > 0)
        {
            Piece piece = _piecesPlayedThisTurn.First();
            action.PiecesPlayedThisTurn.Add(piece);
            _piecesPlayedThisTurn.First().IsPlayedThisTurn = false;
        }
    }

    public void RevertClearPiecesPlayedThisTurn(EndTurnAction action)
    {
        foreach (Piece piece in action.PiecesPlayedThisTurn)
        {
            piece.IsPlayedThisTurn = true;
        }
    }

    public void AddPiecePlayedThisTurn(Piece piece)
    {
        _piecesPlayedThisTurn.Add(piece);
    }

    public void RemovePiecePlayedThisTurn(Piece piece)
    {
        _piecesPlayedThisTurn.Remove(piece);
    }
}
