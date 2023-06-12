using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class TurnManager : ScriptableObject
{
    private int m_TurnNumber = 0;
    private DataManager m_DataManager;
    private PlayersManager m_PlayersManager;

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
    private UIPieceUpdate m_UIPieceUpdate;
    private UISkipTurnButton m_TurnButton;

    public TurnManager(DataManager dataManager, PlayersManager playersManager, float skipTurnCooldown, float laserCooldown)
    {
        m_DataManager = dataManager;
        m_PlayersManager = playersManager;
        m_SkipTurnCooldown = m_DataManager.Rules.SkipTurnCooldown;
        m_Announcement = FindObjectOfType<UIPlayerTurnAnnouncement>();
        m_UIPieceUpdate = FindObjectOfType<UIPieceUpdate>();
        m_PiecesData = FindObjectOfType<PiecesData>();
        m_TurnButton = FindObjectOfType<UISkipTurnButton>();
        m_SkipTurnCooldown = skipTurnCooldown;
        m_LaserCooldown = laserCooldown;
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
            m_DataManager.LaserManager.DestroyLaserPart();
            m_DataManager.LaserManager.PrintLaserPart(false);
        }
        else
        {
            EndOfLaser();
        }
        

    }
    public void EndOfLaser()
    {
        m_PlayersManager.StartNextPlayerTurn(++m_TurnNumber);
        m_TurnButton.StartCoRoutineCooldownFromScriptable(m_LaserCooldown, false);

        m_Announcement.StartCoRoutineTurnAnnouncementFadeFromScriptable(m_SkipTurnCooldown);
        m_UIPieceUpdate.UpdatePieces();
        m_DataManager.LaserManager.DestroyLaserPart();
        m_DataManager.LaserManager.PrintLaserPart(true);

        m_CanSkipTurn = false;
    }
}
