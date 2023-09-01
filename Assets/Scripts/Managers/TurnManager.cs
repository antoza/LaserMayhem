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
    [SerializeField]
    private float m_SkipTurnCooldown;
    [SerializeField]
    private float m_LaserCooldown;
    public bool m_CanSkipTurn { get; set; } = true;


    [Header("Test Data")]
    private PiecesData m_PiecesData;

    //Turn annoucement UI
    private UIPlayerTurnAnnouncement m_Announcement;

    //Piece Update UI
    private UISkipTurnButton m_TurnButton;
    private PieceUpdate m_PieceUpdate;

    public TurnManager(DataManager dataManager, float skipTurnCooldown, float laserCooldown)
    {
        DM = dataManager;
        m_SkipTurnCooldown = DM.Rules.SkipTurnCooldown;
        m_Announcement = FindObjectOfType<UIPlayerTurnAnnouncement>();
        m_PiecesData = FindObjectOfType<PiecesData>();
        m_TurnButton = FindObjectOfType<UISkipTurnButton>();
        m_SkipTurnCooldown = skipTurnCooldown;
        m_LaserCooldown = laserCooldown;
        m_PieceUpdate = FindObjectOfType<PieceUpdate>();
    }

    public void Start()
    {
        m_TurnButton.TurnManager = this;
        SkipTurn(true);
    }

    public bool TrySkipTurn(bool firstTurn)
    {
        if (m_CanSkipTurn)
        {
            SkipTurn(firstTurn);
            return true;
        }
        return false;
    }


    private void SkipTurn(bool firstTurn)
    {
        EndOfTurn(firstTurn);
    }

    public void EndOfTurn(bool firstTurn)
    {
        if (!firstTurn)
        {
            m_TurnButton.StartCoRoutineCooldownFromScriptable(m_LaserCooldown, true);
            DM.LaserManager.UpdateLaser(false);
        }
        else
        {
            EndOfLaser();
        }
        

    }
    public void EndOfLaser()
    {
        DM.PlayersManager.StartNextPlayerTurn(++m_TurnNumber);
        m_TurnButton.StartCoRoutineCooldownFromScriptable(m_LaserCooldown, false);
        m_PieceUpdate.UpdatePieces();
        m_Announcement.StartCoRoutineTurnAnnouncementFadeFromScriptable(m_SkipTurnCooldown);
        DM.LaserManager.UpdateLaser(true);

        m_CanSkipTurn = false;
    }
}
