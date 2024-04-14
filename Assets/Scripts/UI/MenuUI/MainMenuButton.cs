#if !DEDICATED_SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : MenuButton
{
    public override void DoOnClick()
    {
        OnButtonPressed(Menus.Main);
    }
}
#endif