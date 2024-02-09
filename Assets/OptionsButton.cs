using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsButton : GameButton
{
    public override void OnClick()
    {
        base.OnClick();
        //LocalPlayerManager.Instance.CreateSurrenderAction();
    }
}
