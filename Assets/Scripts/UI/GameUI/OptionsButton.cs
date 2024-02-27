using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsButton : MonoBehaviour
{
    [SerializeField]
    private GameObject Options;

    public void OnClick()
    {
        Options.SetActive(!Options.activeSelf);
        //LocalPlayerManager.Instance.CreateSurrenderAction();
    }
}
