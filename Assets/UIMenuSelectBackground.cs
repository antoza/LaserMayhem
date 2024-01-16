#if !DEDICATED_SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuSelectBackground : MonoBehaviour
{
    [field: SerializeField]
    private string BackgroundName;

    [field: SerializeField]
    private Image BackgroundImage;

    [field: SerializeField]
    private GameObject Selectionner;

    public void Start()
    {
        if(MenusManager.Instance.SkinData && MenusManager.Instance.SkinData.BackgroundSkin.ContainsKey(BackgroundName))
        {
            BackgroundImage.sprite = MenusManager.Instance.SkinData.BackgroundSkin[BackgroundName];
        }
        else
        {
            Debug.Log("The background name " + BackgroundName + " doesn't exist in SkinData");
        }
    }
    public void OnClick()
    {
        BackgroundManager.Instance.SelectNewBackground(BackgroundName);
        Selectionner.SetActive(true);
    }

    public void DisableSelectionner()
    {
        Selectionner.SetActive(false);
    }
}
#endif