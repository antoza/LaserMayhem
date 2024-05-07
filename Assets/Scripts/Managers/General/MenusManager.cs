#if !DEDICATED_SERVER

using System.Collections.Generic;
using UnityEngine;

using AYellowpaper.SerializedCollections;

public enum Menus
{
    None = 0,
    Connection = 10,
    Main = 100,
    GameModes = 101,
    Challenges = 102,
    Matchmaking = 150,
    Options = 200,
    BackgroundChoice = 210,
    TutorialList = 300,
    TutorialGeneral = 301,
    TutorialPieceList = 302,
}

public class MenusManager : MonoBehaviour
{
    public static MenusManager Instance { get; private set; }

    [SerializedDictionary("Menu Name", "Object")]
    public SerializedDictionary<Menus, GameObject> MenusDictionnary;

    public SkinData SkinData { get; private set; }

    private Menus m_CurrentMenus = Menus.None;
    private bool m_CanInteractWithUI = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (AccountInfo.isConnected)
        {
            ChangeMenu(Menus.Main);
        }
    }

    public void ChangeMenu(Menus newMenu)
    {
        if (MenusDictionnary.ContainsKey(m_CurrentMenus))
        {
            MenusDictionnary[m_CurrentMenus].SetActive(false);
        }
        MenusDictionnary[newMenu].SetActive(true);
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