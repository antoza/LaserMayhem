#if !DEDICATED_SERVER

using System.Collections.Generic;
using UnityEngine;

using AYellowpaper.SerializedCollections;

public enum Menus
{
    None = 0,
    Connection = 10,
    Main = 100,
    GameMode = 101,
    ChallengeSelection = 102,
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

    [SerializeField]
    private bool IsShredderVersion = false;

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
        
        if(IsShredderVersion)
        {
            ChangeMenu(Menus.Main);
        }
    }

    public void ChangeMenu(Menus newMenu)
    {
        if(m_CurrentMenus != Menus.None)
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