using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    [Header("Player Number")]
    [SerializeField, Tooltip("Id of the current player, < m_PlayerNumber")]
    private int m_CurrentIDPlayer = 0;
    [SerializeField] 
    private int m_PlayerNumber = 2;

    [Header("Time Management")]
    [SerializeField]
    private float m_SkipTurnCooldown = 5;
    private float m_CurrentCooldown = 0;
    private bool m_CanSkipTurn = true;

    [Header("Test Data")]
    [SerializeField]
    private PiecesData m_PiecesData;

    //Turn annoucement UI
    private UIPlayerTurnAnnouncement m_Announcement;


    private void Start()
    {
       m_Announcement = FindObjectOfType<UIPlayerTurnAnnouncement>();
       SkipTurn(true);

    }
    public void TrySkipTurn()
    {
        if (m_CanSkipTurn)
        {
            SkipTurn(false);
        }
    }


    private void SkipTurn(bool firstTurn)
    {
        if (!firstTurn)
        {
            m_CurrentIDPlayer = (m_CurrentIDPlayer + 1) % m_PlayerNumber;
        }
        StartCoroutine(SkipTurnCooldown());
        StartCoroutine(m_Announcement.TurnAnnouncementFade(m_SkipTurnCooldown));
        Debug.Log(m_PiecesData.GetRandomPiece().name);
        
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


    //Getters && Setters

    public int GetCurrentIDPlayer()
    {
        return m_CurrentIDPlayer;
    }

    public bool GetCanSkipTurn()
    {
        return m_CanSkipTurn;
    }

    
}
