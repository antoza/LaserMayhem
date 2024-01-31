#if !DEDICATED_SERVER

using System.Collections.Generic;
using UnityEngine;

using AYellowpaper.SerializedCollections;

public enum Menus
{
    None,
    Connection,
    Main,
    GameMode, 
    Matchmaking,
    Options,
    GameOver,
    BackgroundChoice,
    TutorialList,
    TutorialGeneral,
    TutorialPieceList
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
        if (PlayerProfile.isConnected)
        {
            ChangeMenu(Menus.Main);
        }
    }

    public void ChangeMenu(Menus newMenu)
    {
        MenusDictionnary[m_CurrentMenus].SetActive(false);
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