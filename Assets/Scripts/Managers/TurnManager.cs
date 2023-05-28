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
    private float m_CurrentCooldown = 0;
    public bool m_CanSkipTurn { get; private set; } = true;

    [Header("Test Data")]
    private PiecesData m_PiecesData;

    //Turn annoucement UI
    private UIPlayerTurnAnnouncement m_Announcement;

    //Piece Update UI
    private UIPieceUpdate m_UIPieceUpdate;

    void Awake()
    {
        m_DataManager = FindObjectOfType<DataManager>();
        m_PlayersManager = m_DataManager.PlayersManager;
        m_SkipTurnCooldown = m_DataManager.Rules.SkipTurnCooldown;
        m_Announcement = FindObjectOfType<UIPlayerTurnAnnouncement>();
        m_UIPieceUpdate = FindObjectOfType<UIPieceUpdate>();
        m_PiecesData = FindObjectOfType<PiecesData>();
    }

    void Start()
    {
        SkipTurn();
    }

    public bool TrySkipTurn()
    {
        if (m_CanSkipTurn)
        {
            SkipTurn();
            return true;
        }
        return false;
    }


    private void SkipTurn()
    {
        m_PlayersManager.CallNextPlayer(++m_TurnNumber);

        StartCoroutine(SkipTurnCooldown());
        StartCoroutine(m_Announcement.TurnAnnouncementFade(m_SkipTurnCooldown));
        m_UIPieceUpdate.UpdatePieces();
        
        m_CanSkipTurn = false;
    }


    private IEnumerator SkipTurnCooldown()
    {
        m_CurrentCooldown = m_SkipTurnCooldown;
        while (m_CurrentCooldown > 0)
        {
            m_CurrentCooldown -= Time.deltaTime;
            yield return null;
        }

        m_CanSkipTurn = true;
    }
}
