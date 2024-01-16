#if !DEDICATED_SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager Instance;

    [field: SerializeField]
    private List<UIMenuSelectBackground> BackgroundSelectionButtons;
    void Start()
    {
        Instance = this;
    }

    public void SelectNewBackground(string backgroundName)
    {
        PlayerPrefs.SetString("Background Skin", backgroundName);
        foreach(UIMenuSelectBackground selector in BackgroundSelectionButtons)
        {
            selector.DisableSelectionner();
        }
    }
}

#endif