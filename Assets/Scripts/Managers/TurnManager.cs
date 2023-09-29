using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class TurnManager : ScriptableObject
{
    private int m_TurnNumber = 0;
    private DataManager DM;

    [Header("Time Management")]
    private float m_SkipTurnCooldown;
    private float m_LaserCooldown;
    private bool m_CanSkipTurn = false;


    [Header("Test Data")]
    private PiecesData m_PiecesData;

    //Turn annoucement UI
    private UIPlayerTurnAnnouncement m_Announcement;

    //Piece Update UI
    private UISkipTurnButton m_TurnButton;
    private PieceUpdate m_PieceUpdate;

    public TurnManager(DataManager dataManager)
    {
        DM = dataManager;
        m_SkipTurnCooldown = DM.Rules.SkipTurnCooldown;
        m_LaserCooldown = DM.Rules.LaserCooldown;
        m_Announcement = FindObjectOfType<UIPlayerTurnAnnouncement>();
        m_PiecesData = FindObjectOfType<PiecesData>();
        m_TurnButton = FindObjectOfType<UISkipTurnButton>();
        m_PieceUpdate = FindObjectOfType<PieceUpdate>();
    }

    public void Start()
    {
        StartAnnouncementPhase();
    }

    public void PrepareSkipTurn(int playerID)
    {
        if (m_CanSkipTurn)
        {
            DM.GameMessageManager.TrySkipTurnServerRPC(playerID);
        }
    }

    public void StartLaserPhase()
    {
        m_CanSkipTurn = false;
        DM.LaserManager.UpdateLaser(false);
        m_TurnButton.StartCoroutineCooldownFromScriptable(m_LaserCooldown, true);
    }

    public void StartAnnouncementPhase()
    {
        if (DM.GameMode.CheckGameOver())
        {
            return;
        }
        DM.PlayersManager.StartNextPlayerTurn(++m_TurnNumber);
        DM.LaserManager.UpdateLaser(true);
        m_PieceUpdate.UpdatePieces();
        m_TurnButton.StartCoroutineCooldownFromScriptable(m_SkipTurnCooldown, false);
        m_Announcement.StartCoroutineTurnAnnouncementFadeFromScriptable(m_SkipTurnCooldown);
    }

    public void StartTurnPhase()
    {
        DM.LaserManager.UpdateLaser(true);
        m_CanSkipTurn = true;
    }
}
