#if !DEDICATED_SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButton : MenuButton
{
    public override void DoOnClick()
    {
        MenusManager.Instance.ChangeMenu(Menus.Options);
    }
}
#endif