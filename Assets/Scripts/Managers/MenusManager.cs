#if !DEDICATED_SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Menus
{
    Connection,
    Main,
    GameMode, 
    Matchmaking,
    Options,
    GameOver
}

public class MenusManager : MonoBehaviour
{
    public static MenusManager Instance { get; private set; }

    [field: SerializeField]
    private GameObject m_ConnectionMenu;
    [field: SerializeField]
    private GameObject m_MainMenu;
    [field: SerializeField]
    private GameObject m_GamemodeMenu;
    [field: SerializeField]
    private GameObject m_MatchmakingMenu;
    [field: SerializeField]
    private GameObject m_OptionsMenu;
    [field: SerializeField]
    private GameObject m_GameOverMenu;

    private bool m_CanInteractWithUI = true;

    private Dictionary<Menus, GameObject> m_Menus;

    private Menus m_CurrentMenus = Menus.Connection;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_Menus = new Dictionary<Menus, GameObject>();
        m_Menus[Menus.Connection] = m_ConnectionMenu;
        m_Menus[Menus.Main] = m_MainMenu;
        m_Menus[Menus.GameMode] = m_GamemodeMenu;
        m_Menus[Menus.Matchmaking] = m_MatchmakingMenu;
        m_Menus[Menus.Options] = m_OptionsMenu;
        m_Menus[Menus.GameOver] = m_GameOverMenu;
    }

    public void ChangeMenu(Menus newMenu)
    {
        m_Menus[m_CurrentMenus].SetActive(false);
        m_Menus[newMenu].SetActive(true);
        m_CurrentMenus = newMenu;

        m_CanInteractWithUI = true;
    }

    public bool TryInteractWithUI()
    {
        if (m_CanInteractWithUI)
        {
            m_CanInteractWithUI = false;
            return true;
        }

        return false;
    }

    public void StopChange()
    {
        m_CanInteractWithUI = true;
    }
}
#endif