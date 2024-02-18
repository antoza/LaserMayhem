using UnityEngine;
using System;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
#nullable enable

public abstract class TurnManager : Manager<TurnManager>
{
    public int TurnNumber { get; protected set; } = 0;

    [field: SerializeField]
    public int LaserPhaseDuration { get; private set; }

    protected HashSet<Piece> _piecesPlayedThisTurn = new HashSet<Piece>();

    protected virtual void Start()
    {
        StartCoroutine(StartTurnCoroutine());
    }

    public void EndTurn()
    {
        StartCoroutine(EndTurnCoroutine());
    }

    protected abstract IEnumerator StartTurnCoroutine();

    protected abstract IEnumerator EndTurnCoroutine();


    // Phases

    protected virtual void StartLaserPhase()
    {
#if !DEDICATED_SERVER
        ((UIManagerGame)UIManager.Instance).UpdateEndTurnButtonState("Pressed");
        if (LocalPlayerManager.Instance.IsLocalPlayersTurn()) LocalPlayerManager.Instance.ResetSourceTile();
#endif
        RewindManager.Instance.ClearAllActions();
        BoardManager.Instance.DisplayEndTurnLaser();
        ResetPiecesPlayedThisTurn();
    }

    protected virtual void StartTurnPhase()
    {
#if !DEDICATED_SERVER
        ((UIManagerGame)UIManager.Instance).UpdateEndTurnButtonState("Unpressed");
#endif
    }


    // Pieces played this turn

    public void ResetPiecesPlayedThisTurn()
    {
        while (_piecesPlayedThisTurn.Count > 0)
        {
            _piecesPlayedThisTurn.First().IsPlayedThisTurn = false;
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
