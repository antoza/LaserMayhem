using UnityEngine;
using System;
#nullable enable

public sealed class TurnManager : ScriptableObject
{
    public static TurnManager? Instance { get; private set; }

    private int m_TurnNumber = 0;

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
    private SelectionTilesUpdate m_SelectionTilesUpdate;

    public TurnManager()
    {
        DataManager DM = DataManager.Instance;
        m_SkipTurnCooldown = DataManager.Instance.Rules.SkipTurnCooldown;
        m_LaserCooldown = DataManager.Instance.Rules.LaserCooldown;
        m_Announcement = FindObjectOfType<UIPlayerTurnAnnouncement>();
        m_PiecesData = FindObjectOfType<PiecesData>();
        m_TurnButton = FindObjectOfType<UISkipTurnButton>();
        m_SelectionTilesUpdate = FindObjectOfType<SelectionTilesUpdate>();
    }

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
    }

    public void Start()
    {
        // TODO : A corriger
        m_TurnButton.StartCoroutineAEFFACERAPRES();
        //StartAnnouncementPhase();
    }

    public void StartLaserPhase()
    {
        m_CanSkipTurn = false;
        LaserManager.GetInstance().UpdateLaser(false);
        m_TurnButton.StartCoroutineCooldownFromScriptable(m_LaserCooldown, true);
    }

    public void StartAnnouncementPhase()
    {
        if (DataManager.Instance.GameMode.CheckGameOver())
        {
            return;
        }
        PlayersManager.GetInstance().StartNextPlayerTurn(++m_TurnNumber);
        LaserManager.GetInstance().UpdateLaser(true);
        if (GameInitialParameters.localPlayerID == -1) m_SelectionTilesUpdate.ServerUpdateSelectionPieces();
        m_TurnButton.StartCoroutineCooldownFromScriptable(m_SkipTurnCooldown, false);
        m_Announcement.StartCoroutineTurnAnnouncementFadeFromScriptable(m_SkipTurnCooldown);
    }

    public void StartTurnPhase()
    {
        LaserManager.GetInstance().UpdateLaser(true);
        m_CanSkipTurn = true;
    }
}
