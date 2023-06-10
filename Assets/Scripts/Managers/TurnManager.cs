using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private int m_TurnNumber = 0;
    private DataManager m_DataManager;
    private PlayersManager m_PlayersManager;

    [Header("Time Management")]
    [SerializeField]
    private float m_SkipTurnCooldown;
    [SerializeField]
    private float m_LaserCooldown;
    private float m_CurrentCooldown = 0;
    public bool m_CanSkipTurn { get; set; } = true;


    [Header("Test Data")]
    private PiecesData m_PiecesData;

    //Turn annoucement UI
    private UIPlayerTurnAnnouncement m_Announcement;

    //Piece Update UI
    private UIPieceUpdate m_UIPieceUpdate;
    private UISkipTurnButton m_TurnButton;

    void Awake()
    {
    }

    void Start()
    {
        m_DataManager = FindObjectOfType<DataManager>();
        m_PlayersManager = m_DataManager.PlayersManager;
        m_SkipTurnCooldown = m_DataManager.Rules.SkipTurnCooldown;
        m_Announcement = FindObjectOfType<UIPlayerTurnAnnouncement>();
        m_UIPieceUpdate = FindObjectOfType<UIPieceUpdate>();
        m_PiecesData = FindObjectOfType<PiecesData>();
        m_TurnButton = FindObjectOfType<UISkipTurnButton>();
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
            StartCoroutine(m_TurnButton.Cooldown(m_LaserCooldown, true));
            m_DataManager.LaserManager.DestroyLaserPart();
            m_DataManager.LaserManager.PrintLaserPart(false);
        }
        else
        {
            EndOfLaser(firstTurn);
        }
        

    }
    public void EndOfLaser(bool firstTurn)
    {
        
        if (!firstTurn)
        {
            m_PlayersManager.CallNextPlayer(++m_TurnNumber);
        }
        StartCoroutine(m_TurnButton.Cooldown(m_SkipTurnCooldown, false));

        StartCoroutine(m_Announcement.TurnAnnouncementFade(m_SkipTurnCooldown));
        m_UIPieceUpdate.UpdatePieces();
        m_DataManager.LaserManager.DestroyLaserPart();
        m_DataManager.LaserManager.PrintLaserPart(true);

        m_CanSkipTurn = false;
    }
}
