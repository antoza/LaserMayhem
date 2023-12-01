using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Menus
{
    Connection,
    Main,
    GameMode, 
    Options,
    GameOver
}

public class MenuSelection : MonoBehaviour
{
    public static MenuSelection Instance { get; private set; }

    [field: SerializeField]
    private GameObject m_ConnectionMenu;
    [field: SerializeField]
    private GameObject m_MainMenu;
    [field: SerializeField]
    private GameObject m_GamemodeMenu;
    [field: SerializeField]
    private GameObject m_OptionsMenu;
    [field: SerializeField]
    private GameObject m_GameOverMenu;

    private bool m_ChangeOnGoing = false;

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
        m_Menus[Menus.Options] = m_OptionsMenu;
        m_Menus[Menus.GameOver] = m_GameOverMenu;
    }

    public void ChangeMenu(Menus newMenu)
    {
        m_Menus[m_CurrentMenus].SetActive(false);
        m_Menus[newMenu].SetActive(true);
        m_CurrentMenus = newMenu;

        m_ChangeOnGoing = false;
    }

    public bool StartChange()
    {
        if (!m_ChangeOnGoing)
        {
            m_ChangeOnGoing = true;
            return true;
        }

        return false;
    }
}
