#if !DEDICATED_SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameButton : MenuButton
{
    public override void DoOnClick()
    {
        MenusManager.Instance.ChangeMenu(Menus.GameMode);
    }
}
#endif