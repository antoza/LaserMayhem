using UnityEngine;
using System;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
#nullable enable

public sealed class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    private int m_TurnNumber = 0;

    [Header("Time Management")]
    private float m_AnnouncementPhaseDuration;
    private float m_LaserPhaseDuration;

    [Header("Test Data")]
    private RandomPieceGenerator m_PiecesData;

    private SelectionTilesUpdate m_SelectionTilesUpdate;

    private HashSet<Piece> _piecesPlayedThisTurn = new HashSet<Piece>();

    void Awake()
    {
        Instance = this;
        m_AnnouncementPhaseDuration = DataManager.Instance.Rules.AnnouncementPhaseDuration;
        m_LaserPhaseDuration = DataManager.Instance.Rules.LaserPhaseDuration;
        m_PiecesData = FindObjectOfType<RandomPieceGenerator>();
        m_SelectionTilesUpdate = FindObjectOfType<SelectionTilesUpdate>();
    }
    /*
    public static void SetInstance()
    {
        Instance = new TurnManager();
    }

    public static TurnManager GetInstance()
    {
        if (Instance == null)
        {
            Debug.LogError("TurnManager has not been instantiated");
        }

        return Instance!;
    }*/

    public void Start()
    {
        StartAnnouncementPhase();
    }

    public void StartLaserPhase()
    {
#if !DEDICATED_SERVER
        ((UIManagerGame)UIManager.Instance).UpdateEndTurnButtonState("Pressed");
        if (LocalPlayerManager.Instance.IsLocalPlayersTurn()) LocalPlayerManager.Instance.ResetSourceTile(); // TODO : Ligne à modifier pour ne pas avoir à appeler LocalPlayerManager ici
#endif
        RewindManager.Instance.ClearAllActions();
        BoardManager.Instance.DisplayEndTurnLaser();
        ResetPiecesPlayedThisTurn();
        StartCoroutine(LaserPhaseCoroutine());
    }

    public IEnumerator LaserPhaseCoroutine()
    {
        yield return new WaitForSeconds(m_LaserPhaseDuration);
        StartAnnouncementPhase();
    }

    public void StartAnnouncementPhase()
    { 
        if (GameModeManager.Instance.CheckGameOver())
        {
            return;
        }
        PlayersManager.Instance.StartNextPlayerTurn(++m_TurnNumber);
#if !DEDICATED_SERVER
        ((UIManagerGame)UIManager.Instance).TriggerPlayerTurnAnnouncement();
#endif
        BoardManagerRPG.Instance.SwitchWeakSides(m_TurnNumber);
        BoardManager.Instance.DisplayPredictionLaser();
#if DEDICATED_SERVER
        if (GameInitialParameters.localPlayerID == -1) m_SelectionTilesUpdate.ServerUpdateSelectionPieces(); // TODO : Ligne à modifier
#endif
        StartCoroutine(AnnouncementPhaseCoroutine());
    }

    public IEnumerator AnnouncementPhaseCoroutine()
    {
        yield return new WaitForSeconds(m_AnnouncementPhaseDuration);
        StartTurnPhase();
    }

    public void StartTurnPhase()
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
