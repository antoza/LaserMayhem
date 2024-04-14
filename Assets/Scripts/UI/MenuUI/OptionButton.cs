#if !DEDICATED_SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButton : MenuButton
{
    public override void DoOnClick()
    {
        OnButtonPressed(Menus.Options);
    }
}
#endif