using UnityEngine;
using System;
using System.Collections;
#nullable enable

public sealed class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    private int m_TurnNumber = 0;

    [Header("Time Management")]
    private float m_SkipTurnCooldown;
    private float m_LaserCooldown;
    private bool m_CanSkipTurn = false;
    private float m_SkipTurnCurrentCooldown = 0;

    [Header("Test Data")]
    private PiecesData m_PiecesData;

    //Turn annoucement UI
    private PlayerTurnAnnouncementUI m_Announcement;

    //Piece Update UI
    private SkipTurnButton m_TurnButton;
    private SelectionTilesUpdate m_SelectionTilesUpdate;

    void Awake()
    {
        Instance = this;
        DataManager DM = DataManager.Instance;
        m_SkipTurnCooldown = DataManager.Instance.Rules.SkipTurnCooldown;
        m_LaserCooldown = DataManager.Instance.Rules.LaserCooldown;
        m_Announcement = FindObjectOfType<PlayerTurnAnnouncementUI>();
        m_PiecesData = FindObjectOfType<PiecesData>();
        m_TurnButton = FindObjectOfType<SkipTurnButton>();
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
        m_CanSkipTurn = false;
        LaserManager.Instance.UpdateLaser(false);
        StartCoroutine(Cooldown(true));
    }

    public void StartAnnouncementPhase()
    { 
        if (DataManager.Instance.GameMode.CheckGameOver())
        {
            return;
        }
        PlayersManager.Instance.StartNextPlayerTurn(++m_TurnNumber);
        LaserManager.Instance.UpdateLaser(true);
        if (GameInitialParameters.localPlayerID == -1) m_SelectionTilesUpdate.ServerUpdateSelectionPieces();
        StartCoroutine(Cooldown(false));
        m_Announcement.TurnAnnouncementActivation(m_SkipTurnCooldown);
    }

    public void StartTurnPhase()
    {
        LaserManager.Instance.UpdateLaser(true);
        m_CanSkipTurn = true;
    }

    public IEnumerator Cooldown(bool laser)
    {
        m_TurnButton.BeginCooldown(laser);
        m_SkipTurnCurrentCooldown = m_SkipTurnCooldown;
        while (m_SkipTurnCurrentCooldown > 0)
        {
            m_SkipTurnCurrentCooldown -= Time.deltaTime;
            yield return null;
        }

        m_TurnButton.EndCooldown();
        if (laser)
        {
            StartAnnouncementPhase();
        }
        else
        {
            StartTurnPhase();
        }
    }
}
